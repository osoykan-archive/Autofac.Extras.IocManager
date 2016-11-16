using System;

using Autofac.Extras.IocManager.Extensions;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocManagerTests
    {
        [Fact]
        public void IocManagerShouldWork()
        {
            var localIocManager = new IocManager();
            ContainerBuilder builder = new ContainerBuilder().RegisterIocManager(localIocManager);
            builder.Build().UseIocManager(localIocManager);

            localIocManager.ShouldNotBeNull();
            localIocManager.Container.ShouldNotBeNull();
        }

        [Fact]
        public void IocManagerShould_Resolvedependency()
        {
            var localIocManager = new IocManager();
            ContainerBuilder builder = new ContainerBuilder().RegisterIocManager(localIocManager);
            builder.RegisterType<SimpleDependency>().As<ISimpleDependency>().InstancePerLifetimeScope();
            IContainer container = builder.Build().UseIocManager(localIocManager);

            var simpleDependency = localIocManager.Resolve<ISimpleDependency>();
            simpleDependency.ShouldNotBeNull();
        }

        [Fact]
        public void IocManager_ShouldResolveDisposableDependency_And_Dispose_After_Scope_Finished()
        {
            var localIocManager = new IocManager();
            ContainerBuilder builder = new ContainerBuilder().RegisterIocManager(localIocManager);
            builder.RegisterType<SimpleDisposableDependency>().InstancePerLifetimeScope();
            IContainer container = builder.Build().UseIocManager(localIocManager);

            SimpleDisposableDependency simpleDisposableDependency;
            using (IDisposableDependencyObjectWrapper<SimpleDisposableDependency> simpleDependencyWrapper = localIocManager.ResolveAsDisposable<SimpleDisposableDependency>())
            {
                simpleDisposableDependency = simpleDependencyWrapper.Object;
            }

            simpleDisposableDependency.DisposeCount.ShouldBe(1);
        }

        [Fact]
        public void IocManager_ShouldInjectAnyDependecy()
        {
            var localIocManager = new IocManager();
            ContainerBuilder builder = new ContainerBuilder().RegisterIocManager(localIocManager);
            builder.RegisterType<SimpleDependencyWithIocManager>().InstancePerLifetimeScope();
            IContainer container = builder.Build().UseIocManager(localIocManager);

            var dependencyWithIocManager = container.Resolve<SimpleDependencyWithIocManager>();

            dependencyWithIocManager.GetIocManager().ShouldBe(localIocManager);
        }

        internal interface ISimpleDependency {}

        internal class SimpleDependency : ISimpleDependency {}

        internal class SimpleDisposableDependency : IDisposable
        {
            public int DisposeCount { get; set; }

            public void Dispose()
            {
                DisposeCount++;
            }
        }

        internal class SimpleDependencyWithIocManager
        {
            private readonly IIocManager _iocManager;

            public SimpleDependencyWithIocManager(IIocManager iocManager)
            {
                _iocManager = iocManager;
            }

            public IIocManager GetIocManager()
            {
                return _iocManager;
            }
        }
    }
}
