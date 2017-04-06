using System;
using System.Reflection;

using Autofac.Core;
using Autofac.Extras.IocManager.TestBase;

using Castle.DynamicProxy;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.DynamicProxy.Tests
{
    public class InterceptorRregister_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void interceptor_registration_with_registercallback_should_work_with_conventional_registration()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterCallback(registry => registry.Registered += (sender, args) => { UnitOfWorkRegistrar(args); })));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });

            var orderService = The<IOrderAppService>();
            orderService.DoSomeCoolStuff();
            orderService.ShouldNotBeNull();
            orderService.GetType().Name.ShouldContain("Proxy");
        }

        [Fact]
        public void interceptor_registration_with_registercallback_should_work_with_property_injection()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterCallback(registry => registry.Registered += (sender, args) => { UnitOfWorkRegistrar(args); })));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });

            var orderService = The<IOrderAppService>();
            orderService.ProductAppService.ShouldNotBeNull();
            orderService.DoSomeCoolStuff();
        }

        [Fact]
        public void interceptor_registration_should_work()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterType<OrderAppService>()
                                                                   .As<IOrderAppService>()
                                                                   .AsSelf()
                                                                   .EnableInterception<IOrderAppService, UnitOfWorkInterceptor>()));

                builder.RegisterServices(r => r.Register<IInterceptor, UnitOfWorkInterceptor>());
                builder.RegisterServices(r => r.Register<ILogger, Logger>());
            });

            var orderService = The<IOrderAppService>();
            orderService.DoSomeCoolStuff();
            orderService.ShouldNotBeNull();
            orderService.GetType().Name.ShouldContain("Proxy");
        }

        [Fact]
        public void multiple_interceptor_should_be_able_to_intercept_any_dependency()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterCallback(registry => registry.Registered += (sender, args) => { MultipleInterceptorRegistrar(args); })));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });

            var orderService = The<IOrderAppService>();
            orderService.ProductAppService.ShouldNotBeNull();
            orderService.DoSomeCoolStuff();
        }

        private static void UnitOfWorkRegistrar(ComponentRegisteredEventArgs args)
        {
            Type implType = args.ComponentRegistration.Activator.LimitType;

            if (typeof(IApplicationService).IsAssignableFrom(implType))
            {
                args.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>();
            }
        }

        private static void MultipleInterceptorRegistrar(ComponentRegisteredEventArgs args)
        {
            Type implType = args.ComponentRegistration.Activator.LimitType;

            if (typeof(IApplicationService).IsAssignableFrom(implType))
            {
                args.ComponentRegistration.InterceptedBy(typeof(UnitOfWorkInterceptor), typeof(ExceptionInterceptor));
            }
        }

        public class UnitOfWorkInterceptor : IInterceptor, ITransientDependency
        {
            private readonly ILogger _logger;

            public UnitOfWorkInterceptor(ILogger logger)
            {
                _logger = logger;
            }

            public void Intercept(IInvocation invocation)
            {
                _logger.Log(invocation.Method.Name);
                invocation.Proceed();
            }
        }

        public class ExceptionInterceptor : IInterceptor, ITransientDependency
        {
            private readonly ILogger _logger;

            public ExceptionInterceptor(ILogger logger)
            {
                _logger = logger;
            }

            public void Intercept(IInvocation invocation)
            {
                _logger.Log(invocation.Method.Name);
                invocation.Proceed();
            }
        }

        public interface IOrderAppService
        {
            IProductAppService ProductAppService { get; set; }

            void DoSomeCoolStuff();
        }

        public class OrderAppService : IOrderAppService, IApplicationService
        {
            private readonly IProductAppService _productAppService;

            public OrderAppService(IProductAppService productAppService)
            {
                _productAppService = productAppService;
            }

            public IProductAppService ProductAppService { get; set; }

            public virtual void DoSomeCoolStuff()
            {
            }
        }

        public interface IProductAppService
        {
        }

        public class ProductAppService : IProductAppService, IApplicationService
        {
        }

        public interface IApplicationService : ITransientDependency
        {
        }

        public interface ILogger : ITransientDependency
        {
            void Log(string message);
        }

        public class Logger : ILogger
        {
            public void Log(string message)
            {
            }
        }
    }
}
