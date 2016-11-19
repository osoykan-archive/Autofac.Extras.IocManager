using System;

namespace Autofac.Extras.IocManager
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DoNotInjectAttribute : Attribute
    {
    }
}
