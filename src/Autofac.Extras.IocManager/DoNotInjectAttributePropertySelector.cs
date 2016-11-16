using System.Linq;
using System.Reflection;

using Autofac.Core;

namespace Autofac.Extras.IocManager
{
    public class DoNotInjectAttributePropertySelector : IPropertySelector
    {
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
