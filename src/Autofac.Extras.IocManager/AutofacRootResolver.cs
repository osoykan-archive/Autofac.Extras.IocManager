namespace Autofac.Extras.IocManager
{
    internal class AutofacRootResolver : AutofacScopeResolver, IRootResolver
    {
        public AutofacRootResolver(IContainer container)
            : base(container)
        {
            Container = container;
        }

        public IContainer Container { get; }

        public override void Dispose()
        {
            base.Dispose();
            Container.Dispose();
        }
    }
}