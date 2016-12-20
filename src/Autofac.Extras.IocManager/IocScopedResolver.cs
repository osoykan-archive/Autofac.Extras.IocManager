using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.IIocScopedResolver" />
    public class IocScopedResolver : IIocScopedResolver
    {
        /// <summary>
        ///     The scope
        /// </summary>
        private readonly IScopeResolver _scope;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IocScopedResolver" /> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public IocScopedResolver(IScopeResolver scope)
        {
            _scope = scope;
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
            return _scope.IsRegistered(type);
        }

        /// <summary>
        ///     Determines whether this instance is registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///     <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        /// <summary>
        ///     Resolves this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        /// <summary>
        ///     Resolves the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public T Resolve<T>(Type type)
        {
            return (T)_scope.Resolve(type);
        }

        /// <summary>
        ///     Resolves the specified arguments as anonymous type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumentsAsAnonymousType">Type of the arguments as anonymous.</param>
        /// <returns></returns>
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return _scope.Resolve<T>(argumentsAsAnonymousType);
        }

        /// <summary>
        ///     Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }

        /// <summary>
        ///     Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="argumentsAsAnonymousType">Type of the arguments as anonymous.</param>
        /// <returns></returns>
        public object Resolve(Type type, object argumentsAsAnonymousType)
        {
            return _scope.Resolve(type, argumentsAsAnonymousType);
        }

        /// <summary>
        ///     Resolves all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return _scope.Resolve<IEnumerable<T>>().ToArray();
        }

        /// <summary>
        ///     Resolves all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumentsAsAnonymousType">Type of the arguments as anonymous.</param>
        /// <returns></returns>
        public T[] ResolveAll<T>(object argumentsAsAnonymousType)
        {
            return _scope.Resolve<IEnumerable<T>>(argumentsAsAnonymousType).ToArray();
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
