using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    /// </summary>
    public static class TypeExtensions
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
            Type[] types = @this.GetInterfaces()
                                .Where(x => @this.Name.Contains(x.Name.TrimStart('I')))
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
            return @this.GetInterfaces()
                        .Where(x => @this.Name.Contains(x.GetFriendlyNameWithoutTypeNames().TrimStart('I')))
                        .ToArray();
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
            return AssemblyScanner.FromAssembly(assembly)
                                  .IncludeNonPublicTypes()
                                  .BasedOn(@this)
                                  .Filter()
                                  .Classes()
                                  .NonStatic()
                                  .Scan();
        }

        /// <summary>
        ///     Gets the name of the friendly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetFriendlyName(this Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }
                friendlyName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                for (var i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = typeParameters[i].Name;
                    friendlyName += i == 0 ? typeParamName : "," + typeParamName;
                }

                friendlyName += ">";
            }

            return friendlyName;
        }

        /// <summary>
        ///     Gets the friendly name without type names.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetFriendlyNameWithoutTypeNames(this Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }
            }

            return friendlyName;
        }

        /// <summary>
        ///     Appends the item to the specified sequence.
        /// </summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="trailingItem">The trailing item.</param>
        /// <returns>
        ///     The sequence with an item appended to the end.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">sequence</exception>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, T trailingItem)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            foreach (T obj in sequence)
            {
                yield return obj;
            }

            yield return trailingItem;
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
        ///     Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T obj in items)
            {
                collection.Add(obj);
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

            Type[] genericArguments = type.GetGenericArguments();
            return !type.IsConstructedGenericType
                ? $"{nameParts[0]}<{new string(',', genericArguments.Length - 1)}>"
                : $"{nameParts[0]}<{string.Join(",", genericArguments.Select(t => PrettyPrintRecursive(t, depth + 1)))}>";
        }
    }
}
