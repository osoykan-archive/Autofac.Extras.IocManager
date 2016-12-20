using System.Reflection;

using Autofac.Extras.IocManager.Tests.FluentTests.FakeEventStore;
using Autofac.Extras.IocManager.Tests.FluentTests.FakeRabbitMQ;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests.FluentTests
{
    public class FluentBootstrapperTests : TestBaseWithIocBuilder
    {
        [Fact]
        public void FluentBootstrapper_Should_Work()
        {
            IResolver resolver = Building(builder =>
            {
                builder.UseNLog()
                       .UserEventStore()
                       .UseRabbitMQ()
                       .RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            });

            resolver.HasRegistrationFor<IEventStore>().ShouldBe(true);
            resolver.HasRegistrationFor<ILogger>().ShouldBe(true);
            resolver.HasRegistrationFor<IBus>().ShouldBe(true);

            LocalIocManager.IsRegistered<IEventStore>().ShouldBe(true);
            LocalIocManager.IsRegistered<ILogger>().ShouldBe(true);
            LocalIocManager.IsRegistered<IBus>().ShouldBe(true);
        }

        [Fact]
        public void Injectable_IocResolvers_ShouldWork()
        {
           

            IResolver resolver = Building(builder =>
            {
                builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            });

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
