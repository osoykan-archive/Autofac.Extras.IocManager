using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class RegistrationThroughContainerBuilder_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ContainerBuilderTestsShouldWork()
        {
            Building(builder => { builder.RegisterServices(r => r.UseBuilder(containerBuilder => containerBuilder.RegisterType<MyClass>())); });

            The<MyClass>().Should().NotBeNull();
        }

        [Fact]
        public void RegisterBuildCallback_should_work()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.RegisterType<MyClass>(Lifetime.Singleton);
                    r.UseBuilder(cb => cb.RegisterBuildCallback(container => { container.Resolve<MyClass>(); }));
                });
            });

            The<MyClass>().ResolveCount.Should().Be(1);
        }
    }

    internal class MyClass
    {
        public MyClass()
        {
            ResolveCount++;
        }

        public int ResolveCount { get; set; }
    }
}
