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
            Building(builder => { });

            LocalIocManager.ShouldNotBeNull();
            LocalIocManager.Container.ShouldNotBeNull();
        }

        [Fact]
        public void IocManager_SelfRegistration_ShouldWork()
        {
            Building(builder => { });

            var resolver = LocalIocManager.Resolve<IIocResolver>();
            var managerByInterface = LocalIocManager.Resolve<IIocManager>();
            var managerByClass = LocalIocManager.Resolve<IocManager>();

            managerByClass.ShouldBeSameAs(resolver);
            managerByClass.ShouldBeSameAs(managerByInterface);
        }

        [Fact]
        public void IocManagerShould_Resolvedependency()
        {
            Building(builder =>
                     {
                         builder.RegisterType<SimpleDependency>().As<ISimpleDependency>().InstancePerLifetimeScope();
                     });

            var simpleDependency = LocalIocManager.Resolve<ISimpleDependency>();
            simpleDependency.ShouldNotBeNull();
        }

        [Fact]
        public void IocManager_ShouldResolveDisposableDependency_And_Dispose_After_Scope_Finished()
        {
            Building(builder =>
                     {
                         builder.RegisterType<SimpleDisposableDependency>().InstancePerLifetimeScope();
                     });

            SimpleDisposableDependency simpleDisposableDependency;
            using (var simpleDependencyWrapper = LocalIocManager.ResolveAsDisposable<SimpleDisposableDependency>())
            {
                simpleDisposableDependency = simpleDependencyWrapper.Object;
            }

            simpleDisposableDependency.DisposeCount.ShouldBe(1);
        }

        [Fact]
        public void IocManager_ShouldInjectAnyDependecy()
        {
            var container = Building(builder =>
                                     {
                                         builder.RegisterType<SimpleDependencyWithIocManager>().InstancePerLifetimeScope();
                                     });

            var dependencyWithIocManager = container.Resolve<SimpleDependencyWithIocManager>();

            dependencyWithIocManager.GetIocManager().ShouldBeSameAs(LocalIocManager);
        }

        [Fact]
        public void IocManager_ScopeShouldWork()
        {
            Building(builder =>
                     {
                         builder.RegisterType<SimpleDisposableDependency>().InstancePerLifetimeScope();
                     });

            SimpleDisposableDependency simpleDisposableDependency;
            using (var iocScopedResolver = LocalIocManager.CreateScope())
            {
                simpleDisposableDependency = iocScopedResolver.Resolve<SimpleDisposableDependency>();
            }

            simpleDisposableDependency.DisposeCount.ShouldBe(1);
        }

        internal interface ISimpleDependency
        {
        }

        internal class SimpleDependency : ISimpleDependency
        {
        }

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
