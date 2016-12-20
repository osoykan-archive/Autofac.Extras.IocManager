using Autofac.Extras.IocManager.Tests.FluentTests.FakeEventStore;

namespace Autofac.Extras.IocManager.Tests.FluentTests
{
    public static class FakeEvenStoreBuilderExtensions
    {
        public static IIocBuilder UserEventStore(this IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.Register<IEventStore, EventStore>());

            return iocBuilder;
        }
    }
}
