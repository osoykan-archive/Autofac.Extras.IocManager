namespace Autofac.Extras.IocManager
{
    internal class AutofacRootResolver : AutofacScopeResolver, IRootResolver
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacRootResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public AutofacRootResolver(IContainer container)
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
