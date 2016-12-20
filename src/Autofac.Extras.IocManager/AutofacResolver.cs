using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Autofac.Core;

namespace Autofac.Extras.IocManager
{
    internal class AutofacResolver : IResolver
    {
        private readonly IComponentContext _componentContext;

        public AutofacResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public T Resolve<T>()
        {
            return _componentContext.Resolve<T>();
        }

        public object Resolve(Type serviceType)
        {
            return _componentContext.Resolve(serviceType);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            Type enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return ((IEnumerable)_componentContext.Resolve(enumerableType)).OfType<object>().ToList();
        }

        public IEnumerable<Type> GetRegisteredServices()
        {
            return _componentContext.ComponentRegistry.Registrations
                                    .SelectMany(x => x.Services)
                                    .OfType<TypedService>()
                                    .Where(x => !x.ServiceType.Name.StartsWith("Autofac"))
                                    .Select(x => x.ServiceType);
        }

        public bool IsRegistered<T>()
            where T : class
        {
            return IsRegistered(typeof(T));
        }

        public bool IsRegistered(Type type)
        {
            return GetRegisteredServices().Any(t => t == type);
        }

        public T Resolve<T>(object argumentAsAnonymousType)
        {
            return _componentContext.Resolve<T>(argumentAsAnonymousType.GetTypedResolvingParameters());
        }

        public object Resolve(Type type, object argumentAsAnonymousType)
        {
            return _componentContext.Resolve(type, argumentAsAnonymousType.GetTypedResolvingParameters());
        }

        public T Resolve<T>(params Parameter[] parameters)
        {
            return _componentContext.Resolve<T>(parameters);
        }

        public object Resolve(Type serviceType, params Parameter[] parameters)
        {
            return _componentContext.Resolve(serviceType, parameters);
        }
    }
}
