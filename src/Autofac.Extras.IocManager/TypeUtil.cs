using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Autofac.Extras.IocManager
{
    public static class TypeUtil
    {
        public static FieldInfo[] GetAllFields(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.GetTypeInfo().IsClass == false)
            {
                throw new ArgumentException(string.Format("Type {0} is not a class type. This method supports only classes", type));
            }

            var fields = new List<FieldInfo>();
            Type currentType = type;
            while (currentType != typeof(object))
            {
                Debug.Assert(currentType != null);
                FieldInfo[] currentFields = currentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                fields.AddRange(currentFields);
                currentType = currentType.GetTypeInfo().BaseType;
            }

            return fields.ToArray();
        }

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

                Type[] innerInterfaces = type.GetInterfaces();
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

        public static Type GetTypeOrNull(object target)
        {
            if (target == null)
            {
                return null;
            }

            return target.GetType();
        }

        public static Type[] AsTypeArray(this GenericTypeParameterBuilder[] typeInfos)
        {
            var types = new Type[typeInfos.Length];
            for (var i = 0; i < types.Length; i++)
            {
                types[i] = typeInfos[i].AsType();
            }

            return types;
        }

        public static bool IsFinalizer(this MethodInfo methodInfo)
        {
            return string.Equals("Finalize", methodInfo.Name) && methodInfo.GetBaseDefinition().DeclaringType == typeof(object);
        }

        public static bool IsGetType(this MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType == typeof(object) && string.Equals("GetType", methodInfo.Name, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsMemberwiseClone(this MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType == typeof(object) && string.Equals("MemberwiseClone", methodInfo.Name, StringComparison.OrdinalIgnoreCase);
        }

        public static MemberInfo[] Sort(MemberInfo[] members)
        {
            var sortedMembers = new MemberInfo[members.Length];
            Array.Copy(members, sortedMembers, members.Length);
            Array.Sort(sortedMembers, (l, r) => string.Compare(l.Name, r.Name, StringComparison.OrdinalIgnoreCase));
            return sortedMembers;
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
