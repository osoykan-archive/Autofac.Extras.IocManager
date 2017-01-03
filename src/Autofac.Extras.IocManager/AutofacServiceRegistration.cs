using System;
using System.Reflection;

using Autofac.Builder;
using Autofac.Features.Scanning;

namespace Autofac.Extras.IocManager
{
    internal class AutofacServiceRegistration : IServiceRegistration
    {
        /// <summary>
        ///     The container builder
        /// </summary>
        private readonly ContainerBuilder _containerBuilder;

        /// <summary>
        ///     The decorator service
        /// </summary>
        private readonly DecoratorService _decoratorService = new DecoratorService();

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacServiceRegistration" /> class.
        /// </summary>
        public AutofacServiceRegistration() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacServiceRegistration" /> class.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        public AutofacServiceRegistration(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder ?? new ContainerBuilder();

            _containerBuilder.RegisterType<AutofacResolver>().As<IResolver>();
            _containerBuilder.RegisterType<AutofacScopeResolver>().As<IScopeResolver>();
            _containerBuilder.Register<IDecoratorService>(_ => _decoratorService).SingleInstance();
        }

        /// <summary>
        ///     Registers the specified lifetime.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService1, TService2, TImplementation>(Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterType<TImplementation>()
                .As<TService1, TService2>()
                .InjectPropertiesAsAutowired()
                .AsSelf();

            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService1, TService2, TImplementation>(
            TImplementation instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            IRegistrationBuilder<TImplementation, SimpleActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterInstance(instance)
                .As<TService1, TService2>()
                .AsSelf()
                .InjectPropertiesAsAutowired();

            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified lifetime.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TService3"></typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService1, TService2, TService3, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TImplementation : class, TService1, TService2, TService3
        {
            IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterType<TImplementation>()
                .As<TService1, TService2, TService3>()
                .InjectPropertiesAsAutowired()
                .AsSelf();

            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService1, TService2>(
            object instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
        {
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterInstance(instance).As<TService1, TService2>()
                .AsSelf()
                .InjectPropertiesAsAutowired();

            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TService3">The type of the service3.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService1, TService2, TService3, TImplementation>(
            TImplementation instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TImplementation : class, TService1, TService2, TService3
        {
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterInstance(instance).As<TService1, TService2, TService3>()
                .AsSelf()
                .InjectPropertiesAsAutowired();

            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TService3"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService1, TService2, TService3>(
            object instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
            where TService3 : class
        {
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterInstance(instance).As<TService1, TService2, TService3>()
                .AsSelf()
                .InjectPropertiesAsAutowired();

            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService
            where TService : class
        {
            IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterType<TImplementation>()
                .InjectPropertiesAsAutowired()
                .AsSelf();

            IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> serviceRegistration = _containerBuilder
                .Register<TService>(c => c.Resolve<TImplementation>())
                .As<TService>()
                .InjectPropertiesAsAutowired()
                .OnActivating(args =>
                {
                    TService instance = _decoratorService.Decorate(args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                    args.ReplaceInstance(instance);
                });

            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                serviceRegistration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified factory.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register<TService>(
            Func<IResolverContext, TService> factory,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService : class
        {
            IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .Register(cc => factory(new ResolverContext(new AutofacResolver(cc))))
                .InjectPropertiesAsAutowired()
                .OnActivating(args =>
                {
                    TService instance = _decoratorService.Decorate(args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                    args.ReplaceInstance(instance);
                });
            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void Register(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterType(implementationType)
                .As(serviceType)
                .InjectPropertiesAsAutowired()
                .OnActivating(args =>
                {
                    object instance = _decoratorService.Decorate(serviceType, args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                    args.ReplaceInstance(instance);
                });
            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void RegisterType(
            Type serviceType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration = _containerBuilder
                .RegisterType(serviceType)
                .InjectPropertiesAsAutowired()
                .OnActivating(args =>
                {
                    object instance = _decoratorService.Decorate(args.Instance, new ResolverContext(new AutofacResolver(args.Context)));
                    args.ReplaceInstance(instance);
                });
            registration.ApplyLifeStyle(lifetime);

            if (keepDefault)
            {
                registration.PreserveExistingDefaults();
            }
        }

        /// <summary>
        ///     Registers the type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void RegisterType<TService>(Lifetime lifetime = Lifetime.Transient, bool keepDefault = false)
        {
            RegisterType(typeof(TService), lifetime, keepDefault);
        }

        /// <summary>
        ///     Registers the generic.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void RegisterGeneric(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
        {
            IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registration = _containerBuilder
                .RegisterGeneric(implementationType)
                .As(serviceType)
                .InjectPropertiesAsAutowired();

            registration.ApplyLifeStyle(lifetime);
        }

        /// <summary>
        ///     Registers the generic.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        public void RegisterGeneric<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient, bool keepDefault = false)
        {
            RegisterGeneric(typeof(TService), typeof(TImplementation), lifetime, keepDefault);
        }

        /// <summary>
        ///     Decorates the specified factory.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="factory">The factory.</param>
        public void Decorate<TService>(Func<IResolverContext, TService, TService> factory)
        {
            _decoratorService.AddDecorator(factory);
        }

        /// <summary>
        ///     Registers the assembly by convention.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void RegisterAssemblyByConvention(Assembly assembly)
        {
            _containerBuilder.RegisterAssemblyByConvention(assembly);
        }

        /// <summary>
        ///     Registers the assembly as closed types of.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="closedServiceType">Type of the closed service.</param>
        /// <param name="lifetime">The lifetime.</param>
        public void RegisterAssemblyAsClosedTypesOf(
            Assembly assembly,
            Type closedServiceType,
            Lifetime lifetime = Lifetime.Transient)
        {
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration = _containerBuilder
                .RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(closedServiceType)
                .AsImplementedInterfaces()
                .InjectPropertiesAsAutowired();

            registration.ApplyLifeStyle(lifetime);
        }

        /// <summary>
        ///     Creates the resolver.
        /// </summary>
        /// <returns></returns>
        public IRootResolver CreateResolver()
        {
            IContainer container = _containerBuilder.Build();
            return new AutofacRootResolver(container);
        }

        /// <summary>
        ///     Uses the builder to register components.
        /// </summary>
        /// <param name="builderAction">The builder action.</param>
        public void UseBuilder(Action<ContainerBuilder> builderAction)
        {
            builderAction(_containerBuilder);
        }
    }
}
