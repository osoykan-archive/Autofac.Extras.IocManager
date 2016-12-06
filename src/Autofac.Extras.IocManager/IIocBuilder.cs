using System;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public interface IIocBuilder
    {
        IModuleRegistration ModuleRegistration { get; }

        IIocBuilder RegisterAssemblyByConvention(Assembly assembly);

        IIocBuilder RegisterServices(Action<IServiceRegistration> register);

        IIocBuilder RegisterModule<TModule>() where TModule : IModule, new();

        IIocBuilder UseServiceRegistration(IServiceRegistration serviceRegistration);

        IIocBuilder RegisterModule<TModule>(TModule module) where TModule : IModule;

        IRootResolver CreateResolver();
    }
}
