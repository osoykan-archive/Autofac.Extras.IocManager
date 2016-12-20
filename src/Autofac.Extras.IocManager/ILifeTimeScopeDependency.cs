using Autofac.Builder;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Marks an interface's or class's lifetime to be
    ///     <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.InstancePerLifetimeScope"></see>
    ///     All classes implement this interface are automatically registered to dependency injection as
    ///     <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.InstancePerLifetimeScope" /> object.
    /// </summary>
    /// See also:
    /// <seealso cref="Autofac.Extras.IocManager.ILifetime" />
    public interface ILifetimeScopeDependency : ILifetime {}
}
