using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Builder;
using Autofac.Features.Scanning;

namespace Autofac.Extras.IocManager.Extensions
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
        ///     Sets current Autofac <see cref="IContainer" /> to <see cref="IocManager" />
        /// </summary>
        /// <param name="container"></param>
        public static IContainer UseIocManager(this IContainer container)
        {
            IocManager.Instance.Container = container;
            return container;
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

        public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle>
            AsDefaultInterfacesWithSelf<TLimit>(this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            return registration.As(t => (IEnumerable<Type>)t.GetDefaultInterfaceTypesWithSelf());
        }
    }
}
