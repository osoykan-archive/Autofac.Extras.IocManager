using System;

namespace Autofac.Extras.IocManager
{
    public interface IScopeResolver : IResolver, IDisposable
    {
        IScopeResolver BeginScope();
    }
}