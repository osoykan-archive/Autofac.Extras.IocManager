using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Builder;
using Autofac.Features.Scanning;

namespace Autofac.Extras.IocManager
{
    public static class AutofacExtensions
    {
        /// <summary>
        ///     Registers <see cref="IocManager" /> to resolve in any dependencies.
        /// </summary>
        /// <param name="builder"></param>
        public static ContainerBuilder RegisterIocManager(this ContainerBuilder builder)
        {
            builder.RegisterInstance(IocManager.Instance)
                   .As<IIocManager, IIocResolver>()
                   .AsSelf()
                   .InjectPropertiesAsAutowired()
                   .SingleInstance();

            return builder;
        }

        /// <summary>
        ///     Registers <see cref="IocManager" /> to resolve in any dependencies.
        /// </summary>
        /// <param name="builder">Autofac's <see cref="ContainerBuilder" /></param>
        /// <param name="iocManager">IocManager abstraction for Autofac <see cref="IocManager" /></param>
        public static ContainerBuilder RegisterIocManager(this ContainerBuilder builder, IocManager iocManager)
        {
            builder.RegisterInstance(iocManager)
                   .As<IIocManager, IIocResolver>()
                   .AsSelf()
                   .InjectPropertiesAsAutowired()
                   .SingleInstance();

            return builder;
        }

        /// <summary>
        ///     Sets current Autofac <see cref="IContainer" /> to <see cref="IocManager" />
        /// </summary>
        /// <param name="container"></param>
        public static IContainer UseIocManager(this IContainer container)
        {
            IocManager.Instance.Container = container;
            return container;
        }

        /// <summary>
        ///     Sets current Autofac <see cref="IContainer" /> to <see cref="IocManager" />
        /// </summary>
        /// <param name="container"></param>
        public static IContainer UseIocManager(this IContainer container, IocManager iocManager)
        {
            iocManager.Container = container;
            return container;
        }

        /// <summary>
        ///     Helper for anonymouse resolvings <see cref="IocManager.Resolve{T}(object)" />
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static IEnumerable<TypedParameter> GetTypedResolvingParameters(this object @this)
        {
            foreach (var propertyInfo in @this.GetType().GetProperties())
            {
                yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(@this, null));
            }
        }

        /// <summary>
        ///     Finds and registers as DefaultInterfaces to container conventionally.
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle>
                AsDefaultInterfacesWithSelf<TLimit>(this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            return registration.As(t => (IEnumerable<Type>)t.GetDefaultInterfacesWithSelf());
        }

        /// <summary>
        ///     Finds all types based <see cref="TLifetime" /> in given <see cref="Assembly" />
        /// </summary>
        /// <typeparam name="TLifetime">Lifetime of dependencies</typeparam>
        /// <param name="builder">Autofac's <see cref="ContainerBuilder" /></param>
        /// <param name="assembly">Assemby to search</param>
        internal static void RegisterDependenciesByAssembly<TLifetime>(this ContainerBuilder builder, Assembly assembly) where TLifetime : ILifetime
        {
            typeof(TLifetime)
                    .AssignedTypesInAssembly(assembly)
                    .ForEach(builder.RegisterApplyingLifetime<TLifetime>);
        }

        /// <summary>
        ///     Registers given type according to it's lifetime. Type can be generic or not.
        /// </summary>
        /// <typeparam name="TLifetime">Lifetime of dependency</typeparam>
        /// <param name="builder">Autofac's <see cref="ContainerBuilder" /></param>
        /// <param name="typeToRegister">Type to register Autofac Container</param>
        internal static void RegisterApplyingLifetime<TLifetime>(this ContainerBuilder builder, Type typeToRegister) where TLifetime : ILifetime
        {
            if (typeToRegister.IsGenericTypeDefinition)
            {
                builder.RegisterGeneric(typeToRegister)
                       .As(typeToRegister.GetDefaultInterfaces())
                       .AsSelf()
                       .InjectPropertiesAsAutowired()
                       .ApplyLifeStyle(typeof(TLifetime));
            }
            else
            {
                builder.RegisterType(typeToRegister)
                       .As(typeToRegister.GetDefaultInterfaces())
                       .AsSelf()
                       .InjectPropertiesAsAutowired()
                       .ApplyLifeStyle(typeof(TLifetime));
            }
        }
    }
}
