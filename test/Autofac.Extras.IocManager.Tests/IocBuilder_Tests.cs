using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocBuilder_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void IocBuilder_ShouldWork()
        {
            IResolver resolver = Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))));

            var myDependencyFromLocalIocManager = The<IMyDependency>();
            var myDependencyFromResolver = resolver.Resolve<IMyDependency>();

            myDependencyFromLocalIocManager.Should().BeSameAs(myDependencyFromResolver);
        }

        [Fact]
        public void ResolverContext_should_not_be_null()
        {
            IResolver resolver = Building(builder => { });

            var resolverContext = new ResolverContext(resolver);
            resolverContext.Resolver.Should().NotBeNull();
        }

        internal interface IMyDependency
        {
        }

        internal class MyDependency : IMyDependency, ISingletonDependency
        {
        }
    }
}
