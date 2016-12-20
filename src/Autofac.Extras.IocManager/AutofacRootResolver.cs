namespace Autofac.Extras.IocManager
{
    internal class AutofacRootResolver : AutofacScopeResolver, IRootResolver
    {
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

        public override void Dispose()
        {
            base.Dispose();
            Container.Dispose();
        }
    }
}
