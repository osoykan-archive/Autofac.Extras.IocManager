using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        ///     The pretty print cache
        /// </summary>
        private static readonly ConcurrentDictionary<Type, string> PrettyPrintCache = new ConcurrentDictionary<Type, string>();

        /// <summary>
        ///     The type cache keys
        /// </summary>
        private static readonly ConcurrentDictionary<Type, string> TypeCacheKeys = new ConcurrentDictionary<Type, string>();

        /// <summary>
        ///     Finds default interfaces a givent type and given type adds itsel in returned list.
        /// </summary>
        /// <param name="this">The this.</param>
        /// <returns>
        ///     Default interface types with adding given type itself
        /// </returns>
        public static Type[] GetDefaultInterfacesWithSelf(this Type @this)
        {
            Type[] types = @this.GetAllInterfaces()
                                .Where(i => @this.Name.Contains(GetInterfaceName(i)))
                                .ToArray();
            return types.Prepend(@this).ToArray();
        }

        /// <summary>
        ///     Finds default interfaces a givent type.
        ///     <code>
        /// class SimpleDependency : ISimpleDependency
        /// {
        /// }
        /// </code>
        /// </summary>
        /// <param name="this">Type to search</param>
        /// <returns>
        ///     Default interface types
        /// </returns>
        public static Type[] GetDefaultInterfaces(this Type @this)
        {
            return @this.GetAllInterfaces()
                        .Where(i => @this.Name.Contains(GetInterfaceName(i)))
                        .ToArray();
        }

        private static string GetInterfaceName(Type @interface)
        {
            string name = @interface.Name;
            if (name.Length > 1 && name[0] == 'I' && char.IsUpper(name[1]))
            {
                return name.Substring(1);
            }

            return name;
        }

        /// <summary>
        ///     Searches given assembly by given <see cref="@this" /> type.
        /// </summary>
        /// <param name="this">Type to search</param>
        /// <param name="assembly">Target assembly to search</param>
        /// <returns>
        ///     Founded types list
        /// </returns>
        public static List<Type> AssignedTypesInAssembly(this Type @this, Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(x => @this.GetTypeInfo().IsAssignableFrom(x) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsSealed)
                .ToList();
        }

        /// <summary>
        ///     Prepends the item to the specified sequence.
        /// </summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="leadingItem">The leading item.</param>
        /// <returns>
        ///     The sequence with an item prepended.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">sequence</exception>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> sequence, T leadingItem)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            yield return leadingItem;

            foreach (T obj in sequence)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Pretties the print.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string PrettyPrint(this Type type)
        {
            return PrettyPrintCache.GetOrAdd(
                type,
                t =>
                {
                    try
                    {
                        return PrettyPrintRecursive(t, 0);
                    }
                    catch (Exception)
                    {
                        return t.Name;
                    }
                });
        }

        /// <summary>
        ///     Gets the cache key.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetCacheKey(this Type type)
        {
            return TypeCacheKeys.GetOrAdd(
                type,
                t => $"{t.PrettyPrint()}[hash: {t.GetHashCode()}]");
        }

        /// <summary>
        ///     Pretties the print recursive.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        private static string PrettyPrintRecursive(Type type, int depth)
        {
            if (depth > 3)
            {
                return type.Name;
            }

            string[] nameParts = type.Name.Split('`');
            if (nameParts.Length == 1)
            {
                return nameParts[0];
            }

            Type[] genericArguments = type.GetTypeInfo().GetGenericArguments();
            return !type.IsConstructedGenericType
                ? $"{nameParts[0]}<{new string(',', genericArguments.Length - 1)}>"
                : $"{nameParts[0]}<{string.Join(",", genericArguments.Select(t => PrettyPrintRecursive(t, depth + 1)))}>";
        }
    }
}
