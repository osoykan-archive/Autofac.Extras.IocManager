using System;

namespace Autofac.Extras.IocManager
{
    public interface IDecoratorService
    {
        TService Decorate<TService>(TService implementation, IResolverContext resolverContext);
        object Decorate(Type serviceType, object implementation, IResolverContext resolverContext);
    }
}