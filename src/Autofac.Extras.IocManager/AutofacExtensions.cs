using System.Collections.Generic;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
	public static class AutofacExtensions
	{
		/// <summary>
		///     Helper for anonymouse resolvings <see cref="IocManager.Resolve{T}(object)" />
		/// </summary>
		/// <param name="this">The this.</param>
		/// <returns></returns>
		internal static IEnumerable<TypedParameter> GetTypedResolvingParameters(this object @this)
		{
			foreach (PropertyInfo propertyInfo in @this.GetType().GetTypeInfo().GetProperties())
			{
				yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(@this, null));
			}
		}
	}
}
