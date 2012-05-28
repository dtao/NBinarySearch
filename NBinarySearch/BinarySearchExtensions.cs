using System;
using System.Collections.Generic;

namespace NBinarySearch
{
    public static class BinarySearchExtensions
    {
        public static int BinarySearch<T>(this IList<T> collection, int index, int length, T value, IComparer<T> comparer = null)
        {
            ValidateArguments<T>(collection, index, length);

            comparer = comparer ?? Comparer<T>.Default;

            return BinarySearchImpl(collection, index, length, value, comparer.Compare);
        }

        public static int BinarySearch<T>(this IList<T> collection, T value, IComparer<T> comparer = null)
        {
            return collection.BinarySearch(0, collection.Count, value, comparer);
        }

        public static int BinarySearchByKey<T, TKey>(this IList<T> collection, int index, int length, TKey key, Func<T, TKey> keySelector, IComparer<TKey> keyComparer = null)
        {
            ValidateArguments(collection, index, length, keySelector);

            keyComparer = keyComparer ?? Comparer<TKey>.Default;

            return BinarySearchImpl(collection, index, length, key, (x, y) => keyComparer.Compare(keySelector(x), y));
        }

        public static int BinarySearchByKey<T, TKey>(this IList<T> collection, TKey key, Func<T, TKey> keySelector, IComparer<TKey> keyComparer = null)
        {
            return collection.BinarySearchByKey(0, collection.Count, key, keySelector, keyComparer);
        }

        public static int BinarySearchByValue<T, TKey>(this IList<T> collection, int index, int length, T value, Func<T, TKey> keySelector, IComparer<TKey> keyComparer = null)
        {
            ValidateArguments(collection, index, length, keySelector);

            keyComparer = keyComparer ?? Comparer<TKey>.Default;

            return BinarySearchImpl(collection, index, length, value, (x, y) => keyComparer.Compare(keySelector(x), keySelector(y)));
        }

        public static int BinarySearchByValue<T, TKey>(this IList<T> collection, T value, Func<T, TKey> keySelector, IComparer<TKey> keyComparer = null)
        {
            return collection.BinarySearchByValue(0, collection.Count, value, keySelector, keyComparer);
        }

        static int BinarySearchImpl<Tx, Ty>(IList<Tx> collection, int index, int length, Ty value, Func<Tx, Ty, int> comparison)
        {
            int lower = index;
            int upper = (index + length) - 1;
            while (lower <= upper)
            {
                int adjustedIndex = lower + ((upper - lower) >> 1);

                int comparisonResult = comparison(collection[adjustedIndex], value);

                if (comparisonResult == 0)
                {
                    return adjustedIndex;
                }
                else if (comparisonResult < 0)
                {
                    lower = adjustedIndex + 1;
                }
                else
                {
                    upper = adjustedIndex - 1;
                }
            }

            return ~lower;
        }

        static void ValidateArguments<T, TKey>(IList<T> collection, int index, int length, Func<T, TKey> selector)
        {
            ValidateArguments(collection, index, length);

            if (selector == null)
            {
                throw new ArgumentNullException("selector", "Selector cannot be null.");
            }
        }

        static void ValidateArguments<T>(IList<T> collection, int index, int length)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection", "Collection cannot be null.");
            }
            else if (index < 0 || index >= collection.Count)
            {
                throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the collection.");
            }
            else if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length must be equal to or greater than zero.");
            }
            else if (collection.Count - index < length)
            {
                throw new ArgumentException();
            }
        }
    }
}
