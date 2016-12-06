using System;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public static class IocBuilderExtensions
    {
        public static IIocBuilder UseAutofacContainerBuilder(this IIocBuilder iocBuilder)
        {
            return iocBuilder.UseAutofacContainerBuilder(new ContainerBuilder());
        }

        public static IIocBuilder UseAutofacContainerBuilder(this IIocBuilder iocBuilder, ContainerBuilder containerBuilder)
        {
            return iocBuilder.UseServiceRegistration(new AutofacServiceRegistration(containerBuilder));
        }

        public static IIocBuilder RegisterAssemblyByConvention(this IIocBuilder builder, Assembly assembly)
        {
            return builder.RegisterAssemblyByConvention(assembly);
        }

        public static IContainer CreateContainer(this IIocBuilder iocBuilder)
        {
            IRootResolver rootResolver = iocBuilder.CreateResolver();
            var autofacRootResolver = rootResolver as AutofacRootResolver;
            if (autofacRootResolver == null)
            {
                throw new InvalidOperationException(
                    "Make sure to configure the IocBuilder for Autofac using the .UseAutofacContainerBuilder(...)");
            }

            return autofacRootResolver.Container;
        }
    }
}