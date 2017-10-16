using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class CircularPropertyDependency_Tests : TestBaseWithIocBuilder
    {
        private void Initialize_Test_LifeTimeScope()
        {
            MyClass1.CreateCount = 0;
            MyClass2.CreateCount = 0;
            MyClass3.CreateCount = 0;

            Building(builder =>
            {
                builder.RegisterServices(f => f.RegisterType<MyClass1>(Lifetime.LifetimeScope));
                builder.RegisterServices(f => f.RegisterType<MyClass2>(Lifetime.LifetimeScope));
                builder.RegisterServices(f => f.RegisterType<MyClass3>(Lifetime.LifetimeScope));
            });
        }

        [Fact]
        public void Should_Success_Circular_Property_Injection_PerScope()
        {
            Initialize_Test_LifeTimeScope();

            var obj1 = The<MyClass1>();
            obj1.Obj2.Should().NotBeNull();
            obj1.Obj3.Should().NotBeNull();
            obj1.Obj2.Obj3.Should().NotBeNull();

            var obj2 = The<MyClass2>();
            obj2.Obj1.Should().NotBeNull();
            obj2.Obj3.Should().NotBeNull();
            obj2.Obj1.Obj3.Should().NotBeNull();

            MyClass1.CreateCount.Should().Be(1);
            MyClass2.CreateCount.Should().Be(1);
            MyClass3.CreateCount.Should().Be(1);
        }

        public class MyClass1
        {
            public MyClass1()
            {
                CreateCount++;
            }

            public static int CreateCount { get; set; }

            public MyClass2 Obj2 { get; set; }

            public MyClass3 Obj3 { get; set; }
        }

        public class MyClass2
        {
            public MyClass2()
            {
                CreateCount++;
            }

            public static int CreateCount { get; set; }

            public MyClass1 Obj1 { get; set; }

            public MyClass3 Obj3 { get; set; }
        }

        public class MyClass3
        {
            public MyClass3()
            {
                CreateCount++;
            }

            public static int CreateCount { get; set; }
        }
    }
}
