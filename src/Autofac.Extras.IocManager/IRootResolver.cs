namespace Autofac.Extras.IocManager
{
    public interface IRootResolver : IScopeResolver
    {
        IContainer Container { get; }
    }
}
