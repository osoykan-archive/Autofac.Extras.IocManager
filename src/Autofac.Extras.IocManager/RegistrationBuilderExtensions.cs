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
        /// <typeparam name="TLimit">The type of the limit.</typeparam>
        /// <typeparam name="TActivatorData">The type of the activator data.</typeparam>
        /// <typeparam name="TRegistrationStyle">The type of the registration style.</typeparam>
        /// <param name="registration">The registration.</param>
        /// <returns></returns>
        [Obsolete("This will be removed next major release, use Autofac's PropertiesAutoWired(attributepropertyselector) method instead.")]
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            InjectPropertiesAsAutowired<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
            return registration.OnActivated(args => InjectProperties(args.Context, args.Instance, true, new DoNotInjectAttributePropertySelector()));
        }

        /// <summary>
        ///     Injects properties as autowired with considering <see cref="DoNotInjectAttribute" />
        /// </summary>
        /// <typeparam name="TLimit">The type of the limit.</typeparam>
        /// <typeparam name="TActivatorData">The type of the activator data.</typeparam>
        /// <typeparam name="TRegistrationStyle">The type of the registration style.</typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="allowCircularPropertyInjection">if set to <c>true</c> [allow circular property injection].</param>
        /// <returns></returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            WithPropertyInjection<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, bool allowCircularPropertyInjection = true)
        {
            return registration.PropertiesAutowired(new DoNotInjectAttributePropertySelector(), allowCircularPropertyInjection);
        }

        /// <summary>
        ///     Determines whether the specified property information is injectable.
        /// </summary>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified property information is injectable; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsInjectable(this IPropertySelector propertySelector, PropertyInfo propertyInfo, object instance)
        {
            return propertySelector.InjectProperty(propertyInfo, instance);
        }

        /// <summary>
        ///     Applies the life style.
        /// </summary>
        /// <typeparam name="TLimit">The type of the limit.</typeparam>
        /// <typeparam name="TActivatorData">The type of the activator data.</typeparam>
        /// <typeparam name="TRegistrationStyle">The type of the registration style.</typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="lifetimeType">Type of the lifetime.</param>
        internal static void
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
        }

        /// <summary>
        ///     Applies the life style.
        /// </summary>
        /// <typeparam name="TLimit">The type of the limit.</typeparam>
        /// <typeparam name="TActivatorData">The type of the activator data.</typeparam>
        /// <typeparam name="TRegistrationStyle">The type of the registration style.</typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="lifeTime">The life time.</param>
        internal static void
            ApplyLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, Lifetime lifeTime)
        {
            if (lifeTime == Lifetime.Singleton)
            {
                registration.SingleInstance();
            }

            if (lifeTime == Lifetime.LifetimeScope)
            {
                registration.InstancePerLifetimeScope();
            }

            if (lifeTime == Lifetime.Transient)
            {
                registration.InstancePerDependency();
            }
        }

        /// <summary>
        ///     Injects the properties.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="overrideSetValues">if set to <c>true</c> [override set values].</param>
        /// <param name="propertySelector">The property selector.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     context
        ///     or
        ///     instance
        ///     or
        ///     propertySelector
        /// </exception>
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

            foreach (PropertyInfo propertyInfo in instance.GetType().GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!propertySelector.IsInjectable(propertyInfo, instance))
                {
                    continue;
                }

                Type propertyType = propertyInfo.PropertyType;

                if (IsInjectable(context, propertyInfo, propertyType))
                {
                    MethodInfo[] accessors = propertyInfo.GetAccessors(true);

                    if (IsInjectable(instance, overrideSetValues, propertyInfo, accessors))
                    {
                        object obj = context.Resolve(propertyType);
                        propertyInfo.SetValue(instance, obj);
                    }
                }
            }
        }

        /// <summary>
        ///     Determines whether the specified instance is injectable.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="overrideSetValues">if set to <c>true</c> [override set values].</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="accessors">The accessors.</param>
        /// <returns>
        ///     <c>true</c> if the specified instance is injectable; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsInjectable(object instance, bool overrideSetValues, PropertyInfo propertyInfo, MethodInfo[] accessors)
        {
            return (accessors.Length != 1 || !(accessors[0].ReturnType != typeof(void)))
                   && (overrideSetValues || accessors.Length != 2 || propertyInfo.GetValue(instance, null) == null);
        }

        /// <summary>
        ///     Determines whether the specified context is injectable.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        ///     <c>true</c> if the specified context is injectable; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsInjectable(IComponentContext context, PropertyInfo propertyInfo, Type propertyType)
        {
            return (!propertyType.GetTypeInfo().IsValueType || propertyType.GetTypeInfo().IsEnum) && propertyInfo.GetIndexParameters().Length == 0 && context.IsRegistered(propertyType);
        }
    }
}
