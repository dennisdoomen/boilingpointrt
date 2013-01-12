using System;
using System.Linq;
using System.Reflection;

namespace BoilingPointRT.Services.Common
{
    public static class TypeExtensions
    {
        public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return (type.GetCustomAttributes(typeof (TAttribute), true).Length > 0);
        }

        public static TAttribute FindAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return (TAttribute)type.GetCustomAttributes(typeof (TAttribute), true).SingleOrDefault();
        }

        /// <summary>
        /// Finds the attributes that implement or subclass <typeparam name="TAttribute"/>
        /// </summary>
        public static TAttribute[] FindAttributes<TAttribute>(this Type type)
        {
            return type.GetCustomAttributes(true).OfType<TAttribute>().ToArray();
        }

        public static bool HasAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        {
            return (memberInfo.GetCustomAttributes(typeof(TAttribute), true).Length > 0);
        }

        /// <summary>
        /// Finds properties of the specified type, and returns their values.
        /// </summary>
        /// <typeparam name="TPropertyType">The type of the property.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="target">The target.</param>
        /// <returns>
        /// The values of the matching properties, or a an empty collection.
        /// </returns>
        public static TPropertyType[] FindPropertyValues<TPropertyType>(this Type type, object target)
        {
            return type.GetProperties()
                .Where(p => p.PropertyType == typeof(TPropertyType))
                .Select(p => (TPropertyType)p.GetValue(target, null))
                .ToArray();
        }

        public static bool Implements<TInterface>(this Type type)
        {
            return Implements(type, typeof(TInterface));
        }

        public static bool Implements(this Type type, Type expectedBaseType)
        {
            return expectedBaseType.IsAssignableFrom(type) && (type != expectedBaseType);
        }

        /// <summary>
        /// Finds the most specific interface on a object that inherits <typeparamref name="TInterface"/>.
        /// </summary>
        public static Type FindMostSpecificInterface<TInterface>(this Type policyType)
        {
            var interfaces =
                from @interface in policyType.GetInterfaces()
                where @interface.Implements<TInterface>()
                orderby @interface.GetInterfaces().Length descending
                select @interface;

            return interfaces.FirstOrDefault();
        }

        /// <summary>
        /// Alternative version of <see cref="Type.IsSubclassOf"/> that supports raw generic types (generic types without
        /// any type parameters).
        /// </summary>
        /// <param name="baseType">The base type class for which the check is made.</param>
        /// <param name="toCheck">To type to determine for whether it derives from <paramref name="baseType"/>.</param>
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type baseType)
        {
            if (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (baseType == cur || baseType.IsAssignableFrom(cur))
                {
                    return true;
                }

                if (toCheck.BaseType.IsSubclassOfRawGeneric(baseType))
                {
                    return true;
                }

                foreach (var @interface in toCheck.GetInterfaces())
                {
                    if (@interface.IsSubclassOfRawGeneric(baseType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
