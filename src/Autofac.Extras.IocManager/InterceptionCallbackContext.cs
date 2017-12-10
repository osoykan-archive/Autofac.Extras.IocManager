using System;
using System.Collections.Generic;

namespace Autofac.Extras.IocManager
{
    public class InterceptionCallbackContext
    {
        public ICollection<Type> InterceptorTypes;
        public Func<Type, bool> Selector;

        public InterceptionCallbackContext(ICollection<Type> interceptorTypes, Func<Type, bool> selector)
        {
            InterceptorTypes = interceptorTypes;
            Selector = selector;
        }
    }

    public class InterceptionCallbackContextList : List<InterceptionCallbackContext>
    {
    }
}
