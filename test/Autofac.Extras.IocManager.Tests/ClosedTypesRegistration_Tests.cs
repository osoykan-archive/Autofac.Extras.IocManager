using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ClosedTypesRegistration_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ClosedTypesRegistration_ShouldWork()
        {
            IResolver resolver = Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyAsClosedTypesOf(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")), typeof(IMyGenericInterface<>))); });

            resolver.IsRegistered<MyBaseClass>().Should().Be(true);
            resolver.IsRegistered<MyGeneric<SomeClass>>().Should().Be(true);
            resolver.IsRegistered<IMyGenericInterface<SomeClass>>().Should().Be(true);
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
