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

        /// <summary>
        ///     Performs application-defined <see cref="ILifetimeScope" /> tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}
