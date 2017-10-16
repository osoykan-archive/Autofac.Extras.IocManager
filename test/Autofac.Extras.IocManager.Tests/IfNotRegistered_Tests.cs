using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

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

            LocalIocManager.IsRegistered<ServiceB>().Should().Be(false);
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
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceB)).Should().Be(false);
            The<IService>().Should().BeAssignableTo<ServiceA>();
        }

        [Fact]
        public void RegisterIfAbsent_should_work_with_type_parameters_()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.Register<IService, ServiceA>();
                    r.RegisterIfAbsent(typeof(IService), typeof(ServiceB));
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceB)).Should().Be(false);
            The<IService>().Should().BeAssignableTo<ServiceA>();
        }

        [Fact]
        public void RegisterIfAbsent_should_work_with_type_parameters_propertyinjection_should_not_be_null()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.RegisterIfAbsent(typeof(MyClass));
                    r.RegisterIfAbsent(typeof(IService), typeof(ServiceB));
                    r.RegisterIfAbsent<MyClass>();
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceB)).Should().Be(true);
            The<IService>().Should().BeAssignableTo<ServiceB>();
            The<IService>().MyProperty.Should().NotBeNull();
        }

        [Fact]
        public void RegisterIfAbsent_should_work_with_TGeneric_propertyinjection_should_not_be_null()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.RegisterType(typeof(MyClass));
                    r.RegisterIfAbsent<IService, ServiceB>();
                    r.RegisterIfAbsent<MyClass>();
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceB)).Should().Be(true);
            The<IService>().Should().BeAssignableTo<ServiceB>();
            The<IService>().MyProperty.Should().NotBeNull();
        }

        [Fact]
        public void RegisterIfAbsent_should_work_on_propertyInjection()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.RegisterIfAbsent<IService, ServiceA>();
                    r.RegisterIfAbsent(typeof(IService), typeof(ServiceB));
                    r.RegisterType(typeof(MyClass));
                });
            });

            LocalIocManager.IsRegistered(typeof(ServiceB)).Should().Be(false);
            The<IService>().Should().BeAssignableTo<ServiceA>();

            var instanceA = The<IService>();
            instanceA.MyProperty.Should().NotBeNull();
        }

        [Fact]
        public void IfNotRegistered_PropertyInjection_should_work()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MyClass>().AsSelf().IfNotRegistered(typeof(MyClass));
            builder.RegisterType<ServiceA>().As<IService>().IfNotRegistered(typeof(ServiceA)).AsSelf().PropertiesAutowired();
            IContainer resolver = builder.Build();

            var serviceAInstance = resolver.Resolve<ServiceA>();
            serviceAInstance.MyProperty.Should().NotBeNull();
        }

        [Fact]
        public void IfNotRegistered_PropertyInjection_should_work_single()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MyClass>().AsSelf().IfNotRegistered(typeof(MyClass));
            builder.RegisterType<ServiceA>().AsSelf().IfNotRegistered(typeof(ServiceA)).PropertiesAutowired();
            IContainer resolver = builder.Build();

            var serviceAInstance = resolver.Resolve<ServiceA>();
            serviceAInstance.MyProperty.Should().NotBeNull();
        }

        internal class ServiceA : IService
        {
            public MyClass MyProperty { get; set; }
        }

        internal interface IService
        {
            MyClass MyProperty { get; set; }
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
