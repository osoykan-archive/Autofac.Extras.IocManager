using System;

namespace Autofac.Extras.IocManager
{
    public class OnDisposingEventArgs : EventArgs
    {
        public IResolverContext Context { get; set; }

        public OnDisposingEventArgs(IResolver rootResolver)
        {
            Context = new ResolverContext(rootResolver);
        }
    }
}
