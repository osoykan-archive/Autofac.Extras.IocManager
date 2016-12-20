namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     This interface is used to directly perform dependency injection tasks.
    /// </summary>
    public interface IIocManager : IIocResolver
    {
        /// <summary>
        ///     Reference to the Autofac Container.
        /// </summary>
        IRootResolver Resolver { get; }
    }
}
