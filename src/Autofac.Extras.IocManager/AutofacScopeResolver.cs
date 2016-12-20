namespace Autofac.Extras.IocManager
{
    internal class AutofacScopeResolver : AutofacResolver, IScopeResolver
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacScopeResolver(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        ///     Begins the lifetimescope.
        /// </summary>
        /// <returns>
        ///     <see cref="IScopeResolver" />
        /// </returns>
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
