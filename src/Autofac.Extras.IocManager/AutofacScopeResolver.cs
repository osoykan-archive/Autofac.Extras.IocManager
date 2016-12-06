namespace Autofac.Extras.IocManager
{
    internal class AutofacScopeResolver : AutofacResolver, IScopeResolver
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacScopeResolver(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
            _lifetimeScope = lifetimeScope.BeginLifetimeScope();
        }

        public IScopeResolver BeginScope()
        {
            return new AutofacScopeResolver(_lifetimeScope.BeginLifetimeScope());
        }

        public virtual void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}