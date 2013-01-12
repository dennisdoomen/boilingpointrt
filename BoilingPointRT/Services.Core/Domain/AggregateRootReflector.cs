using System;
using System.Linq;
using System.Reflection;

using BoilingPointRT.Services.Common;

namespace BoilingPointRT.Services.Domain
{
    public static class AggregateRootReflector
    {
        /// <summary>
        /// Gets the single property on the specified <paramref name="aggregateRootType"/> that is decorated
        /// with the <see cref="KeyAttribute"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Will be thrown when the specified <paramref name="aggregateRootType"/> does not implement the
        /// <see cref="IAggregateRoot"/> interface, or when it does not have a single property decorated with
        /// the <see cref=" KeyAttribute"/>.
        /// </exception>
        public static string GetKeyPropertyName(Type aggregateRootType)
        {
            if (!aggregateRootType.Implements<IAggregateRoot>())
            {
                throw new InvalidOperationException(string.Format(
                    "Type {0} is not an aggregate root", aggregateRootType.Name));
            }

            var properties = aggregateRootType
                .GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance)
                .Where(pi => pi.GetCustomAttributes(typeof(IdentityAttribute), true).Any()).ToArray();

            if (properties.Count() != 1)
            {
                throw new InvalidOperationException(string.Format(
                    "Aggregate root {0} must have exactly one property that is marked with [Identity] attribute",
                    aggregateRootType.Name));
            }

            return properties.Single().Name;
        }

        /// <summary>
        /// Gets the functional key of an <see cref="IAggregateRoot"/> from the property that is decorated with the
        /// <see cref="KeyAttribute"/>.
        /// </summary>
        public static object GetKey(IAggregateRoot aggregateRoot)
        {
            Type type = aggregateRoot.GetType();
            string keyPropertyName = GetKeyPropertyName(type);
            return type.GetProperty(keyPropertyName).GetValue(aggregateRoot, null);
        }
    }
}