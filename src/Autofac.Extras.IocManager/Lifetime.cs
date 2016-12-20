namespace Autofac.Extras.IocManager
{
    public enum Lifetime
    {
        /// <summary>
        ///     The transient lifetime.
        /// </summary>
        Transient,

        /// <summary>
        ///     The lifetimescope lifetime.
        /// </summary>
        LifetimeScope,

        /// <summary>
        ///     The singleton lifetime.
        /// </summary>
        Singleton
    }
}
