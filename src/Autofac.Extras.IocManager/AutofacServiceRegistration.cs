using System;
using System.Reflection;

using Autofac.Builder;

namespace Autofac.Extras.IocManager
{
    internal class AutofacServiceRegistration : IServiceRegistration
    {
        private readonly ContainerBuilder _containerBuilder;
        private readonly DecoratorService _decoratorService = new DecoratorService();

        public AutofacServiceRegistration() : this(null)
        {
        }

        public AutofacServiceRegistration(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder ?? new ContainerBuilder();

            _containerBuilder.RegisterType<AutofacResolver>().As<IResolver>();
            _containerBuilder.RegisterType<AutofacScopeResolver>().As<IScopeResolver>();
            _containerBuilder.Register<IDecoratorService>(_ => _decoratorService).SingleInstance();
        }

        public void Register<TService1, TService2, TImplementation>(Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder.RegisterType<TImplementation>().As<TService1, TService2>().InjectPropertiesAsAutowired().AsSelf();

            if (lifetime == Lifetime.Singleton)
            {
                registration.SingleInstance();
            }

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        public void Register<TService, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService
            where TService : class
        {
            IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder.RegisterType<TImplementation>().InjectPropertiesAsAutowired().AsSelf();
            if (lifetime == Lifetime.Singleton)
            {
                registration.SingleInstance();
            }

            IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> serviceRegistration = _containerBuilder
                    .Register<TService>(c => c.Resolve<TImplementation>())
                    .As<TService>()
                    .OnActivating(args =>
                                  {
                                      TService instance = _decoratorService.Decorate(args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                                      args.ReplaceInstance(instance);
                                  });
            if (keepDefault)
            {
                serviceRegistration.PreserveExistingDefaults();
            }
        }

        public void Register<TService>(
            Func<IResolverContext, TService> factory,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService : class
        {
            IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                    .Register(cc => factory(new ResolverContext(new AutofacResolver(cc))))
                    .OnActivating(args =>
                                  {
                                      TService instance = _decoratorService.Decorate(args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                                      args.ReplaceInstance(instance);
                                  });
            if (lifetime == Lifetime.Singleton)
            {
                registration.SingleInstance();
            }
            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        public void Register(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                    .RegisterType(implementationType)
                    .As(serviceType)
                    .OnActivating(args =>
                                  {
                                      object instance = _decoratorService.Decorate(serviceType, args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                                      args.ReplaceInstance(instance);
                                  });
            if (lifetime == Lifetime.Singleton)
            {
                registration.SingleInstance();
            }
            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        public void RegisterType(
            Type serviceType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                    .RegisterType(serviceType)
                    .OnActivating(args =>
                                  {
                                      object instance = _decoratorService.Decorate(args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                                      args.ReplaceInstance(instance);
                                  });
            if (lifetime == Lifetime.Singleton)
            {
                registration.SingleInstance();
            }
            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        public void RegisterGeneric(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
        {
            IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registration = _containerBuilder
                    .RegisterGeneric(implementationType).As(serviceType);
            if (lifetime == Lifetime.Singleton)
            {
                registration.SingleInstance();
            }
        }

        public void Decorate<TService>(Func<IResolverContext, TService, TService> factory)
        {
            _decoratorService.AddDecorator(factory);
        }

        public void RegisterAssemblyByConvention(Assembly assembly)
        {
            _containerBuilder.RegisterAssemblyByConvention(assembly);
        }

        public IRootResolver CreateResolver()
        {
            IContainer container = _containerBuilder.Build();
            return new AutofacRootResolver(container);
        }
    }
}
