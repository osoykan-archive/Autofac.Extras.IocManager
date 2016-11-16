using System;

using Autofac.Extras.IocManager.Extensions;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocManagerTests : TestBase
    {
        [Fact]
        public void IocManagerShouldWork()
        {
            Builder.Build().UseIocManager();

            IocManager.Instance.ShouldNotBeNull();
            IocManager.Instance.Container.ShouldNotBeNull();
        }

        [Fact]
        public void IocManagerShould_Resolvedependency()
        {
            Builder.RegisterType<SimpleDependency>().As<ISimpleDependency>().InstancePerDependency();
            Builder.Build().UseIocManager();

            var simpleDependency = IocManager.Instance.Resolve<ISimpleDependency>();
            simpleDependency.ShouldNotBeNull();
        }

        [Fact]
        public void IocManager_ShouldResolveDisposableDependency_And_Dispose_After_Scope_Finished()
        {
            Builder.RegisterType<SimpleDisposableDependency>().InstancePerDependency();
            Builder.Build().UseIocManager();

            SimpleDisposableDependency simpleDisposableDependency;
            using (IDisposableDependencyObjectWrapper<SimpleDisposableDependency> simpleDependencyWrapper = IocManager.Instance.ResolveAsDisposable<SimpleDisposableDependency>())
            {
                simpleDisposableDependency = simpleDependencyWrapper.Object;
            }

            simpleDisposableDependency.DisposeCount.ShouldBe(1);
        }

        [Fact]
        public void IocManager_ShouldInjectAnyDependecy()
        {
            Builder.RegisterType<SimpleDependencyWithIocManager>().InstancePerDependency();
            IContainer container = Builder.Build().UseIocManager();

            var dependencyWithIocManager = container.Resolve<SimpleDependencyWithIocManager>();

            dependencyWithIocManager.GetIocManager().ShouldBe(IocManager.Instance);
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
