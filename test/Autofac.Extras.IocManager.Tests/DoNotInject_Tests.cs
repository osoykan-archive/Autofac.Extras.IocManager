using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class DoNotInject_Tests : TestBaseWithIocBuilder
    {
        public DoNotInject_Tests()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())); });
        }

        [Fact]
        public void DoNotInject_Should_Work()
        {
            The<IFoo>().Bar.ShouldNotBeNull();

            The<IFoo>().Gong.ShouldBeNull();

            The<FooKinda>().Foo.ShouldNotBeNull();

            The<FooKinda>().GetGong().ShouldNotBeNull();
            The<FooKinda>().GetBar().ShouldBeNull();
        }

        public class FooKinda : FooBase
        {
            public IGong GetGong()
            {
                return Gong;
            }

            public IBar GetBar()
            {
                return Bar;
            }
        }

        public abstract class FooBase : ITransientDependency
        {
            public IFoo Foo { get; set; }

            protected IGong Gong { get; set; }

            [DoNotInject]
            protected IBar Bar { get; set; }
        }

        public interface IFoo
        {
            IBar Bar { get; }

            IGong Gong { get; set; }
        }

        public class Foo : IFoo, ITransientDependency
        {
            public IBar Bar { get; set; }

            [DoNotInject]
            public IGong Gong { get; set; }
        }

        public interface IBar
        {
        }

        public class Bar : IBar, ITransientDependency
        {
        }

        public interface IGong
        {
        }

        public class Gong : IGong, ITransientDependency
        {
        }
    }
}
