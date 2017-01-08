using System;
using System.Globalization;

namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Extension methods for all objects.
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        ///     Used to simplify and beautify casting an object to a type.
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        ///     Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)" /> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
