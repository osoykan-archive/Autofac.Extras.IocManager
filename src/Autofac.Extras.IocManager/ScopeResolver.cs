namespace Autofac.Extras.IocManager
{
    internal class ScopeResolver : Resolver, IScopeResolver
    {
        private readonly ILifetimeScope _lifetimeScope;

        public ScopeResolver(ILifetimeScope lifetimeScope)
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
            return new ScopeResolver(_lifetimeScope.BeginLifetimeScope());
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
