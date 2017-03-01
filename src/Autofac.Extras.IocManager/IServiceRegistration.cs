using System;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public interface IServiceRegistration
    {
        /// <summary>
        ///     Occurs when [registration completed].
        /// </summary>
        event EventHandler<RegistrationCompletedEventArgs> RegistrationCompleted;

        /// <summary>
        ///     Registers the specified lifetime.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register<TService1, TService2, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService1, TService2
            where TService1 : class
            where TService2 : class;

        /// <summary>
        ///     Registers the specified lifetime.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TService3">The type of the service3.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register<TService1, TService2, TService3, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService1, TService2, TService3
            where TService1 : class
            where TService2 : class
            where TService3 : class;

        /// <summary>
        ///     Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register<TService1, TService2, TImplementation>(
            TImplementation instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService1, TService2
            where TService1 : class
            where TService2 : class;

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
        void Register<TService1, TService2, TService3, TImplementation>(
            TImplementation instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService1, TService2, TService3
            where TService1 : class
            where TService2 : class
            where TService3 : class;

        /// <summary>
        ///     Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <typeparam name="TService3"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register<TService1, TService2, TService3>(
            object instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class
            where TService3 : class;

        /// <summary>
        ///     Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService1">The type of the service1.</typeparam>
        /// <typeparam name="TService2">The type of the service2.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register<TService1, TService2>(
            object instance,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService1 : class
            where TService2 : class;

        /// <summary>
        ///     Registers the specified lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register<TService, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService
            where TService : class;

        /// <summary>
        ///     Registers if absent.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        void RegisterIfAbsent<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        ///     Registers if absent.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        void RegisterIfAbsent<TService>(Lifetime lifetime = Lifetime.Transient)
            where TService : class;

        /// <summary>
        ///     Registers if absent.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime.</param>
        void RegisterIfAbsent(Type serviceType, Type implementationType, Lifetime lifetime = Lifetime.Transient);

        /// <summary>
        ///     Registers if absent.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="lifetime">The lifetime.</param>
        void RegisterIfAbsent(Type type, Lifetime lifetime = Lifetime.Transient);

        /// <summary>
        ///     Registers the specified factory.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register<TService>(
            Func<IResolverContext, TService> factory,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService : class;

        /// <summary>
        ///     Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void Register(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        /// <summary>
        ///     Registers the type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void RegisterType(
            Type serviceType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        /// <summary>
        ///     Registers the type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void RegisterType<TService>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        /// <summary>
        ///     Registers the generic.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void RegisterGeneric(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        /// <summary>
        ///     Registers the generic.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="keepDefault">if set to <c>true</c> [keep default].</param>
        void RegisterGeneric<TService, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        /// <summary>
        ///     Decorates the specified factory.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="factory">The factory.</param>
        void Decorate<TService>(Func<IResolverContext, TService, TService> factory);

        /// <summary>
        ///     Registers the assembly by convention.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void RegisterAssemblyByConvention(Assembly assembly);

        /// <summary>
        ///     Registers the assembly as closed types of.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="closedServiceType">Type of the closed service.</param>
        /// <param name="lifetime">The lifetime.</param>
        void RegisterAssemblyAsClosedTypesOf(
            Assembly assembly,
            Type closedServiceType,
            Lifetime lifetime = Lifetime.Transient);

        /// <summary>
        ///     Uses the builder to register components.
        /// </summary>
        /// <param name="builderAction">The builder action.</param>
        void UseBuilder(Action<ContainerBuilder> builderAction);

        /// <summary>
        ///     Creates the resolver.
        /// </summary>
        /// <returns></returns>
        IRootResolver CreateResolver(bool ignoreStartableComponents = false);
    }
}
