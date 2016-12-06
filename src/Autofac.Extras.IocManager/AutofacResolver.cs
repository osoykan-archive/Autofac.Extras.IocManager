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

        public T Resolve<T>(params Parameter[] parameters)
        {
            return _componentContext.Resolve<T>(parameters);
        }

        public object Resolve(Type serviceType)
        {
            return _componentContext.Resolve(serviceType);
        }

        public object Resolve(Type serviceType, params Parameter[] parameters)
        {
            return _componentContext.Resolve(serviceType, parameters);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
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

        public bool HasRegistrationFor<T>()
            where T : class
        {
            var serviceType = typeof(T);
            return GetRegisteredServices().Any(t => serviceType == t);
        }
    }
}