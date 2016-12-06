using Autofac.Extras.IocManager;

public class ResolverContext : IResolverContext
{
    public IResolver Resolver { get; }

    public ResolverContext(IResolver resolver)
    {
        Resolver = resolver;
    }
}