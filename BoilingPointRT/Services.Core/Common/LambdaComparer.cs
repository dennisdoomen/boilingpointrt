using System;
using System.Collections.Generic;

namespace BoilingPointRT.Services.Common
{
    /// <summary>
    /// Generic implementation of <see cref="IEqualityComparer{T}"/> that uses a lambda expression to refer to the property to use for
    /// comparing equality.
    /// </summary>
    public class LambdaComparer<T, TKey> : IEqualityComparer<T>
    {
        private readonly Func<T, TKey> selector;

        public LambdaComparer(Func<T, TKey> selector)
        {
            this.selector = selector;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(T x, T y)
        {
            return selector(x).Equals(selector(y));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
        public int GetHashCode(T obj)
        {
            return selector(obj).GetHashCode();
        }
    }
}