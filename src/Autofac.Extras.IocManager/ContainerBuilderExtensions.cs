using System;
using System.Collections.Generic;

namespace Autofac.Extras.IocManager
{
    internal static class ContainerBuilderExtensions
    {
        public static T GetInstanceOrAdd<T>(this ContainerBuilder cb, string key, Func<T> factory)
        {
            if (!cb.Properties.TryGetValue(key, out object value))
            {
                cb.Properties[key] = factory();
            }

            return (T)value;
        }

        public static T GetInstance<T>(this ContainerBuilder cb, string key)
        {
            if (!cb.Properties.TryGetValue(key, out object value))
            {
                throw new KeyNotFoundException($"Given key({key}) not present in the ContainerBuilder.Properties dictionary");
            }

            return (T)value;
        }
    }
}
