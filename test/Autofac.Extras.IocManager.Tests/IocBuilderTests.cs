using System.Reflection;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocBuilderTests
    {
        [Fact]
        public void IocBuilder_ShouldWork()
        {
            using (IRootResolver resolver = IocBuilder.New
                                                      .UseAutofacContainerBuilder()
                                                      .RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())
                                                      .CreateResolver())
            {
                var myDependency = resolver.Resolve<IMyDependency>();
                myDependency.ShouldNotBeNull();
            }
        }

        internal interface IMyDependency
        {
        }

        internal class MyDependency : IMyDependency, ITransientDependency
        {
        }
    }
}
