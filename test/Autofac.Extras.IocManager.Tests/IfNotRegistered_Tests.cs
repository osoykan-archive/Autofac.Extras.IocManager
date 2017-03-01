using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class IfNotRegistered_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void if_not_registered_should_work()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.UseBuilder(cb =>
                {
                    cb.RegisterType<ServiceA>().As<IService>();
                    cb.RegisterType<ServiceB>().As<IService>().IfNotRegistered(typeof(ServiceA));
                }));
            });

            LocalIocManager.IsRegistered<ServiceB>().ShouldBe(false);
        }

        [Fact]
        public void RegisterIfAbsent_should_work_with_generic_T()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.Register<IService, ServiceA>();
                    r.RegisterIfAbsent<IService, ServiceB>();
                    r.RegisterIfAbsent<MyClass>();
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceB)).ShouldBe(false);
            LocalIocManager.Resolve<IService>().ShouldBeAssignableTo<ServiceA>();
        }

        [Fact]
        public void RegisterIfAbsent_should_work_with_type_parameters()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.Register<IService, ServiceA>();
                    r.RegisterIfAbsent(typeof(IService), typeof(ServiceB));
                    r.RegisterIfAbsent<MyClass>();
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceB)).ShouldBe(false);
            LocalIocManager.Resolve<IService>().ShouldBeAssignableTo<ServiceA>();
        }

        [Fact]
        public void RegisterIfAbsent_should_work_with_type()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.RegisterIfAbsent(typeof(ServiceA));
                    r.Register<IService, ServiceA>();
                    r.RegisterIfAbsent<MyClass>();
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceA)).ShouldBe(true);
            LocalIocManager.IsRegistered(typeof(IService)).ShouldBe(true);
            LocalIocManager.Resolve<IService>().ShouldBeAssignableTo<ServiceA>();
        }

        [Fact]
        public void RegisterIfAbsent_should_work_with_only_concrete_type_registration()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.RegisterIfAbsent(typeof(ServiceA));
                    r.RegisterIfAbsent<ServiceB>();
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceA)).ShouldBe(true);
            LocalIocManager.IsRegistered(typeof(ServiceB)).ShouldBe(true);

            LocalIocManager.Resolve<ServiceA>().ShouldBeAssignableTo<ServiceA>();
            LocalIocManager.Resolve<ServiceB>().ShouldBeAssignableTo<ServiceB>();
        }

        internal class ServiceA : IService
        {
            public MyClass MyProperty { get; set; }
        }

        internal interface IService
        {
        }

        internal class ServiceB : IService
        {
            public MyClass MyProperty { get; set; }
        }

        internal class MyClass
        {
        }
    }
}
