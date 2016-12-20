namespace Autofac.Extras.IocManager
{
    public interface IModuleRegistration
    {
        /// <summary>
        ///     Registers this instance.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        void Register<TModule>()
            where TModule : IModule, new();

        /// <summary>
        ///     Registers the specified module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module.</param>
        void Register<TModule>(TModule module)
            where TModule : IModule;

        /// <summary>
        ///     Gets the module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <returns></returns>
        TModule GetModule<TModule>()
            where TModule : IModule;

        /// <summary>
        ///     Tries the get module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        bool TryGetModule<TModule>(out TModule module)
            where TModule : IModule;
    }
}
