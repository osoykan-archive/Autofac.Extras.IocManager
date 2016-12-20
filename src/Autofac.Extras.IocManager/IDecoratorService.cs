using System;

namespace Autofac.Extras.IocManager
{
    public interface IDecoratorService
    {
        /// <summary>
        ///     Decorates the specified implementation.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementation">The implementation.</param>
        /// <param name="resolverContext">The resolver context.</param>
        /// <returns></returns>
        TService Decorate<TService>(TService implementation, IResolverContext resolverContext);

        /// <summary>
        ///     Decorates the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="resolverContext">The resolver context.</param>
        /// <returns></returns>
        object Decorate(Type serviceType, object implementation, IResolverContext resolverContext);
    }
}
