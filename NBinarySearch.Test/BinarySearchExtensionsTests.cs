using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace NBinarySearch.Test
{
    class BinarySearchExtensionsTests
    {
        static readonly int[] ZeroToNinetyNine = Enumerable.Range(0, 100).ToArray();
        static readonly int[] OddNumbersToOneHundred = Enumerable.Range(1, 100).Where(i => i % 2 != 0).ToArray();
        static readonly string[] DaysOf2012AsStrings = DateRange(new DateTime(2012, 1, 1), 366).Select(d => d.ToString("MMM dd, yyyy")).ToArray();
        static readonly string[] StringsInAscendingOrderOfLength = new[] { "dog", "puma", "horse", "chicken", "kangaroo" };
        static readonly string[] StringsInOrderOfSecondLetter = new[] { "bat", "ABS", "ack", "ODD", "leg", "OFT", "egg" };

        static IEnumerable<DateTime> DateRange(DateTime startDate, int dayCount)
        {
            for (int i = 0; i < dayCount; ++i)
            {
                yield return startDate.AddDays((double)i);
            }
        }

        class FormattedDateComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return DateTime.Parse(x).CompareTo(DateTime.Parse(y));
            }
        }

        [TestFixture]
        public class GeneralBinarySearch
        {
            [Test]
            public void ReturnsIndexIfValuePresent()
            {
                Assert.That(ZeroToNinetyNine.BinarySearch(20), Is.EqualTo(20));
            }
            
            [Test]
            public void ReturnsComplementOfLowerIndexIfValueNotPresent()
            {
                Assert.That(OddNumbersToOneHundred.BinarySearch(10), Is.EqualTo(~5));
            }

            [Test]
            public void ReturnsNegativeOneIfValueLessThanMin()
            {
                Assert.That(OddNumbersToOneHundred.BinarySearch(0), Is.EqualTo(~0));
            }

            [Test]
            public void ReturnsComplementOfCountIfValueGreaterThanMax()
            {
                Assert.That(ZeroToNinetyNine.BinarySearch(100), Is.EqualTo(~100));
            }
        }

        [TestFixture]
        public class UsingCustomComparers
        {
            [Test]
            public void ReturnsIndexBasedOnGivenComparer()
            {
                Assert.That(DaysOf2012AsStrings.BinarySearch("Jan 15, 2012", new FormattedDateComparer()), Is.EqualTo(14));
            }
        }

        [TestFixture]
        public class UsingKeySelectors
        {
            [Test]
            public void ReturnsIndexOfKeyBasedOnGivenKeySelector()
            {
                Assert.That(StringsInAscendingOrderOfLength.BinarySearchByKey(5, s => s.Length), Is.EqualTo(2));
            }

            [Test]
            public void ReturnsIndexOfValueBasedOnGivenKeySelector()
            {
                Assert.That(StringsInAscendingOrderOfLength.BinarySearchByValue("chicken", s => s.Length), Is.EqualTo(3));
            }
        }

        [TestFixture]
        public class UsingKeySelectorsAndCustomComparers
        {
            [Test]
            public void ReturnsIndexBasedOnGivenKeySelectorAndComparer()
            {
                Assert.That(StringsInOrderOfSecondLetter.BinarySearchByValue("oft", s => s.Substring(1), StringComparer.InvariantCultureIgnoreCase), Is.EqualTo(5));
            }
        }
    }
}
