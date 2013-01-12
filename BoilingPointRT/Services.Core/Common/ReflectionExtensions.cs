using System;
using System.Collections.Generic;
using System.Reflection;

namespace BoilingPointRT.Services.Common
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Retrieves the property value(s) from the specified property of the specified type.
        /// </summary>
        public static IEnumerable<T> GetPropertyValues<T>(this object obj, string propertyName)
        {
            if (obj.HasProperty<T>(propertyName))
            {
                return obj.GetPropertyValue<T>(propertyName).ToEnumerable();
            }

            if (obj.HasProperty<IEnumerable<T>>(propertyName))
            {
                return obj.GetPropertyValue<IEnumerable<T>>(propertyName);
            }

            throw new InvalidOperationException(string.Format(
                "'{0}' does not contain a property that matches specific property '{1}' of type '{2}' or 'System.IEnumerable<{2}>'.",
                obj.GetType().Name, propertyName, typeof(T).FullName));
        }

        /// <summary>
        /// Retrieves the property value(s) from the specified property of the specified type.
        /// </summary>
        public static IEnumerable<T> FindPropertyValues<T>(this object obj, string propertyName)
        {
            if (obj.HasProperty<T>(propertyName))
            {
                return obj.GetPropertyValue<T>(propertyName).ToEnumerable();
            }

            if (obj.HasProperty<IEnumerable<T>>(propertyName))
            {
                return obj.GetPropertyValue<IEnumerable<T>>(propertyName);
            }

            return null;
        }

        private static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            return (T)propertyInfo.GetValue(obj, null);
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (!obj.HasProperty<object>(propertyName))
            {
                throw new InvalidOperationException(string.Format(
                    "'{0}' does not contain a property that matches specific property '{1}'.",
                    obj.GetType().Name, propertyName));
            }

            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo.GetValue(obj, null);
        }

        private static bool HasProperty<T>(this object obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return false;
            }

            if (!typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
            {
                return false;
            }

            return true;
        }
    }
}