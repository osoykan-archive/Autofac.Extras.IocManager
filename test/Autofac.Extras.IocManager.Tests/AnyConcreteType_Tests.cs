using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class AnyConcreteType_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void any_concrete_type_should_be_registered()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.UseBuilder(cb =>
                {
                    cb.RegisterType<SomeKindOf>().As<ISomeKindOf>();
                }));
            });


            LocalIocManager.Resolve<ISomeKindOf>().ShouldNotBeNull();
            LocalIocManager.Resolve<SomeKindOf>().ShouldNotBeNull();
        }

        public interface ISomeKindOf
        {
        }

        public class SomeKindOf : ISomeKindOf
        {
        }
    }
}
