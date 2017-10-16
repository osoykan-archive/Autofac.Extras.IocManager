using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class Startable_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void StartableShouldWork()
        {
            Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))));

            var startableDependency = The<IStartableDependency>();
            startableDependency.StartCallExecuted.Should().Be(true);
        }

        [Fact]
        public void Startable_ShouldInject_Any_Dependency()
        {
            Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))));

            var startableDependency = The<IStartableDependency2>();

            startableDependency.StartCallExecuted.Should().Be(true);
            startableDependency.GetIocManager().Should().NotBeNull();
            startableDependency.GetIocManager().Should().BeSameAs(LocalIocManager);
            startableDependency.IocResolver.Should().NotBeNull();
            startableDependency.IocResolver.Should().BeSameAs(LocalIocManager);
        }

        [Fact]
        public void ConcreteStartable_should_instantiatable()
        {
            Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))));

            var concreteStartable = The<ConcreteStartable>();

            concreteStartable.StartCallExecuted.Should().Be(true);
        }

        internal class ConcreteStartable : MyConcreteStartable, ISingletonDependency, IStartable
        {
            public bool StartCallExecuted { get; private set; }

            public void Start()
            {
                StartCallExecuted = true;
            }
        }

        internal class MyConcreteStartable
        {
        }

        internal class StartableDependency : IStartableDependency, IStartable, ISingletonDependency
        {
            public void Start()
            {
                StartCallExecuted = true;
            }

            public bool StartCallExecuted { get; private set; }
        }

        internal interface IStartableDependency
        {
            bool StartCallExecuted { get; }
        }

        internal class StartableDependency2 : IStartableDependency2, IStartable, ISingletonDependency
        {
            private readonly IIocManager _iocManager;

            public StartableDependency2(IIocManager iocManager)
            {
                _iocManager = iocManager;
            }

            public void Start()
            {
                StartCallExecuted = true;
            }

            public IIocResolver IocResolver { get; set; }

            public bool StartCallExecuted { get; private set; }

            public IIocManager GetIocManager()
            {
                return _iocManager;
            }
        }

        internal interface IStartableDependency2
        {
            bool StartCallExecuted { get; }

            IIocResolver IocResolver { get; set; }

            IIocManager GetIocManager();
        }
    }
}
