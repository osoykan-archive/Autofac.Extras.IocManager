using Autofac.Extras.IocManager;

public class ResolverContext : IResolverContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResolverContext" /> class.
    /// </summary>
    /// <param name="resolver">The resolver.</param>
    public ResolverContext(IResolver resolver)
    {
        Resolver = resolver;
    }

    /// <summary>
    ///     Gets the resolver.
    /// </summary>
    /// <value>
    ///     The resolver.
    /// </value>
    public IResolver Resolver { get; }
}
