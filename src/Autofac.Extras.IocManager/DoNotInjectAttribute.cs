using System;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Marks to property injectable objects as non-injectable.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class DoNotInjectAttribute : Attribute {}
}
