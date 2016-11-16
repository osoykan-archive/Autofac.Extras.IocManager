using System.Reflection;

using Autofac.Core;
using System.Linq;

namespace Autofac.Extras.IocManager
{
    public class DoNotInjectAttributePropertySelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.GetCustomAttributes<DoNotInjectAttribute>().Any();
        }
    }
}