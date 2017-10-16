using System.Reflection;

using Autofac.Extras.IocManager.TestBase;
using Autofac.Extras.IocManager.Tests.FluentTests.FakeEventStore;
using Autofac.Extras.IocManager.Tests.FluentTests.FakeNLog;
using Autofac.Extras.IocManager.Tests.FluentTests.FakeRabbitMQ;

using FluentAssertions;

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
                       .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests"))));
            });

            resolver.IsRegistered<IEventStore>().Should().Be(true);
            resolver.IsRegistered<ILogger>().Should().Be(true);
            resolver.IsRegistered<IBus>().Should().Be(true);

            LocalIocManager.IsRegistered<IEventStore>().Should().Be(true);
            LocalIocManager.IsRegistered<ILogger>().Should().Be(true);
            LocalIocManager.IsRegistered<IBus>().Should().Be(true);
        }

        [Fact]
        public void Injectable_IocResolvers_ShouldWork()
        {
            IResolver resolver = Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))); });

            var injectable = resolver.Resolve<InjectableIocResolver>();
            injectable.Should().NotBeNull();
            injectable.GetResolver().Should().BeAssignableTo<IResolver>();
            injectable.GetScopeResolver().Should().BeAssignableTo<IScopeResolver>();
        }

        [Fact]
        public void Module_Registration_Should_Work()
        {
            IResolver resolver = Building(builder => { builder.RegisterModule<FakeEventStoreModule>(); });

            bool moduleBasedEventStore = resolver.IsRegistered<IModuleBasedEventStore>();

            moduleBasedEventStore.Should();
        }

        [Fact]
        public void Module_Registration_Should_Work_WithExtension()
        {
            IResolver resolver = Building(builder => { builder.UseEventStore(); });

            bool moduleBasedEventStore = resolver.IsRegistered<IModuleBasedEventStore>();

            moduleBasedEventStore.Should().Be(true);
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
