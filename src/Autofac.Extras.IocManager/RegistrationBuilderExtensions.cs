using System;

using Autofac.Builder;

namespace Autofac.Extras.IocManager
{
    public static class RegistrationBuilderExtensions
    {
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
    }
}
