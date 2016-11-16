using System;

namespace Autofac.Extras.IocManager
{
    public static class IocManagerExtensions
    {
        public static IIocScopedResolver CreateScope(this IIocManager iocManager)
        {
            return new IocScopedResolver(iocManager.Container.BeginLifetimeScope());
        }

        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocManager iocManager, Type type, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper(iocManager.Container.BeginLifetimeScope(), type, argumentsAsAnonymousType);
        }

        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocManager iocManager, Type type)
        {
            return new DisposableDependencyObjectWrapper(iocManager.Container.BeginLifetimeScope(), type, null);
        }

        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocManager iocManager)
        {
            return new DisposableDependencyObjectWrapper<T>(iocManager.Container.BeginLifetimeScope(), typeof(T), null);
        }

        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocManager iocManager, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper<T>(iocManager.Container.BeginLifetimeScope(), typeof(T), argumentsAsAnonymousType);
        }

        public static void ResolveUsing<T>(this IIocManager iocManager, Action<T> doAction)
        {
            using (IDisposableDependencyObjectWrapper<T> wrapper = iocManager.ResolveAsDisposable<T>())
            {
                doAction(wrapper.Object);
            }
        }

        public static TReturn ResolveUsing<TService, TReturn>(this IIocManager iocManager, Func<TService, TReturn> func)
        {
            using (IDisposableDependencyObjectWrapper<TService> wrapper = iocManager.ResolveAsDisposable<TService>())
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
            using (IIocScopedResolver scope = iocResolver.CreateScope())
            {
                action(scope);
            }
        }
    }
}
