using System;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public static class IocBuilderExtensions
    {
        /// <summary>
        ///     Uses the autofac container builder.
        /// </summary>
        /// <param name="iocBuilder">The ioc builder.</param>
        /// <returns></returns>
        public static IIocBuilder UseAutofacContainerBuilder(this IIocBuilder iocBuilder)
        {
            return iocBuilder.UseAutofacContainerBuilder(new ContainerBuilder());
        }

        /// <summary>
        ///     Uses the autofac container builder.
        /// </summary>
        /// <param name="iocBuilder">The ioc builder.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IIocBuilder UseAutofacContainerBuilder(this IIocBuilder iocBuilder, ContainerBuilder containerBuilder)
        {
            return iocBuilder.UseServiceRegistration(new AutofacServiceRegistration(containerBuilder));
        }

        /// <summary>
        ///     Registers the assembly by convention.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static IIocBuilder RegisterAssemblyByConvention(this IIocBuilder builder, Assembly assembly)
        {
            return builder.RegisterAssemblyByConvention(assembly);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="iocBuilder">The ioc builder.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Make sure to configure the IocBuilder for Autofac using the
        ///     .UseAutofacContainerBuilder(...)
        /// </exception>
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
