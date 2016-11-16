using System;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocManagerTests : TestBase
    {
        [Fact]
        public void IocManagerShouldWork()
        {
            Builder.Build().UseIocManager(LocalIocManager);

            LocalIocManager.ShouldNotBeNull();
            LocalIocManager.Container.ShouldNotBeNull();
        }

        [Fact]
        public void IocManagerShould_Resolvedependency()
        {
            Builder.RegisterType<SimpleDependency>().As<ISimpleDependency>().InstancePerLifetimeScope();
            Builder.Build().UseIocManager(LocalIocManager);

            var simpleDependency = LocalIocManager.Resolve<ISimpleDependency>();
            simpleDependency.ShouldNotBeNull();
        }

        [Fact]
        public void IocManager_ShouldResolveDisposableDependency_And_Dispose_After_Scope_Finished()
        {
            Builder.RegisterType<SimpleDisposableDependency>().InstancePerLifetimeScope();
            Builder.Build().UseIocManager(LocalIocManager);

            SimpleDisposableDependency simpleDisposableDependency;
            using (IDisposableDependencyObjectWrapper<SimpleDisposableDependency> simpleDependencyWrapper = LocalIocManager.ResolveAsDisposable<SimpleDisposableDependency>())
            {
                simpleDisposableDependency = simpleDependencyWrapper.Object;
            }

            simpleDisposableDependency.DisposeCount.ShouldBe(1);
        }

        [Fact]
        public void IocManager_ShouldInjectAnyDependecy()
        {
            Builder.RegisterType<SimpleDependencyWithIocManager>().InstancePerLifetimeScope();
            IContainer container = Builder.Build().UseIocManager(LocalIocManager);

            var dependencyWithIocManager = container.Resolve<SimpleDependencyWithIocManager>();

            dependencyWithIocManager.GetIocManager().ShouldBe(LocalIocManager);
        }

        [Fact]
        public void IocManager_ScopeShouldWork()
        {
            Builder.RegisterType<SimpleDisposableDependency>().InstancePerLifetimeScope();
            Builder.Build().UseIocManager(LocalIocManager);

            SimpleDisposableDependency simpleDisposableDependency;
            using (IIocScopedResolver iocScopedResolver = LocalIocManager.CreateScope())
            {
                simpleDisposableDependency = iocScopedResolver.Resolve<SimpleDisposableDependency>();
            }

            simpleDisposableDependency.DisposeCount.ShouldBe(1);
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
