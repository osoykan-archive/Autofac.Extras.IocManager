using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     This class is used to directly perform dependency injection tasks.
    /// </summary>
    public class IocManager : IIocManager
    {
        static IocManager()
        {
            Instance = new IocManager();
        }

        /// <summary>
        ///     The Singleton instance.
        /// </summary>
        public static IocManager Instance { get; private set; }

        /// <summary>
        ///     Reference to the Autofac Container.
        /// </summary>
        public IRootResolver Resolver { get; internal set; }

        /// <summary>
        ///     Checks whether given type is registered before.
        /// </summary>
        /// <param name="type">Type to check</param>
        public bool IsRegistered(Type type)
        {
            return Resolver.HasRegistrationFor(type);
        }

        /// <summary>
        ///     Checks whether given type is registered before.
        /// </summary>
        /// <typeparam name="TType">Type to check</typeparam>
        public bool IsRegistered<TType>()
        {
            return IsRegistered(typeof(TType));
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="ILifetimeScope" />) after usage.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <returns>The instance object</returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="ILifetimeScope" />) after usage.
        /// </summary>
        /// <typeparam name="T">Type of the object to cast</typeparam>
        /// <param name="type">Type of the object to resolve</param>
        /// <returns>The object instance</returns>
        public T Resolve<T>(Type type)
        {
            return (T)Resolver.Resolve(type);
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="ILifetimeScope" />) after usage.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The instance object</returns>
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return Resolver.Resolve<T>(argumentsAsAnonymousType.GetTypedResolvingParameters());
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="ILifetimeScope" />) after usage.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <returns>The instance object</returns>
        public object Resolve(Type type)
        {
            return Resolver.Resolve(type);
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="ILifetimeScope" />) after usage.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The instance object</returns>
        public object Resolve(Type type, object argumentsAsAnonymousType)
        {
            return Resolver.Resolve(type, argumentsAsAnonymousType);
        }

        /// <summary>
        ///     Gets all implementations for given type.
        ///     Returning objects must be Released after usage.
        /// </summary>
        /// <typeparam name="T">Type of the objects to resolve</typeparam>
        /// <returns>Object instances</returns>
        public T[] ResolveAll<T>()
        {
            return Resolver.Resolve<IEnumerable<T>>().ToArray();
        }

        /// <summary>
        ///     Gets all implementations for given type.
        ///     Returning objects must be Released after usage.
        /// </summary>
        /// <typeparam name="T">Type of the objects to resolve</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>Object instances</returns>
        public T[] ResolveAll<T>(object argumentsAsAnonymousType)
        {
            return Resolver.Resolve<IEnumerable<T>>(argumentsAsAnonymousType).ToArray();
        }
    }
}
