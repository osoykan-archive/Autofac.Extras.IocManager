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
                       .UseEventStore()
                       .UseRabbitMQ()
                       .RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            });

            resolver.IsRegistered<IEventStore>().ShouldBe(true);
            resolver.IsRegistered<ILogger>().ShouldBe(true);
            resolver.IsRegistered<IBus>().ShouldBe(true);

            LocalIocManager.IsRegistered<IEventStore>().ShouldBe(true);
            LocalIocManager.IsRegistered<ILogger>().ShouldBe(true);
            LocalIocManager.IsRegistered<IBus>().ShouldBe(true);
        }

        [Fact]
        public void Injectable_IocResolvers_ShouldWork()
        {
            IResolver resolver = Building(builder => { builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()); });

            var injectable = resolver.Resolve<InjectableIocResolver>();
            injectable.ShouldNotBeNull();
            injectable.GetResolver().ShouldBeAssignableTo<IResolver>();
            injectable.GetScopeResolver().ShouldBeAssignableTo<IScopeResolver>();
        }

        [Fact]
        public void Module_Registration_Should_Work()
        {
            IResolver resolver = Building(builder => { builder.RegisterModule<FakeEventStoreModule>(); });

            bool moduleBasedEventStore = resolver.IsRegistered<IModuleBasedEventStore>();

            moduleBasedEventStore.ShouldNotBeNull();
        }

        [Fact]
        public void Module_Registration_Should_Work_WithExtension()
        {
            IResolver resolver = Building(builder => { builder.UseEventStore(); });

            bool moduleBasedEventStore = resolver.IsRegistered<IModuleBasedEventStore>();

            moduleBasedEventStore.ShouldNotBeNull();
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

            public IResolver GetResolver()
            {
                return _resolver;
            }

            public IScopeResolver GetScopeResolver()
            {
                return _scopeResolver;
            }
        }
    }
}
