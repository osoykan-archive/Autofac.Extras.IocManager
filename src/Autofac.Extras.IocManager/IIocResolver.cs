using System;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Define interface for classes those are used to resolve dependencies.
    /// </summary>
    public interface IIocResolver
    {
        /// <summary>
        ///     Checks whether given type is registered before.
        /// </summary>
        /// <param name="type">Type to check</param>
        bool IsRegistered(Type type);

        /// <summary>
        ///     Checks whether given type is registered before.
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        bool IsRegistered<T>();

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be should be released by <see cref="ILifetimeScope" /> after usage automatically.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <returns>The object instance</returns>
        T Resolve<T>();

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be should be released by <see cref="ILifetimeScope" /> after usage automatically.
        /// </summary>
        /// <typeparam name="T">Type of the object to cast</typeparam>
        /// <param name="type">Type of the object to resolve</param>
        /// <returns>The object instance</returns>
        T Resolve<T>(Type type);

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be should be released by <see cref="ILifetimeScope" /> after usage automatically.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The object instance</returns>
        T Resolve<T>(object argumentsAsAnonymousType);

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be should be released by <see cref="ILifetimeScope" /> after usage automatically.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <returns>The object instance</returns>
        object Resolve(Type type);

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be should be released by <see cref="ILifetimeScope" /> after usage automatically.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The object instance</returns>
        object Resolve(Type type, object argumentsAsAnonymousType);

        /// <summary>
        ///     Gets all implementations for given type.
        ///     Returning objects must be Released after usage.
        /// </summary>
        /// <typeparam name="T">Type of the objects to resolve</typeparam>
        /// <returns>Object instances</returns>
        T[] ResolveAll<T>();

        /// <summary>
        ///     Gets all implementations for given type.
        ///     Returning objects must be Released after usage.
        /// </summary>
        /// <typeparam name="T">Type of the objects to resolve</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>Object instances</returns>
        T[] ResolveAll<T>(object argumentsAsAnonymousType);
    }
}
