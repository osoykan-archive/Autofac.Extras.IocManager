using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Extras.IocManager
{
    public class IocScopedResolver : IIocScopedResolver
    {
        private readonly IScopeResolver _scope;

        public IocScopedResolver(IScopeResolver scope)
        {
            _scope = scope;
        }

        public bool IsRegistered(Type type)
        {
            return _scope.HasRegistrationFor(type);
        }

        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        public T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        public T Resolve<T>(Type type)
        {
            return (T)_scope.Resolve(type);
        }

        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return _scope.Resolve<T>(argumentsAsAnonymousType);
        }

        public object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }

        public object Resolve(Type type, object argumentsAsAnonymousType)
        {
            return _scope.Resolve(type, argumentsAsAnonymousType);
        }

        public T[] ResolveAll<T>()
        {
            return _scope.Resolve<IEnumerable<T>>().ToArray();
        }

        public T[] ResolveAll<T>(object argumentsAsAnonymousType)
        {
            return _scope.Resolve<IEnumerable<T>>(argumentsAsAnonymousType).ToArray();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
