using System;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Applies an abstraction to Autofac Root Resolver to be Scoped Resolver.
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.IResolver" />
    /// <seealso cref="System.IDisposable" />
    public interface IScopeResolver : IResolver, IDisposable
    {
        /// <summary>
        ///     Begins and wraps the Autofac's <see cref="ILifetimeScope" />.
        /// </summary>
        /// <returns></returns>
        IScopeResolver BeginScope();
    }
}
