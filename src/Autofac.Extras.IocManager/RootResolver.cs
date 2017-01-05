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
            base.Dispose();
            Container.Dispose();
        }
    }
}
