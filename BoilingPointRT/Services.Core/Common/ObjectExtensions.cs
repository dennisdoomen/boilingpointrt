using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BoilingPointRT.Services.Domain;

namespace BoilingPointRT.Services.Common
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///   Wraps an existing object into a collection.
        /// </summary>
        public static IEnumerable<T> ToEnumerable<T>(this T t)
        {
            return Equals(t, default(T)) ? new T[0] : new[] { t };
        }

        /// <summary>
        ///   Wraps an existing object into a collection.
        /// </summary>
        public static IQueryable<T> ToQueryable<T>(this T t)
        {
            return t.ToEnumerable().AsQueryable();
        }

        /// <summary>
        /// Returns the value of the specified string property if <paramref name="owner"/> is not <c>null</c>, 
        /// and an empty string otherwise.
        /// </summary>
        public static string EmptyOr<T>(this T owner, Expression<Func<T, string>> propertySelector) where T : class
        {
            return (owner != null) ? propertySelector.Compile()(owner) : String.Empty;
        }

        /// <summary>
        /// Returns the value of the specified property if <paramref name="owner"/> is not <c>null</c>, 
        /// and a default value otherwise.
        /// </summary>
        public static TProperty DefaultOr<TOwner, TProperty>(this TOwner owner,
            Expression<Func<TOwner, TProperty>> propertySelector)
            where TOwner : class
        {
            return (owner != null) ? propertySelector.Compile()(owner) : default(TProperty);
        }

        /// <summary>
        /// Determines whether the current value matches any of the provided elements.
        /// </summary>
        public static bool IsOneOf<T>(this T value, params T[] values)
        {
            return values.Contains(value);
        }

        /// <summary>
        /// Generates a deterministic <see cref="Guid"/> based on a string.
        /// </summary>
        public static Guid ToDeterministicGuid(this object value, Type ownerType)
        {
            Guid result;

            object keyValue = IsAggregateRootKey(value) ? GetValueFromAggregateRootKey(value) : value;

            if (keyValue is Guid)
            {
                result = (Guid)keyValue;
            }
            else if (keyValue is GuidKey)
            {
                result = ((GuidKey)keyValue).Key;
            }
            else if (keyValue is string)
            {
                result = GuidUtility.Create(
                    GuidUtility.VisionSuiteNamespace,
                    string.Format("{0}_{1}", ownerType.FullName, keyValue));
            }
            else
            {
                throw new ArgumentException("Cannot generate a deterministic Guid from a " + keyValue.GetType().Name);
            }

            return result;
        }

        private static bool IsAggregateRootKey(object value)
        {
            return (value is IAggregateRootKey<Guid>) || (value is IAggregateRootKey<string>);
        }

        private static object GetValueFromAggregateRootKey(object value)
        {
            var guidAggregateRootKey = value as IAggregateRootKey<Guid>;
            if (guidAggregateRootKey != null)
            {
                return guidAggregateRootKey.Key;
            }

            var stringAggregateRootKey = value as IAggregateRootKey<string>;
            if (stringAggregateRootKey != null)
            {
                return stringAggregateRootKey.Key;
            }

            return value;
        }
    }
}