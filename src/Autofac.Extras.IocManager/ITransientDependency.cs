namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     All classes implement this interface are automatically registered to dependency injection as transient object.
    /// </summary>
    public interface ITransientDependency : ILifetime
    {
    }
}
