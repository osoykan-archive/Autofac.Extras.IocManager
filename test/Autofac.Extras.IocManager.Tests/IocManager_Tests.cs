using System;
using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IocManager_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void IocManagerShouldWork()
        {
            Building(builder => { });

            LocalIocManager.ShouldNotBeNull();
            LocalIocManager.Resolver.ShouldNotBeNull();
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
            Building(builder => { builder.RegisterServices(f => f.Register<ISimpleDependency, SimpleDependency>(Lifetime.LifetimeScope)); });

            var simpleDependency = LocalIocManager.Resolve<ISimpleDependency>();
            simpleDependency.ShouldNotBeNull();
        }

        [Fact]
        public void IocManager_ShouldResolveDisposableDependency_And_Dispose_After_Scope_Finished()
        {
            Building(builder => { builder.RegisterServices(f => f.RegisterType<SimpleDisposableDependency>(Lifetime.LifetimeScope)); });

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
            IResolver resolver = Building(builder => { builder.RegisterServices(f => f.RegisterType<SimpleDependencyWithIocManager>(Lifetime.LifetimeScope)); });

            var dependencyWithIocManager = resolver.Resolve<SimpleDependencyWithIocManager>();

            dependencyWithIocManager.GetIocManager().ShouldBeSameAs(LocalIocManager);
            dependencyWithIocManager.GetIocResolver().ShouldBeSameAs(LocalIocManager);
        }

        [Fact]
        public void IocManager_ScopeShouldWork()
        {
            Building(builder => { builder.RegisterServices(f => f.RegisterType<SimpleDisposableDependency>(Lifetime.LifetimeScope)); });

            SimpleDisposableDependency simpleDisposableDependency;
            using (IIocScopedResolver iocScopedResolver = LocalIocManager.CreateScope())
            {
                simpleDisposableDependency = iocScopedResolver.Resolve<SimpleDisposableDependency>();
            }

            simpleDisposableDependency.DisposeCount.ShouldBe(1);
        }

        [Fact]
        public void DefaultInterfaces_registration_should_work()
        {
            Building(builder => builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())));

            LocalIocManager.Resolve<ICurrentUnitOfWorkProvider>().ShouldNotBeNull();
        }

        internal class CallContextCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider,ITransientDependency
        {
        }

        internal interface ICurrentUnitOfWorkProvider
        {
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
            private readonly IIocResolver _iocResolver;

            public SimpleDependencyWithIocManager(IIocManager iocManager, IIocResolver iocResolver)
            {
                _iocManager = iocManager;
                _iocResolver = iocResolver;
            }

            public IIocManager GetIocManager()
            {
                return _iocManager;
            }

            public IIocResolver GetIocResolver()
            {
                return _iocResolver;
            }
        }
    }
}
