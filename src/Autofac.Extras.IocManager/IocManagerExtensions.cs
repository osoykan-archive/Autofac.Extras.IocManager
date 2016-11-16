using System;

namespace Autofac.Extras.IocManager
{
    public static class IocManagerExtensions
    {
        /// <summary>
        ///     Gets a <see cref="IocScopedResolver" /> object that starts a <see cref="ILifetimeScope" /> to resolved objects to
        ///     be Disposable.
        /// </summary>
        /// <param name="iocManager"></param>
        /// <returns>The instance object wrapped by <see cref="IocScopedResolver" /></returns>
        public static IIocScopedResolver CreateScope(this IIocManager iocManager)
        {
            return new IocScopedResolver(iocManager.Container.BeginLifetimeScope());
        }

        /// <summary>
        ///     Gets an <see cref="DisposableDependencyObjectWrapper{T}" /> object that wraps resolved object to be Disposable.
        /// </summary>
        /// <param name="iocManager">IIocManager object</param>
        /// <param name="type">Type of the object to get</param>
        /// <param name="argumentsAsAnonymousType">Constructor args to set when resolving is done</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}" /></returns>
        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocManager iocManager, Type type, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper(iocManager.Container.BeginLifetimeScope(), type, argumentsAsAnonymousType);
        }

        /// <summary>
        ///     Gets an <see cref="DisposableDependencyObjectWrapper{T}" /> object that wraps resolved object to be Disposable.
        /// </summary>
        /// <param name="iocManager">IIocManager object</param>
        /// <param name="type">Type of the object to get</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}" /></returns>
        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocManager iocManager, Type type)
        {
            return new DisposableDependencyObjectWrapper(iocManager.Container.BeginLifetimeScope(), type, null);
        }

        /// <summary>
        ///     Gets an <see cref="DisposableDependencyObjectWrapper{T}" /> object that wraps resolved object to be Disposable.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <param name="iocManager">IIocManager object</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}" /></returns>
        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocManager iocManager)
        {
            return new DisposableDependencyObjectWrapper<T>(iocManager.Container.BeginLifetimeScope(), typeof(T), null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <param name="iocManager">IIocManager object</param>
        /// <param name="argumentsAsAnonymousType">Constructor args to set when resolving is done</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}" /></returns>
        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocManager iocManager, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper<T>(iocManager.Container.BeginLifetimeScope(), typeof(T), argumentsAsAnonymousType);
        }

        /// <summary>
        ///     This method can be used to resolve and release an object automatically.
        ///     You can use the object in <see cref="doAction" />.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <param name="iocManager">IIocManager object</param>
        /// <param name="doAction"></param>
        public static void ResolveUsing<T>(this IIocManager iocManager, Action<T> doAction)
        {
            using (var wrapper = iocManager.ResolveAsDisposable<T>())
            {
                doAction(wrapper.Object);
            }
        }

        /// <summary>
        ///     This method can be used to resolve and release an object automatically.
        ///     You can use the object in <see cref="func" /> and return a value.
        /// </summary>
        /// <typeparam name="TService">Type of the service to use</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="iocManager">IIocManager object</param>
        /// <param name="func">A function that can use the resolved object</param>
        /// <returns></returns>
        public static TReturn ResolveUsing<TService, TReturn>(this IIocManager iocManager, Func<TService, TReturn> func)
        {
            using (var wrapper = iocManager.ResolveAsDisposable<TService>())
            {
                return func(wrapper.Object);
            }
        }

        /// <summary>
        ///     This method starts a scope to resolve and release all objects automatically.
        ///     You can use the <c>scope</c> in <see cref="action" />.
        /// </summary>
        /// <param name="iocResolver">IIocResolver object</param>
        /// <param name="action">An action that can use the resolved object</param>
        public static void UsingScope(this IIocManager iocResolver, Action<IIocScopedResolver> action)
        {
            using (var scope = iocResolver.CreateScope())
            {
                action(scope);
            }
        }
    }
}
