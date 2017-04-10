using System;
using System.Collections.Generic;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    internal static class TypeUtil
    {
        /// <summary>
        ///     Returns list of all unique interfaces implemented given types, including their base interfaces.
        /// </summary>
        /// <param name="types"> </param>
        /// <returns> </returns>
        public static Type[] GetAllInterfaces(params Type[] types)
        {
            if (types == null)
            {
                return Type.EmptyTypes;
            }

            var interfaces = new HashSet<Type>();
            for (var index = 0; index < types.Length; index++)
            {
                Type type = types[index];
                if (type == null)
                {
                    continue;
                }

                if (type.GetTypeInfo().IsInterface)
                {
                    if (interfaces.Add(type) == false)
                    {
                        continue;
                    }
                }

                Type[] innerInterfaces = type.GetTypeInfo().GetInterfaces();
                for (var i = 0; i < innerInterfaces.Length; i++)
                {
                    Type @interface = innerInterfaces[i];
                    interfaces.Add(@interface);
                }
            }

            return Sort(interfaces);
        }

        public static Type[] GetAllInterfaces(this Type type)
        {
            return GetAllInterfaces(new[] { type });
        }

        private static Type[] Sort(ICollection<Type> types)
        {
            var array = new Type[types.Count];
            types.CopyTo(array, 0);

            //NOTE: is there a better, stable way to sort Types. We will need to revise this once we allow open generics
            Array.Sort(array, (l, r) => string.Compare(l.AssemblyQualifiedName, r.AssemblyQualifiedName, StringComparison.OrdinalIgnoreCase));
            return array;
        }
    }
}
