using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Builder;
using Autofac.Features.Scanning;

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
            foreach (PropertyInfo propertyInfo in @this.GetType().GetProperties())
            {
                yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(@this, null));
            }
        }

        /// <summary>
        ///     Finds and registers as DefaultInterfaces to container conventionally.
        /// </summary>
        /// <typeparam name="TLimit">The type of the limit.</typeparam>
        /// <param name="registration">The registration.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">registration</exception>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle>
            AsDefaultInterfacesWithSelf<TLimit>(this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            return registration.As(t => (IEnumerable<Type>)t.GetDefaultInterfacesWithSelf());
        }

        /// <summary>
        ///     Finds all types based <see cref="TLifetime" /> in given <see cref="Assembly" />
        /// </summary>
        /// <typeparam name="TLifetime">Lifetime of dependencies</typeparam>
        /// <param name="builder">Autofac's <see cref="ContainerBuilder" /></param>
        /// <param name="assembly">Assemby to search</param>
        internal static void RegisterDependenciesByAssembly<TLifetime>(this ContainerBuilder builder, Assembly assembly) where TLifetime : ILifetime
        {
            typeof(TLifetime)
                .AssignedTypesInAssembly(assembly)
                .ForEach(builder.RegisterApplyingLifetime<TLifetime>);
        }

        /// <summary>
        ///     Registers given type according to it's lifetime. Type can be generic or not.
        /// </summary>
        /// <typeparam name="TLifetime">Lifetime of dependency</typeparam>
        /// <param name="builder">Autofac's <see cref="ContainerBuilder" /></param>
        /// <param name="typeToRegister">Type to register Autofac Container</param>
        internal static void RegisterApplyingLifetime<TLifetime>(this ContainerBuilder builder, Type typeToRegister) where TLifetime : ILifetime
        {
            List<Type> defaultInterfaces = typeToRegister.GetDefaultInterfaces().ToList();

            if (typeToRegister.IsGenericTypeDefinition)
            {
                List<Type> defaultGenerics = defaultInterfaces.Where(t => t.IsGenericType).ToList();
                AddStartableIfPossible(typeToRegister, defaultGenerics);
                builder.RegisterGeneric(typeToRegister)
                       .As(defaultGenerics.ToArray())
                       .AsSelf()
                       .InjectPropertiesAsAutowired()
                       .ApplyLifeStyle(typeof(TLifetime));
            }
            else
            {
                List<Type> defaults = defaultInterfaces.Where(t => !t.IsGenericType).ToList();
                AddStartableIfPossible(typeToRegister, defaults);
                builder.RegisterType(typeToRegister)
                       .As(defaults.ToArray())
                       .AsSelf()
                       .InjectPropertiesAsAutowired()
                       .ApplyLifeStyle(typeof(TLifetime));
            }
        }

        private static void AddStartableIfPossible(Type typeToRegister, ICollection<Type> defaultInterfaces)
        {
            if (typeToRegister.IsAssignableTo<IStartable>())
            {
                defaultInterfaces.Add(typeof(IStartable));
            }
        }
    }
}
