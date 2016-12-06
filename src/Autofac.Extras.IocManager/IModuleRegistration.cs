namespace Autofac.Extras.IocManager
{
    public interface IModuleRegistration
    {
        void Register<TModule>()
            where TModule : IModule, new();

        void Register<TModule>(TModule module)
            where TModule : IModule;

        TModule GetModule<TModule>()
            where TModule : IModule;

        bool TryGetModule<TModule>(out TModule module)
            where TModule : IModule;
    }
}