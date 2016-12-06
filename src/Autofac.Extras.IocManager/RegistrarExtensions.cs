using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public static class RegistrarExtensions
    {
        public static void RegisterAssemblyByConvention(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterDependenciesByAssembly<ISingletonDependency>(assembly);
            builder.RegisterDependenciesByAssembly<ITransientDependency>(assembly);
            builder.RegisterDependenciesByAssembly<ILifetimeScopeDependency>(assembly);
        }
    }
}
