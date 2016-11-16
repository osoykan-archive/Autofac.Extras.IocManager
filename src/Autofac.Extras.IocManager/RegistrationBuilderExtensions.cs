﻿using System;
using System.Reflection;

using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Extras.IocManager
{
    public static class RegistrationBuilderExtensions
    {
        /// <summary>
        ///     This extension allows circular dependencies and applies <see cref="DoNotInjectAttribute" />
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            InjectPropertiesAsAutowired<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
            return registration.OnActivated(args => InjectProperties(args.Context, args.Instance, true, new DoNotInjectAttributePropertySelector()));
        }

        private static void InjectProperties(IComponentContext context, object instance, bool overrideSetValues, IPropertySelector propertySelector)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (propertySelector.InjectProperty(propertyInfo, instance))
                {
                    continue;
                }

                Type propertyType = propertyInfo.PropertyType;

                if ((!propertyType.IsValueType || propertyType.IsEnum) && (propertyInfo.GetIndexParameters().Length == 0) && context.IsRegistered(propertyType))
                {
                    MethodInfo[] accessors = propertyInfo.GetAccessors(true);

                    if (((accessors.Length != 1) || !(accessors[0].ReturnType != typeof(void)))
                        && (overrideSetValues || (accessors.Length != 2) || (propertyInfo.GetValue(instance, null) == null)))
                    {
                        object obj = context.Resolve(propertyType);
                        propertyInfo.SetValue(instance, obj, null);
                    }
                }
            }
        }
    }
}