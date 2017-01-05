using System;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class DisposableDependencyObjectWrapper_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ResolveAsDisposable_With_Constructor_Args_Should_Work()
        {
            Building(builder =>
                     {
                         builder.RegisterServices(f => f.RegisterType<SimpleDisposableObject>());
                     });

            using (IDisposableDependencyObjectWrapper<SimpleDisposableObject> wrapper = LocalIocManager.ResolveAsDisposable<SimpleDisposableObject>(new { myData = 42 }))
            {
                wrapper.Object.MyData.ShouldBe(42);
            }
        }

        [Fact]
        public void Using_Test()
        {
            Building(builder =>
                     {
                         builder.RegisterServices(f => f.RegisterType<SimpleDisposableObject>());
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
