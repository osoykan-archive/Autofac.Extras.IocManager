using Autofac.Builder;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     <seealso cref="Autofac.Extras.IocManager.ILifetime" />
    ///     Marks an interface's or class's lifetime to be
    ///     <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.SingleInstance"></see>
    ///     All classes implement this interface are automatically registered to dependency injection as
    ///     <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.SingleInstance" /> object.
    /// </summary>
    public interface ISingletonDependency : ILifetime {}
}
