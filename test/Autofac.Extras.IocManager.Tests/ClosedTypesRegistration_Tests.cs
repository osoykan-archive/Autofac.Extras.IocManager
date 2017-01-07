using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ClosedTypesRegistration_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ClosedTypesRegistration_ShouldWork()
        {
            IResolver resolver = Building(builder =>
            {
                builder.RegisterServices(r => r.RegisterAssemblyAsClosedTypesOf(Assembly.GetExecutingAssembly(), typeof(IMyGenericInterface<>)));
            });

            resolver.IsRegistered<MyBaseClass>().ShouldBe(true);
            resolver.IsRegistered<MyGeneric<SomeClass>>().ShouldBe(true);
            resolver.IsRegistered<IMyGenericInterface<SomeClass>>().ShouldBe(true);
        }

        internal class SomeClass
        {
        }

        internal class MyBaseClass : MyGeneric<SomeClass>
        {
        }

        internal class MyGeneric<T> : IMyGenericInterface<T>
        {
        }

        internal interface IMyGenericInterface<T>
        {
        }
    }
}
