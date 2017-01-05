using System;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     This interface is used to wrap an object that is resolved from <see cref="ILifetimeScope"/>.
    ///     It inherits <see cref="IDisposable" />, so resolved object can be easily released.
    ///     In <see cref="IDisposable.Dispose" /> method which is called to dispose the object.
    ///     This is non-generic version of <see cref="IDisposableDependencyObjectWrapper{T}" /> interface.
    /// </summary>
    public interface IDisposableDependencyObjectWrapper : IDisposableDependencyObjectWrapper<object> {}
}