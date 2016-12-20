using System;
using System.Collections.Generic;

namespace Autofac.Extras.IocManager
{
    public interface IResolver
    {
        T Resolve<T>();

        T Resolve<T>(object argumentAsAnonymousType);

        object Resolve(Type type, object argumentAsAnonymousType);

        object Resolve(Type serviceType);

        IEnumerable<object> ResolveAll(Type serviceType);

        IEnumerable<Type> GetRegisteredServices();

        bool HasRegistrationFor<T>() where T : class;

        bool HasRegistrationFor(Type type);
    }
}
