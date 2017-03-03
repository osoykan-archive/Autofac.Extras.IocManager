using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocBuilder_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void IocBuilder_ShouldWork()
        {
            IResolver resolver = Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())));

            var myDependencyFromLocalIocManager = LocalIocManager.Resolve<IMyDependency>();
            var myDependencyFromResolver = resolver.Resolve<IMyDependency>();

            myDependencyFromLocalIocManager.ShouldBeSameAs(myDependencyFromResolver);
        }

        [Fact]
        public void ResolverContext_should_not_be_null()
        {
            IResolver resolver = Building(builder => { });

            var resolverContext = new ResolverContext(resolver);
            resolverContext.Resolver.ShouldNotBeNull();
        }

        internal interface IMyDependency
        {
        }

        internal class MyDependency : IMyDependency, ISingletonDependency
        {
        }
    }
}
