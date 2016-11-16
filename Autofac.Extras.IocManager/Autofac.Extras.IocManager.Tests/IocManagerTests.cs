using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocManagerTests
    {
        public IocManagerTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterIocManager();
            IContainer container = builder.Build();
            container.UseIocManager();
        }

        [Fact]
        public void IocManagerShouldWork()
        {
            IocManager.Instance.ShouldNotBeNull();
            IocManager.Instance.Container.ShouldNotBeNull();
        }

        [Fact]
        public void IocManagerShould_Resolvedependency()
        {
            var simpleDependency = IocManager.Instance.Resolve<ISimpleDependency>();
            simpleDependency.ShouldNotBeNull();
        }

        internal interface ISimpleDependency {}

        internal class SimpleDependency : ISimpleDependency {}
    }
}
