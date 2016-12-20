using Autofac.Extras.IocManager.Tests.FluentTests.FakeEventStore;

namespace Autofac.Extras.IocManager.Tests.FluentTests
{
    public static class FakeEvenStoreBuilderExtensions
    {
        public static IIocBuilder UseEventStore(this IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.Register<IEventStore, EventStore>());
            iocBuilder.RegisterModule<FakeEventStoreModule>();
            return iocBuilder;
        }
    }
}
