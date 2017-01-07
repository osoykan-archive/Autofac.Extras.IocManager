using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;

using Castle.DynamicProxy;

namespace Autofac.Extras.IocManager.DynamicProxy
{
    public static class AutofacInterceptionExtensions
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> EnableInterception<TService, TInterceptor>(
            this IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration)
            where TInterceptor : IInterceptor
        {
            return EnableInterception(registration, new Type[] { typeof(TService) }, new Type[] { typeof(TInterceptor) });
        }

        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> EnableInterception<TService>(
            this IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type[] interfacesToIntercept,
            Type[] interceptors
        )
        {
            return registration.EnableClassInterceptors(ProxyGenerationOptions.Default, interfacesToIntercept).InterceptedBy(interceptors);
        }

        public static void InterceptedBy<TInterceptor>(this IComponentRegistration registration) where TInterceptor : IInterceptor
        {
            InterceptedBy<TInterceptor>(registration, false);
        }

        public static void InterceptedBy<TInterceptor>(this IComponentRegistration registration, bool interceptAdditionalInterfaces) where TInterceptor : IInterceptor
        {
            InterceptedBy(registration, interceptAdditionalInterfaces, typeof(TInterceptor));
        }

        public static void InterceptedBy(this IComponentRegistration registration, params Type[] interceptorTypes)
        {
            InterceptedBy(registration, false, interceptorTypes);
        }

        public static void InterceptedBy(this IComponentRegistration registration, bool interceptAddtionalInterfaces, params Type[] interceptorTypes)
        {
            registration.Activating += (sender, e) => { ApplyInterception(interceptorTypes, e, interceptAddtionalInterfaces); };
        }

        private static void ApplyInterception(Type[] interceptorTypes, IActivatingEventArgs<object> e, bool interceptAdditionalInterfaces)
        {
            Type type = e.Instance.GetType();

            if (e.Component.Services.OfType<IServiceWithType>()
                 .Any(swt => !swt.ServiceType.IsVisible)
                || type.Namespace == "Castle.Proxies")
            {
                return;
            }

            Type[] proxiedInterfaces = type.GetInterfaces().Where(i => i.IsVisible).ToArray();
            if (!proxiedInterfaces.Any())
            {
                return;
            }

            // intercept with all interceptors
            Type theInterface = proxiedInterfaces.First();
            Type[] interfaces = proxiedInterfaces.Skip(1).ToArray();

            IList<IInterceptor> interceptorInstances = new List<IInterceptor>();
            foreach (Type interceptorType in interceptorTypes)
            {
                interceptorInstances.Add((IInterceptor)e.Context.Resolve(interceptorType));
            }

            if (interceptorInstances.Count > 0)
            {
                object interceptedInstance = interceptAdditionalInterfaces
                    ? generator.CreateInterfaceProxyWithTarget(theInterface, interfaces, e.Instance, interceptorInstances.ToArray())
                    : generator.CreateInterfaceProxyWithTarget(theInterface, e.Instance, interceptorInstances.ToArray());

                e.ReplaceInstance(interceptedInstance);
            }
        }

        /// <summary>
        ///     Intercept ALL registered interfaces with provided interceptors.
        ///     Override Autofac activation with a Interface Proxy.
        ///     Does not intercept classes, only interface bindings.
        /// </summary>
        /// <param name="builder">Contained builder to apply interceptions to.</param>
        /// <param name="interceptors">List of interceptors to apply.</param>
        internal static void InterceptInterfacesBy(this ContainerBuilder builder, params IInterceptor[] interceptors)
        {
            builder.RegisterCallback(componentRegistry =>
            {
                foreach (IComponentRegistration registration in componentRegistry.Registrations)
                {
                    InterceptRegistration(registration, interceptors);
                }
            });
        }

        /// <summary>
        ///     Intercept a specific component registrations.
        /// </summary>
        /// <param name="registration">Component registration</param>
        /// <param name="interceptors">List of interceptors to apply.</param>
        internal static void InterceptRegistration(IComponentRegistration registration, params IInterceptor[] interceptors)
        {
            // proxy does not get allong well with Activated event and registrations with Activated events cannot be proxied.
            // They are casted to LimitedType in the IRegistrationBuilder OnActivated method. This is the offending Autofac code:
            // 
            // public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnActivated(Action<IActivatedEventArgs<TLimit>> handler)
            // {
            //    if (handler == null) throw new ArgumentNullException("handler");
            //    RegistrationData.ActivatedHandlers.Add(
            //        (s, e) => handler(new ActivatedEventArgs<TLimit>(e.Context, e.Component, e.Parameters, (TLimit)e.Instance)));
            //    return this;
            // }
            Delegate[] handlers = GetActivatedEventHandlers(registration);
            if (handlers.Any(h => handlers[0].Method.DeclaringType.Namespace.StartsWith("Autofac")))
            {
                return;
            }

            registration.Activating += (sender, e) =>
            {
                Type type = e.Instance.GetType();

                if (e.Component.Services.OfType<IServiceWithType>().Any(swt => !swt.ServiceType.IsInterface || !swt.ServiceType.IsVisible) ||

                    // prevent proxying the proxy 
                    type.Namespace == "Castle.Proxies")
                {
                    return;
                }

                Type[] proxiedInterfaces = type.GetInterfaces().Where(i => i.IsVisible).ToArray();

                if (!proxiedInterfaces.Any())
                {
                    return;
                }

                // intercept with all interceptors
                Type theInterface = proxiedInterfaces.First();
                Type[] interfaces = proxiedInterfaces.Skip(1).ToArray();

                e.Instance = generator.CreateInterfaceProxyWithTarget(theInterface, interfaces, e.Instance, interceptors);
            };
        }

        /// <summary>
        ///     Get Activated event handlers for a registrations
        /// </summary>
        /// <param name="registration">Registration to retrieve events from</param>
        /// <returns>Array of delegates in the event handler</returns>
        private static Delegate[] GetActivatedEventHandlers(IComponentRegistration registration)
        {
            FieldInfo eventHandlerField = registration.GetType().GetField("Activated", BindingFlags.NonPublic | BindingFlags.Instance);
            object registrations = eventHandlerField.GetValue(registration);
            Debug.WriteLine(registrations);
            return registrations.GetType().GetMethod("GetInvocationList").Invoke(registrations, null) as Delegate[];
        }
    }
}
