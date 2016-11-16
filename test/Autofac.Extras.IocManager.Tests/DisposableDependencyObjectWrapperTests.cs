using System;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class DisposableDependencyObjectWrapperTests : TestBase
    {
        [Fact]
        public void ResolveAsDisposable_With_Constructor_Args_Should_Work()
        {
            Building(builder =>
                     {
                         builder.RegisterType<SimpleDisposableObject>().InstancePerDependency();
                     });

            using (var wrapper = LocalIocManager.ResolveAsDisposable<SimpleDisposableObject>(new { myData = 42 }))
            {
                wrapper.Object.MyData.ShouldBe(42);
            }
        }

        [Fact]
        public void Using_Test()
        {
            Building(builder =>
                     {
                         builder.RegisterType<SimpleDisposableObject>().InstancePerDependency();
                     });

            LocalIocManager.ResolveUsing<SimpleDisposableObject>(obj => obj.MyData.ShouldBe(0));
        }

        internal class SimpleDisposableObject : IDisposable
        {
            public SimpleDisposableObject()
            {
            }

            public SimpleDisposableObject(int myData)
            {
                MyData = myData;
            }

            public int MyData { get; set; }

            public void Dispose()
            {
            }
        }
    }
}
