using System;

using Autofac.Extras.IocManager.Extensions;

namespace Autofac.Extras.IocManager
{
    internal class DisposableDependencyObjectWrapper : DisposableDependencyObjectWrapper<object>, IDisposableDependencyObjectWrapper
    {
        public DisposableDependencyObjectWrapper(ILifetimeScope scope, Type serviceType, object argumentsAsAnonymousType)
            : base(scope, serviceType, argumentsAsAnonymousType) {}
    }

    internal class DisposableDependencyObjectWrapper<T> : IDisposableDependencyObjectWrapper<T>
    {
        private readonly ILifetimeScope _scope;

        public DisposableDependencyObjectWrapper(ILifetimeScope scope, Type serviceType, object argumentsAsAnonymousType)
        {
            _scope = scope;
            if (argumentsAsAnonymousType != null)
            {
                Object = (T)_scope.Resolve(serviceType, argumentsAsAnonymousType.GetTypedResolvingParameters());
            }
            else
            {
                Object = (T)_scope.Resolve(serviceType);
            }
        }

        public T Object { get; }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}