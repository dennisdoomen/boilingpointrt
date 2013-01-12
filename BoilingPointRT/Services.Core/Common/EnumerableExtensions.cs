using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BoilingPointRT.Services.Common
{
    public static class EnumerableExtensions
    {
        private const int FirstPageNumber = 1;

        /// <summary>
        /// Adds the specified value to the end of the IEnumerable until it reaches the specified length.
        /// </summary>
        /// <remarks>If the IEnumerable is larger or equal in length, a new IEnumerable is returned containing the same values.</remarks>
        public static IEnumerable<T> PadRight<T>(this IEnumerable<T> list, int length, T value)
        {
            var newList = new List<T>(list);

            while (newList.Count < length)
            {
                newList.Add(value);
            }

            return newList;
        }

        public static Type GetEnumeratedType<T>(this IEnumerable<T> list)
        {
            Type[] enumeratedTypes = list.GetType().GetInterfaces()
                .Where(IsGenericEnumerable)
                .Select(x => x.GetGenericArguments().Single()).ToArray();

            if (enumeratedTypes.Count() > 1)
            {
                throw new ArgumentException("list implements more than one IEnumerable<>");
            }

            return enumeratedTypes.Single();
        }

        private static bool IsGenericEnumerable(Type interfaceType)
        {
            return interfaceType.IsGenericType && (interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static IQueryable<TSource> TakePage<TSource>(this IQueryable<TSource> source, int pageNumber, int pageSize)
        {
            ValidatePageNumber(pageNumber);

            return source.Skip((pageNumber - FirstPageNumber) * pageSize).Take(pageSize);
        }

        public static IEnumerable<TSource> TakePage<TSource>(this IEnumerable<TSource> source, int pageNumber, int pageSize)
        {
            ValidatePageNumber(pageNumber);

            return source.Skip((pageNumber - FirstPageNumber) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Determines whether two collections are equivalent.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if both collections contain the exact same items regardless of the order; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEquivalentTo<T>(this IEnumerable<T> source, IEnumerable<T> itemsToCompareWith)
        {
            source = source.Distinct();
            itemsToCompareWith = itemsToCompareWith.Distinct();

            return (source.Intersect(itemsToCompareWith).Count() == source.Count()) &&
                (source.Count() == itemsToCompareWith.Count());
        }

        private static void ValidatePageNumber(int pageNumber)
        {
            if (pageNumber < FirstPageNumber)
            {
                throw new ArgumentException("Page numbers should start at " + FirstPageNumber, "pageNumber");
            }
        }

        /// <summary>
        /// Executes the action to each element in the list.
        /// </summary>
        /// <typeparam name="T">The enumerable item's type.</typeparam>
        /// <param name="enumerable">The elements to enumerate.</param>
        /// <param name="action">The action to execute to each item in the list.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// Returns true if the collection doesn't contain any element, otherwise false.
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        /// <summary>
        /// Concats an item into a sequence.
        /// </summary>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item)
        {
            return source.Concat(new[] { item });
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the default equality comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return DistinctByImpl(source, keySelector, comparer);
        }

        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the specified selector function.
        /// </summary>
        public static IEnumerable<TSource> Except<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second,
            Func<TSource, TKey> selector)
        {
            return first.Except(second, new LambdaComparer<TSource, TKey>(selector));
        }

        /// <summary>
        /// Verifies if any of the elements is part of the collection.
        /// </summary>
        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            return items.Any(source.Contains);
        }

        /// <summary>
        /// Verifies that all elements are part of a collection.
        /// </summary>
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            return items.All(source.Contains);
        }

        /// <summary>
        /// Verifies if the collection contains the specified <paramref name="items"/> in the exact same sequence.
        /// </summary>
        public static bool ContainsSequence<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            var sequence = items.ToArray();
            int count = sequence.Count();

            while (source.Any())
            {
                if (source.Take(count).SequenceEqual(sequence))
                {
                    return true;
                }

                source = source.Skip(1);
            }

            return false;
        }
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        /// <param name="source">The IEnumerable to check for emptiness.</param>
        /// <returns><c>true</c> if the source sequence contains any elements; otherwise, <c>false</c></returns>
        public static bool Any(this IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// Splits a collection in batches of the specified size.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> InBatchesOf<T>(this IEnumerable<T> items, int batchSize)
        {
            var batch = new List<T>(batchSize);

            foreach (var item in items)
            {
                batch.Add(item);

                if (batch.Count >= batchSize)
                {
                    yield return batch;
                    batch = new List<T>();
                }
            }

            if (batch.Count != 0)
            {
                //can't be batch size or would've yielded above 
                batch.TrimExcess();
                yield return batch;
            }
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> collection) where T : ICloneable<T>
        {
            return collection.Select(item => item.Clone()).ToArray();
        }

        /// <summary>
        /// Returns a sequence with a single element with its default value when source is null or an empty sequence; 
        /// othwise it will return the current enumeration.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        public static IEnumerable<TSource> NullElementWhenEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null || !source.Any())
            {
                var item = default(TSource);
                return new[] { item };
            }

            return source;
        }
    }
}