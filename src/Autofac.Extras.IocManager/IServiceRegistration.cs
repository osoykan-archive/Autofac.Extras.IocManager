using System;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public interface IServiceRegistration
    {
        void Register<TService1, TService2, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService1, TService2
            where TService1 : class
            where TService2 : class;

        void Register<TService, TImplementation>(
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TImplementation : class, TService
            where TService : class;

        void Register<TService>(
            Func<IResolverContext, TService> factory,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false)
            where TService : class;

        void Register(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        void RegisterType(
            Type serviceType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        void RegisterGeneric(
            Type serviceType,
            Type implementationType,
            Lifetime lifetime = Lifetime.Transient,
            bool keepDefault = false);

        void Decorate<TService>(Func<IResolverContext, TService, TService> factory);

        void RegisterAssemblyByConvention(Assembly assembly);

        IRootResolver CreateResolver();
    }
}
