using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Autofac.Core;

namespace Autofac.Extras.IocManager
{
    internal class AutofacResolver : IResolver
    {
        /// <summary>
        ///     The component context
        /// </summary>
        private readonly IComponentContext _componentContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacResolver" /> class.
        /// </summary>
        /// <param name="componentContext">The component context.</param>
        public AutofacResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        /// <summary>
        ///     Resolves this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _componentContext.Resolve<T>();
        }

        /// <summary>
        ///     Resolves the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public object Resolve(Type serviceType)
        {
            return _componentContext.Resolve(serviceType);
        }

        /// <summary>
        ///     Resolves all.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            Type enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return ((IEnumerable)_componentContext.Resolve(enumerableType)).OfType<object>().ToList();
        }

        /// <summary>
        ///     Gets the registered services.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetRegisteredServices()
        {
            return _componentContext.ComponentRegistry.Registrations
                                    .SelectMany(x => x.Services)
                                    .OfType<TypedService>()
                                    .Where(x => !x.ServiceType.Name.StartsWith("Autofac"))
                                    .Select(x => x.ServiceType);
        }

        /// <summary>
        ///     Determines whether this instance is registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///     <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered<T>()
            where T : class
        {
            return IsRegistered(typeof(T));
        }

        /// <summary>
        ///     Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered(Type type)
        {
            return GetRegisteredServices().Any(t => t == type);
        }

        /// <summary>
        ///     Resolves the specified argument as anonymous type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumentAsAnonymousType">Type of the argument as anonymous.</param>
        /// <returns></returns>
        public T Resolve<T>(object argumentAsAnonymousType)
        {
            return _componentContext.Resolve<T>(argumentAsAnonymousType.GetTypedResolvingParameters());
        }

        /// <summary>
        ///     Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="argumentAsAnonymousType">Type of the argument as anonymous.</param>
        /// <returns></returns>
        public object Resolve(Type type, object argumentAsAnonymousType)
        {
            return _componentContext.Resolve(type, argumentAsAnonymousType.GetTypedResolvingParameters());
        }

        /// <summary>
        ///     Resolves the specified parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T Resolve<T>(params Parameter[] parameters)
        {
            return _componentContext.Resolve<T>(parameters);
        }

        /// <summary>
        ///     Resolves the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public object Resolve(Type serviceType, params Parameter[] parameters)
        {
            return _componentContext.Resolve(serviceType, parameters);
        }
    }
}
