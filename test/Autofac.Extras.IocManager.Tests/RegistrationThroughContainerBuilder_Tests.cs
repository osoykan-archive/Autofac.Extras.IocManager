using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class RegistrationThroughContainerBuilder_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ContainerBuilderTestsShouldWork()
        {
            Building(builder => { builder.RegisterServices(r => r.UseBuilder(containerBuilder => containerBuilder.RegisterType<MyClass>())); });

            LocalIocManager.Resolve<MyClass>().ShouldNotBeNull();
        }

        internal class MyClass
        {
        }
    }
}
