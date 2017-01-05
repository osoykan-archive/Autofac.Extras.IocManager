using System;
using System.Collections.Generic;

namespace Autofac.Extras.IocManager
{
    public interface IResolver
    {
        /// <summary>
        ///     Resolves this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        ///     Resolves the specified argument as anonymous type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumentAsAnonymousType">Type of the argument as anonymous.</param>
        /// <returns></returns>
        T Resolve<T>(object argumentAsAnonymousType);

        /// <summary>
        ///     Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="argumentAsAnonymousType">Type of the argument as anonymous.</param>
        /// <returns></returns>
        object Resolve(Type type, object argumentAsAnonymousType);

        /// <summary>
        ///     Resolves the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        object Resolve(Type serviceType);

        /// <summary>
        ///     Resolves all.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type serviceType);

        /// <summary>
        ///     Gets the registered services.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetRegisteredServices();

        /// <summary>
        ///     Determines whether this instance is registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///     <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered<T>() where T : class;

        /// <summary>
        ///     Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(Type type);
    }
}
