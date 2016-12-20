using System;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Scoped resolver interface for scoping any resolution context.
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.IIocResolver" />
    /// <seealso cref="System.IDisposable" />
    public interface IIocScopedResolver : IIocResolver, IDisposable {}
}
