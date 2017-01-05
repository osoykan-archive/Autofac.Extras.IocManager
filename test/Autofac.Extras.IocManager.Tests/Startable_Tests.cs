using System.Reflection;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class Startable_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void StartableShouldWork()
        {
            Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())));

            var startableDependency = LocalIocManager.Resolve<IStartableDependency>();
            startableDependency.StartCallExecuted.ShouldBe(true);
        }

        [Fact]
        public void Startable_ShouldInject_Any_Dependency()
        {
            Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())));

            var startableDependency = LocalIocManager.Resolve<IStartableDependency2>();

            startableDependency.StartCallExecuted.ShouldBe(true);
            startableDependency.GetIocManager().ShouldNotBeNull();
            startableDependency.GetIocManager().ShouldBeSameAs(LocalIocManager);
            startableDependency.IocResolver.ShouldNotBeNull();
            startableDependency.IocResolver.ShouldBeSameAs(LocalIocManager);
        }

        [Fact]
        public void ConcreteStartable_should_instantiatable()
        {
            Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())));

            var concreteStartable = LocalIocManager.Resolve<ConcreteStartable>();

            concreteStartable.StartCallExecuted.ShouldBe(true);
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
