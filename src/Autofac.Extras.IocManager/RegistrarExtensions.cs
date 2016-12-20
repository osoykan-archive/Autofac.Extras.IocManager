using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public static class RegistrarExtensions
    {
        /// <summary>
        ///     Registers the assembly by convention.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="assembly">The assembly.</param>
        public static void RegisterAssemblyByConvention(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterDependenciesByAssembly<ISingletonDependency>(assembly);
            builder.RegisterDependenciesByAssembly<ITransientDependency>(assembly);
            builder.RegisterDependenciesByAssembly<ILifetimeScopeDependency>(assembly);
        }
    }
}
