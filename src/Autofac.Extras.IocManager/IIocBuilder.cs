using System;

namespace Autofac.Extras.IocManager
{
    public interface IIocBuilder
    {
        /// <summary>
        ///     Gets the module registration.
        /// </summary>
        /// <value>
        ///     The module registration.
        /// </value>
        IModuleRegistration ModuleRegistration { get; }

        /// <summary>
        ///     Registers the services.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        IIocBuilder RegisterServices(Action<IServiceRegistration> register);

        /// <summary>
        ///     Registers the module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <returns></returns>
        IIocBuilder RegisterModule<TModule>() where TModule : IModule, new();

        /// <summary>
        ///     Uses the service registration.
        /// </summary>
        /// <param name="serviceRegistration">The service registration.</param>
        /// <returns></returns>
        IIocBuilder UseServiceRegistration(IServiceRegistration serviceRegistration);

        /// <summary>
        ///     Registers the module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        IIocBuilder RegisterModule<TModule>(TModule module) where TModule : IModule;

        /// <summary>
        ///     Creates the resolver.
        /// </summary>
        /// <returns></returns>
        IRootResolver CreateResolver(bool ignoreStartableComponents = false);
    }
}
