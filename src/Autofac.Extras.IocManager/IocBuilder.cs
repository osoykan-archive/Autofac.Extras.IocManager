using System;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.IIocBuilder" />
    public class IocBuilder : IIocBuilder
    {
        /// <summary>
        ///     The lazy registration factory
        /// </summary>
        private Lazy<IServiceRegistration> _lazyRegistrationFactory = new Lazy<IServiceRegistration>(() => new ServiceRegistration());

        /// <summary>
        ///     Prevents a default instance of the <see cref="IocBuilder" /> class from being created.
        /// </summary>
        private IocBuilder()
        {
            UseServiceRegistration(new ServiceRegistration());
            ModuleRegistration = new ModuleRegistration(this);
        }

        /// <summary>
        ///     Gets the new.
        /// </summary>
        /// <value>
        ///     The new.
        /// </value>
        public static IocBuilder New => new IocBuilder();

        /// <summary>
        ///     Gets the module registration.
        /// </summary>
        /// <value>
        ///     The module registration.
        /// </value>
        public IModuleRegistration ModuleRegistration { get; }

        /// <summary>
        ///     Registers the services.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        public IIocBuilder RegisterServices(Action<IServiceRegistration> register)
        {
            register(_lazyRegistrationFactory.Value);
            return this;
        }

        /// <summary>
        ///     Registers the module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <returns></returns>
        public IIocBuilder RegisterModule<TModule>()
            where TModule : IModule, new()
        {
            ModuleRegistration.Register<TModule>();
            return this;
        }

        /// <summary>
        ///     Registers the module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        public IIocBuilder RegisterModule<TModule>(TModule module)
            where TModule : IModule
        {
            ModuleRegistration.Register(module);
            return this;
        }

        /// <summary>
        ///     Uses the service registration.
        /// </summary>
        /// <param name="serviceRegistration">The service registration.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Service registration is already in use</exception>
        public IIocBuilder UseServiceRegistration(IServiceRegistration serviceRegistration)
        {
            if (_lazyRegistrationFactory != null && _lazyRegistrationFactory.IsValueCreated)
            {
                throw new InvalidOperationException("Service registration is already in use");
            }

            RegisterDefaults(serviceRegistration);
            _lazyRegistrationFactory = new Lazy<IServiceRegistration>(() => serviceRegistration);

            return this;
        }

        /// <summary>
        ///     Creates the resolver.
        /// </summary>
        /// <returns></returns>
        public IRootResolver CreateResolver()
        {
            return _lazyRegistrationFactory.Value.CreateResolver();
        }

        /// <summary>
        ///     Registers the defaults.
        /// </summary>
        /// <param name="serviceRegistration">The service registration.</param>
        private void RegisterDefaults(IServiceRegistration serviceRegistration)
        {
            serviceRegistration.Register(_ => ModuleRegistration, Lifetime.Singleton);
        }
    }
}
