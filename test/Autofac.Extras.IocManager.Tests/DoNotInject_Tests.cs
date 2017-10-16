using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class DoNotInject_Tests : TestBaseWithIocBuilder
    {
        public DoNotInject_Tests()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))); });
        }

        [Fact]
        public void DoNotInject_Should_Work()
        {
            The<IFoo>().Bar.Should().NotBeNull();

            The<IFoo>().Gong.Should().BeNull();

            The<FooKinda>().Foo.Should().NotBeNull();

            The<FooKinda>().GetGong().Should().NotBeNull();
            The<FooKinda>().GetBar().Should().BeNull();
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
