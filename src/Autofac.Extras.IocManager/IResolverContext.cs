namespace Autofac.Extras.IocManager
{
    public interface IResolverContext
    {
        /// <summary>
        ///     Gets the resolver.
        /// </summary>
        /// <value>
        ///     The resolver.
        /// </value>
        IResolver Resolver { get; }
    }
}
