namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     This interface is used to directly perform dependency injection tasks.
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.IIocResolver" />
    public interface IIocManager : IIocResolver
    {
        /// <summary>
        ///     Reference to the Autofac Container.
        /// </summary>
        /// <value>
        ///     The resolver.
        /// </value>
        IRootResolver Resolver { get; }
    }
}
