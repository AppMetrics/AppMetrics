using System;
using System.Linq;
using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class UserValueWrapperEqualityTests
    {
        [Theory]
        [InlineData(1L, "userValue", true)]
        [InlineData(2L, "userValue", false)]
        [InlineData(1L, "userValue1", false)]
        public void equality_with_equals(long value, string userValue, bool expected)
        {
            var userValueWrapper = new UserValueWrapper(1L, "userValue");
            var other = new UserValueWrapper(value, userValue);

            userValueWrapper.Equals(other).Should().Be(expected);
        }

        [Fact]
        public void equality_with_equals_false_when_same_object()
        {
            var userValueWrapper = new UserValueWrapper(1L, "userValue");

            object other = userValueWrapper;

            userValueWrapper.Equals(other).Should().Be(true);
        }

        [Theory]
        [InlineData(1L, "userValue", true)]
        [InlineData(2L, "userValue", false)]
        [InlineData(1L, "userValue1", false)]
        public void equality_with_equals_operator(long value, string userValue, bool expected)
        {
            var userValueWrapper = new UserValueWrapper(1L, "userValue");
            var other = new UserValueWrapper(value, userValue);

            (userValueWrapper == other).Should().Be(expected);
        }

        [Theory]
        [InlineData(1L, "userValue", true)]
        [InlineData(2L, "userValue", false)]
        [InlineData(1L, "userValue1", false)]
        public void equality_with_not_equals_operator(long value, string userValue, bool expected)
        {
            var userValueWrapper = new UserValueWrapper(1L, "userValue");
            var other = new UserValueWrapper(value, userValue);

            (userValueWrapper != other).Should().Be(!expected);
        }

        [Fact]
        public void array_using_comparer_should_sort_values_correct()
        {
            var count = 50;
            var array = new UserValueWrapper[count];
            var rnd = new Random();
            foreach (var i in Enumerable.Range(0, count))
            {
                array[i] = new UserValueWrapper(rnd.Next(1, 100), "userValue");
            }

            Array.Sort(array, UserValueWrapper.Comparer);

            array.ToList().Select(x => x.Value).Should().BeInAscendingOrder();
        }
    }
}