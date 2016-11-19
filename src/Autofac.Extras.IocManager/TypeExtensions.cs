using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner;

namespace Autofac.Extras.IocManager
{
    public static class TypeExtensions
    {
        /// <summary>
        ///     Finds default interfaces a givent type and given type adds itsel in returned list.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Default interface types with adding given type itself</returns>
        public static Type[] GetDefaultInterfacesWithSelf(this Type @this)
        {
            var types = @this.GetInterfaces()
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
        ///  </code>
        /// </summary>
        /// <param name="this">Type to search</param>
        /// <returns>Default interface types</returns>
        public static Type[] GetDefaultInterfaces(this Type @this)
        {
            return @this.GetInterfaces()
                        .Where(x => @this.Name.Contains(x.Name.TrimStart('I')))
                        .ToArray();
        }

        /// <summary>
        ///     Searches given assembly by given <see cref="@this" /> type.
        /// </summary>
        /// <param name="this">Type to search</param>
        /// <param name="assembly">Target assembly to search</param>
        /// <returns>Founded types list</returns>
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

        /// <summary>Appends the item to the specified sequence.</summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="trailingItem">The trailing item.</param>
        /// <returns>The sequence with an item appended to the end.</returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, T trailingItem)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            foreach (var obj in sequence)
            {
                yield return obj;
            }

            yield return trailingItem;
        }

        /// <summary>Prepends the item to the specified sequence.</summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="leadingItem">The leading item.</param>
        /// <returns>The sequence with an item prepended.</returns>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> sequence, T leadingItem)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            yield return leadingItem;

            foreach (var obj in sequence)
            {
                yield return obj;
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var obj in items)
            {
                collection.Add(obj);
            }
        }
    }
}
