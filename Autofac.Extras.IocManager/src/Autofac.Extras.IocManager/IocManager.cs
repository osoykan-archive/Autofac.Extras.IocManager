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
        public IContainer Container { get; internal set; }

        /// <summary>
        ///     Checks whether given type is registered before.
        /// </summary>
        /// <param name="type">Type to check</param>
        public bool IsRegistered(Type type)
        {
            return Container.IsRegistered(type);
        }

        /// <summary>
        ///     Checks whether given type is registered before.
        /// </summary>
        /// <typeparam name="TType">Type to check</typeparam>
        public bool IsRegistered<TType>()
        {
            return Container.IsRegistered<TType>();
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="IIocResolver.Release" />) after usage.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <returns>The instance object</returns>
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="Release" />) after usage.
        /// </summary>
        /// <typeparam name="T">Type of the object to cast</typeparam>
        /// <param name="type">Type of the object to resolve</param>
        /// <returns>The object instance</returns>
        public T Resolve<T>(Type type)
        {
            return (T)Container.Resolve(type);
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="IIocResolver.Release" />) after usage.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The instance object</returns>
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return Container.Resolve<T>(argumentsAsAnonymousType.GetTypedResolvingParameters());
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="IIocResolver.Release" />) after usage.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <returns>The instance object</returns>
        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        /// <summary>
        ///     Gets an object from IOC container.
        ///     Returning object must be Released (see <see cref="IIocResolver.Release" />) after usage.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The instance object</returns>
        public object Resolve(Type type, object argumentsAsAnonymousType)
        {
            return Container.Resolve(type, argumentsAsAnonymousType.GetTypedResolvingParameters());
        }

        /// <inheritdoc />
        public T[] ResolveAll<T>()
        {
            return Container.Resolve<IEnumerable<T>>().ToArray();
        }

        /// <inheritdoc />
        public T[] ResolveAll<T>(object argumentsAsAnonymousType)
        {
            return Container.Resolve<IEnumerable<T>>(argumentsAsAnonymousType.GetTypedResolvingParameters()).ToArray();
        }
    }
}
