using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    public class DecoratorService : IDecoratorService
    {
        private readonly ConcurrentDictionary<Type, List<Func<object, IResolverContext, object>>> _decorators = new ConcurrentDictionary<Type, List<Func<object, IResolverContext, object>>>();
        private readonly ConcurrentDictionary<Type, Func<object, IResolverContext, object>> _genericDecoratorCall = new ConcurrentDictionary<Type, Func<object, IResolverContext, object>>();

        /// <summary>
        ///     Decorates the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="resolverContext">The resolver context.</param>
        /// <returns></returns>
        public object Decorate(Type serviceType, object implementation, IResolverContext resolverContext)
        {
            Func<object, IResolverContext, object> decorate = _genericDecoratorCall.GetOrAdd(
                serviceType,
                t =>
                {
                    MethodInfo genericMethodInfo = GetType()
                        .GetMethods()
                        .Single(mi => mi.IsGenericMethodDefinition && mi.Name == "Decorate");
                    MethodInfo methodInfo = genericMethodInfo.MakeGenericMethod(t);
                    return ((o, r) => methodInfo.Invoke(this, new[] { o, r }));
                });
            return decorate(implementation, resolverContext);
        }

        /// <summary>
        ///     Decorates the specified implementation.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementation">The implementation.</param>
        /// <param name="resolverContext">The resolver context.</param>
        /// <returns></returns>
        public TService Decorate<TService>(TService implementation, IResolverContext resolverContext)
        {
            List<Func<object, IResolverContext, object>> decorators;
            return !_decorators.TryGetValue(typeof(TService), out decorators)
                ? implementation
                : decorators.Aggregate(implementation, (current, decorator) => (TService)decorator(current, resolverContext));
        }

        /// <summary>
        ///     Adds the decorator.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="factory">The factory.</param>
        public void AddDecorator<TService>(Func<IResolverContext, TService, TService> factory)
        {
            List<Func<object, IResolverContext, object>> decorators = _decorators.GetOrAdd(
                typeof(TService),
                new List<Func<object, IResolverContext, object>>());
            decorators.Add((s, c) => factory(c, (TService)s));
        }
    }
}
