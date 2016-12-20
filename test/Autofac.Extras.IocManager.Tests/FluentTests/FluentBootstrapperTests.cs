using System.Reflection;

using Autofac.Extras.IocManager.Tests.FluentTests.FakeEventStore;
using Autofac.Extras.IocManager.Tests.FluentTests.FakeRabbitMQ;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests.FluentTests
{
    public class FluentBootstrapperTests
    {
        [Fact]
        public void FluentBootstrapper_Should_Work()
        {
            var iocManager = new IocManager();

            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseNLog()
                                               .UserEventStore()
                                               .UseRabbitMQ()
                                               .RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())
                                               .RegisterIocManager(iocManager)
                                               .CreateResolver()
                                               .UseIocManager(iocManager);

            resolver.HasRegistrationFor<IEventStore>().ShouldBe(true);
            resolver.HasRegistrationFor<ILogger>().ShouldBe(true);
            resolver.HasRegistrationFor<IBus>().ShouldBe(true);

            iocManager.IsRegistered<IEventStore>().ShouldBe(true);
            iocManager.IsRegistered<ILogger>().ShouldBe(true);
            iocManager.IsRegistered<IBus>().ShouldBe(true);

            var injectable = resolver.Resolve<InjectableIocResolver>();
            injectable.ShouldNotBeNull();
        }

        internal class InjectableIocResolver : ITransientDependency
        {
            private readonly IResolver _resolver;
            private readonly IScopeResolver _scopeResolver;

            public InjectableIocResolver(IResolver resolver, IScopeResolver scopeResolver)
            {
                _resolver = resolver;
                _scopeResolver = scopeResolver;
            }
        }
    }
}
