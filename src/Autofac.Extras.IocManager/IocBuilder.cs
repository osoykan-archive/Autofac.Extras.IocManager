using System;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public class IocBuilder : IIocBuilder
    {
        private Lazy<IServiceRegistration> _lazyRegistrationFactory = new Lazy<IServiceRegistration>(() => new AutofacServiceRegistration());

        private IocBuilder()
        {
            UseServiceRegistration(new AutofacServiceRegistration());
            ModuleRegistration = new ModuleRegistration(this);
        }

        public static IocBuilder New => new IocBuilder();

        public IModuleRegistration ModuleRegistration { get; }

        public IIocBuilder RegisterAssemblyByConvention(Assembly assembly)
        {
            _lazyRegistrationFactory.Value.RegisterAssemblyByConvention(assembly);
            return this;
        }

        public IIocBuilder RegisterServices(Action<IServiceRegistration> register)
        {
            register(_lazyRegistrationFactory.Value);
            return this;
        }

        public IIocBuilder RegisterModule<TModule>()
            where TModule : IModule, new()
        {
            ModuleRegistration.Register<TModule>();
            return this;
        }

        public IIocBuilder RegisterModule<TModule>(TModule module)
            where TModule : IModule
        {
            ModuleRegistration.Register(module);
            return this;
        }

        public IIocBuilder UseServiceRegistration(IServiceRegistration serviceRegistration)
        {
            if ((_lazyRegistrationFactory != null) && _lazyRegistrationFactory.IsValueCreated)
            {
                throw new InvalidOperationException("Service registration is already in use");
            }

            RegisterDefaults(serviceRegistration);
            _lazyRegistrationFactory = new Lazy<IServiceRegistration>(() => serviceRegistration);

            return this;
        }

        public IRootResolver CreateResolver()
        {
            return _lazyRegistrationFactory.Value.CreateResolver();
        }

        private void RegisterDefaults(IServiceRegistration serviceRegistration)
        {
            serviceRegistration.Register(_ => ModuleRegistration, Lifetime.Singleton);
        }
    }
}
