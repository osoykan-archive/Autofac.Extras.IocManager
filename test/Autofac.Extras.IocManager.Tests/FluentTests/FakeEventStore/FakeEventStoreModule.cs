namespace Autofac.Extras.IocManager.Tests.FluentTests.FakeEventStore
{
    public class FakeEventStoreModule : IModule
    {
        public void Register(IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.Register<IModuleBasedEventStore, ModuleBasedEventStore>());
        }
    }
}
