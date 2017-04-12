using System;

namespace Autofac.Extras.IocManager
{
    internal class RootResolver : ScopeResolver, IRootResolver
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RootResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RootResolver(IContainer container)
            : base(container)
        {
            Container = container;
        }

        public event EventHandler<OnDisposingEventArgs> OnDisposing;

        /// <summary>
        ///     Occurs when [on disposing].
        /// </summary>
        /// <summary>
        ///     Gets the Autofac Container.
        /// </summary>
        /// <value>
        ///     The Autofac Container.
        /// </value>
        public IContainer Container { get; }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public override void Dispose()
        {
            try
            {
                OnDisposing?.Invoke(this, new OnDisposingEventArgs(this));
            }
            finally
            {
                base.Dispose();
                Container.Dispose();
                if (OnDisposing != null) { OnDisposing -= OnDisposing; }
            }
        }
    }
}
