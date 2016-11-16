using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager.InterfaceMarking;

using FluentAssemblyScanner;

namespace Autofac.Extras.IocManager.Extensions
{
    public static class RegistrarExtensions
    {
        public static void RegisterAssemblyByConvention(this ContainerBuilder builder, Assembly assembly)
        {
            RegisterSingletonDependencies(builder, assembly);
            RegisterTransientDependencies(builder, assembly);
            RegisterLifeTimeScopeDependencies(builder, assembly);
        }

        private static void RegisterSingletonDependencies(ContainerBuilder builder, Assembly assembly)
        {
            List<Type> singletonTypes = AssemblyScanner.FromAssembly(assembly)
                                                       .IncludeNonPublicTypes()
                                                       .BasedOn<ISingletonDependency>()
                                                       .Filter()
                                                       .Classes()
                                                       .NonStatic()
                                                       .Scan();

            singletonTypes.ForEach(type =>
            {
                if (type.IsGenericTypeDefinition)
                {
                    builder.RegisterGeneric(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().SingleInstance();
                }
                else
                {
                    builder.RegisterType(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().SingleInstance();
                }
            });
        }

        private static void RegisterTransientDependencies(ContainerBuilder builder, Assembly assembly)
        {
            List<Type> transientTypes = AssemblyScanner.FromAssembly(assembly)
                                                       .IncludeNonPublicTypes()
                                                       .BasedOn<ITransientDependency>()
                                                       .Filter()
                                                       .Classes()
                                                       .NonStatic()
                                                       .Scan();

            transientTypes.ForEach(type =>
            {
                if (type.IsGenericTypeDefinition)
                {
                    builder.RegisterGeneric(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().InstancePerDependency();
                }
                else
                {
                    builder.RegisterType(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().InstancePerDependency();
                }
            });
        }

        private static void RegisterLifeTimeScopeDependencies(ContainerBuilder builder, Assembly assembly)
        {
            List<Type> lifeTimeScopeTypes = AssemblyScanner.FromAssembly(assembly)
                                                           .IncludeNonPublicTypes()
                                                           .BasedOn<ILifeTimeScopeDependency>()
                                                           .Filter()
                                                           .Classes()
                                                           .NonStatic()
                                                           .Scan();

            lifeTimeScopeTypes.ForEach(type =>
            {
                if (type.IsGenericTypeDefinition)
                {
                    builder.RegisterGeneric(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().InstancePerLifetimeScope();
                }
                else
                {
                    builder.RegisterType(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().InstancePerLifetimeScope();
                }
            });
        }

        private static void RegisterPerRequestDependencies(ContainerBuilder builder, Assembly assembly)
        {
            List<Type> perRequestTypes = AssemblyScanner.FromAssembly(assembly)
                                                        .IncludeNonPublicTypes()
                                                        .BasedOn<IPerRequestDependency>()
                                                        .Filter()
                                                        .Classes()
                                                        .NonStatic()
                                                        .Scan();

            perRequestTypes.ForEach(type =>
            {
                if (type.IsGenericTypeDefinition)
                {
                    builder.RegisterGeneric(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().InstancePerRequest();
                }
                else
                {
                    builder.RegisterType(type).As(type.GetDefaultInterfaceTypes()).AsSelf().InjectPropertiesAsAutowired().InstancePerRequest();
                }
            });
        }
    }
}
