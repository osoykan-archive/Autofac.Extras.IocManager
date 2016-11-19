using System;
using System.Reflection;

using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Extras.IocManager
{
    public static class RegistrationBuilderExtensions
    {
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

        private static bool IsInjectable(this IPropertySelector propertySelector, PropertyInfo propertyInfo, object instance)
        {
            return propertySelector.InjectProperty(propertyInfo, instance);
        }

        public static void
                ApplyLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(
                    this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, Type lifetimeType)
        {
            if (lifetimeType == typeof(ISingletonDependency))
            {
                registration.SingleInstance();
            }

            if (lifetimeType == typeof(ILifetimeScopeDependency))
            {
                registration.InstancePerLifetimeScope();
            }

            if (lifetimeType == typeof(ITransientDependency))
            {
                registration.InstancePerDependency();
            }

            if (lifetimeType == typeof(IPerRequestDependency))
            {
                registration.InstancePerRequest();
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

            foreach (var propertyInfo in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!propertySelector.IsInjectable(propertyInfo, instance))
                {
                    continue;
                }

                var propertyType = propertyInfo.PropertyType;

                if (IsInjectable(context, propertyInfo, propertyType))
                {
                    var accessors = propertyInfo.GetAccessors(true);

                    if (IsInjectable(instance, overrideSetValues, propertyInfo, accessors))
                    {
                        var obj = context.Resolve(propertyType);
                        propertyInfo.SetValue(instance, obj, null);
                    }
                }
            }
        }

        private static bool IsInjectable(object instance, bool overrideSetValues, PropertyInfo propertyInfo, MethodInfo[] accessors)
        {
            return ((accessors.Length != 1) || !(accessors[0].ReturnType != typeof(void)))
                   && (overrideSetValues || (accessors.Length != 2) || (propertyInfo.GetValue(instance, null) == null));
        }

        private static bool IsInjectable(IComponentContext context, PropertyInfo propertyInfo, Type propertyType)
        {
            return (!propertyType.IsValueType || propertyType.IsEnum) && (propertyInfo.GetIndexParameters().Length == 0) && context.IsRegistered(propertyType);
        }
    }
}
