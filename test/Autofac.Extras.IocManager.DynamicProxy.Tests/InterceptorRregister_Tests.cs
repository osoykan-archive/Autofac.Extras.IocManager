using System;
using System.Collections.Generic;
using System.Linq;
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

            var orderService = LocalIocManager.Resolve<IOrderAppService>();
            orderService.DoSomeCoolStuff();
            orderService.ShouldNotBeNull();
            orderService.GetType().Name.ShouldContain("Proxy");
        }

        [Fact]
        public void interceptor_registration_with_registercallback_should_work()
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

            var orderService = LocalIocManager.Resolve<IOrderAppService>();
            orderService.DoSomeCoolStuff();
            orderService.ShouldNotBeNull();
            orderService.GetType().Name.ShouldContain("Proxy");
        }

        private static void UnitOfWorkRegistrar(ComponentRegisteredEventArgs args)
        {
            Type implType = args.ComponentRegistration.Activator.LimitType;

            if (typeof(IApplicationService).IsAssignableFrom(implType))
            {
                args.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>();
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
            }
        }

        public interface IOrderAppService
        {
            void DoSomeCoolStuff();
        }

        public class OrderAppService : IOrderAppService, IApplicationService
        {
            public void DoSomeCoolStuff()
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

        public class UnitOfWorkInterceptorRegistrarModule : Module
        {
            // This is a private constant from the Autofac.Extras.DynamicProxy2 assembly
            // that is needed to "poke" interceptors into registrations.
            private const string InterceptorsPropertyName = "Autofac.Extras.DynamicProxy2.RegistrationExtensions.InterceptorsPropertyName";

            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<UnitOfWorkInterceptor>().As<IInterceptor>().AsSelf();
            }

            protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
            {
                // Here is where you define your "global interceptor list"
                var interceptorServices = new Service[] { new TypedService(typeof(UnitOfWorkInterceptor)) };

                Type impl = registration.Activator.LimitType;

                //Type[] services = registration.Services.OfType<TypedService>().Select(x => x.ServiceType).ToArray();

                // Append the global interceptors to any existing list, or create a new interceptor
                // list if none are specified. Note this info will only be used by registrations
                // that are set to have interceptors enabled. It'll be ignored by others.

                if (typeof(IApplicationService).IsAssignableFrom(impl))
                {
                    object existing;
                    if (registration.Metadata.TryGetValue(InterceptorsPropertyName, out existing))
                    {
                        registration.Metadata[InterceptorsPropertyName] =
                            ((IEnumerable<Service>)existing).Concat(interceptorServices).Distinct();
                    }
                    else
                    {
                        registration.Metadata.Add(InterceptorsPropertyName, interceptorServices);
                    }
                }
            }
        }
    }
}
