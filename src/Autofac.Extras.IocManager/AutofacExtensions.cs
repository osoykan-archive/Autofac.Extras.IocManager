using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Extras.IocManager
{
    public static class AutofacExtensions
    {
        /// <summary>
        ///     Registers <see cref="IocManager" /> to resolve in any dependencies.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterIocManager(this ContainerBuilder builder)
        {
            builder.RegisterInstance(IocManager.Instance)
                   .As<IIocManager, IIocResolver>()
                   .AsSelf()
                   .InjectPropertiesAsAutowired()
                   .SingleInstance();
        }

        /// <summary>
        ///     Sets current Autofac <see cref="IContainer" /> to <see cref="IocManager" />
        /// </summary>
        /// <param name="container"></param>
        public static void UseIocManager(this IContainer container)
        {
            IocManager.Instance.Container = container;
        }

        /// <summary>
        ///     This extension allows circular dependencies and applies <see cref="DoNotInjectAttribute" />
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            InjectPropertiesAsAutowired<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
            return registration.OnActivated(args => InjectProperties(args.Context, args.Instance, true, new DoNotInjectAttributePropertySelector()));
        }

        /// <summary>
        ///     Helper for anonymouse resolvings <see cref="IocManager.Resolve{T}(object)" />
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static IEnumerable<TypedParameter> GetTypedResolvingParameters(this object @this)
        {
            foreach (PropertyInfo propertyInfo in @this.GetType().GetProperties())
            {
                yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(@this, null));
            }
        }

        private static void InjectProperties(IComponentContext context, object instance, bool overrideSetValues, IPropertySelector propertySelector)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (propertySelector.InjectProperty(propertyInfo, instance))
                {
                    continue;
                }

                Type propertyType = propertyInfo.PropertyType;

                if ((!propertyType.IsValueType || propertyType.IsEnum) && (propertyInfo.GetIndexParameters().Length == 0) && context.IsRegistered(propertyType))
                {
                    MethodInfo[] accessors = propertyInfo.GetAccessors(true);

                    if (((accessors.Length != 1) || !(accessors[0].ReturnType != typeof(void)))
                        && (overrideSetValues || (accessors.Length != 2) || (propertyInfo.GetValue(instance, null) == null)))
                    {
                        object obj = context.Resolve(propertyType);
                        propertyInfo.SetValue(instance, obj, null);
                    }
                }
            }
        }
    }
}
