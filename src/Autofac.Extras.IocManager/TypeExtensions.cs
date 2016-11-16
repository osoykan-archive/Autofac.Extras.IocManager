using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Extras.IocManager
{
    public static class TypeExtensions
    {
        public static Type[] GetDefaultInterfaceTypesWithSelf(this Type @this)
        {
            Type[] types = @this.GetInterfaces()
                                .Where(x => @this.Name.Contains(x.Name.TrimStart('I')))
                                .ToArray();
            return types.Prepend(@this).ToArray();
        }

        public static Type[] GetDefaultInterfaceTypes(this Type @this)
        {
            return @this.GetInterfaces()
                        .Where(x => @this.Name.Contains(x.Name.TrimStart('I')))
                        .ToArray();
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

            foreach (T obj in sequence)
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

            foreach (T obj in sequence)
            {
                yield return obj;
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T obj in items)
            {
                collection.Add(obj);
            }
        }
    }
}
