using System;

namespace Autofac.Extras.IocManager
{
    public static class IocManagerRegistrationExtensions
    {
        /// <summary>
        ///     Registers the ioc manager.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIocBuilder RegisterIocManager(this IIocBuilder builder)
        {
            builder.RegisterServices(f => f.Register<IIocManager, IIocResolver>(IocManager.Instance, Lifetime.Singleton));
            return builder;
        }

        /// <summary>
        ///     Registers the ioc manager.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="iocManager">The ioc manager.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">iocManager</exception>
        public static IIocBuilder RegisterIocManager(this IIocBuilder builder, IIocManager iocManager)
        {
            if (iocManager == null)
            {
                throw new ArgumentNullException(nameof(iocManager));
            }

            builder.RegisterServices(f => f.Register<IIocManager, IIocResolver>(iocManager, Lifetime.Singleton));
            return builder;
        }
    }
}
