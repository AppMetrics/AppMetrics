using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class AtomicLongArrayTests
    {
        [Fact]
        public void AtomicLongArray_CanCompareAndSet()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 10);

            array.CompareAndSwap(1, 5, 11).Should().Be(false);
            array.GetValue(1).Should().Be(10);

            array.CompareAndSwap(1, 10, 11).Should().Be(true);
            array.GetValue(1).Should().Be(11);
        }

        [Fact]
        public void AtomicLongArray_CanCreateArray()
        {
            var array = new AtomicLongArray(10);
            array.Length.Should().Be(10);
        }

        [Fact]
        public void AtomicLongArray_CanDecrement()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 10);

            array.Decrement(1).Should().Be(9);
            array.Decrement(1, 4).Should().Be(5);

            array.GetAndDecrement(1).Should().Be(5);
            array.GetValue(1).Should().Be(4);
        }

        [Fact]
        public void AtomicLongArray_CanGetAndSetValue()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 3);
            array.GetValue(1).Should().Be(3);

            array.GetAndSet(1, 4).Should().Be(3);
            array.GetValue(1).Should().Be(4);

            array.GetAndReset(1).Should().Be(4);
            array.GetValue(1).Should().Be(0);
        }

        [Fact]
        public void AtomicLongArray_CanIncrement()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 3);

            array.Increment(1).Should().Be(4);
            array.Increment(1, 4).Should().Be(8);

            array.GetAndIncrement(1).Should().Be(8);
            array.GetValue(1).Should().Be(9);
        }
    }
}