using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ClosedTypesRegistration : TestBaseWithIocBuilder
    {
        [Fact]
        public void ClosedTypesRegistration_ShouldWork()
        {
            Building(builder =>
            {
              
            });
        }

        internal class SomeClass {}

        internal class MyBaseClass : MyGeneric<SomeClass> {}

        internal class MyGeneric<T> : IMyGenericInterface<T> {}

        internal interface IMyGenericInterface<T> {}
    }
}
