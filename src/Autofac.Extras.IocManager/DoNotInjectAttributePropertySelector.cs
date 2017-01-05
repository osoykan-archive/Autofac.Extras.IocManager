using System.Linq;
using System.Reflection;

using Autofac.Core;

namespace Autofac.Extras.IocManager
{
    public class DoNotInjectAttributePropertySelector : IPropertySelector
    {
        /// <summary>
        ///     Provides filtering to determine if property should be injected
        /// </summary>
        /// <param name="propertyInfo">Property to be injected</param>
        /// <param name="instance">Instance that has the property to be injected</param>
        /// <returns>
        ///     Whether property should be injected
        /// </returns>
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            if (propertyInfo.GetCustomAttributes<DoNotInjectAttribute>().Any())
            {
                return false;
            }

            return true;
        }
    }
}
