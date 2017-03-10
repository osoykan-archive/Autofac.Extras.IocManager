using System;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Root resolver for IocBuilder. This is not registered in the Autofac Container.
    ///     Just for resolving objects after container built.
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.IScopeResolver" />
    public interface IRootResolver : IScopeResolver
    {
        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        IContainer Container { get; }

        /// <summary>
        ///     Occurs when [on disposing].
        /// </summary>
        event EventHandler<OnDisposingEventArgs> OnDisposing;
    }
}
