using System;

namespace Autofac.Extras.IocManager
{
    public static class IocManagerRegistrationExtensions
    {
        public static IIocBuilder RegisterIocManager(this IIocBuilder builder)
        {
            builder.RegisterServices(f => f.Register<IIocManager, IIocResolver, IocManager>(IocManager.Instance, Lifetime.Singleton));
            return builder;
        }

        public static IIocBuilder RegisterIocManager(this IIocBuilder builder, IocManager iocManager)
        {
            if (iocManager == null)
            {
                throw new ArgumentNullException(nameof(iocManager));
            }

            builder.RegisterServices(f => f.Register<IIocManager, IIocResolver, IocManager>(iocManager, Lifetime.Singleton));
            return builder;
        }
    }
}
