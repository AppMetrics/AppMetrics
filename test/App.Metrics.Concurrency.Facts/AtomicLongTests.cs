using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class AtomicLongTests
    {
        private AtomicLong _num = new AtomicLong();

        [Fact]
        public void AtomicLong_CanAddValue()
        {
            _num.Add(7L).Should().Be(7L);
            _num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void AtomicLong_CanBeAssigned()
        {
            _num.SetValue(10L);
            AtomicLong y = _num;
            y.GetValue().Should().Be(10L);
        }

        [Fact]
        public void AtomicLong_CanBeCreatedWithValue()
        {
            new AtomicLong(5L).GetValue().Should().Be(5L);
        }

        [Fact]
        public void AtomicLong_CanBeDecremented()
        {
            _num.Decrement().Should().Be(-1L);
            _num.GetValue().Should().Be(-1L);
        }

        [Fact]
        public void AtomicLong_CanBeDecrementedMultipleTimes()
        {
            _num.Decrement().Should().Be(-1L);
            _num.Decrement().Should().Be(-2L);
            _num.Decrement().Should().Be(-3L);

            _num.GetValue().Should().Be(-3L);
        }

        [Fact]
        public void AtomicLong_CanBeIncremented()
        {
            _num.Increment().Should().Be(1L);
            _num.GetValue().Should().Be(1L);
        }

        [Fact]
        public void AtomicLong_CanBeIncrementedMultipleTimes()
        {
            _num.Increment().Should().Be(1L);
            _num.GetValue().Should().Be(1L);

            _num.Increment().Should().Be(2L);
            _num.GetValue().Should().Be(2L);

            _num.Increment().Should().Be(3L);
            _num.GetValue().Should().Be(3L);
        }

        [Fact]
        public void AtomicLong_CanCompareAndSwap()
        {
            _num.SetValue(10L);

            _num.CompareAndSwap(5L, 11L).Should().Be(false);
            _num.GetValue().Should().Be(10L);

            _num.CompareAndSwap(10L, 11L).Should().Be(true);
            _num.GetValue().Should().Be(11L);
        }

        [Fact]
        public void AtomicLong_CanGetAndAdd()
        {
            _num.SetValue(10L);
            _num.GetAndAdd(5L).Should().Be(10L);
            _num.GetValue().Should().Be(15L);
        }

        [Fact]
        public void AtomicLong_CanGetAndDecrement()
        {
            _num.SetValue(10L);

            _num.GetAndDecrement().Should().Be(10L);
            _num.GetValue().Should().Be(9L);

            _num.GetAndDecrement(5L).Should().Be(9L);
            _num.GetValue().Should().Be(4L);
        }

        [Fact]
        public void AtomicLong_CanGetAndIncrement()
        {
            _num.SetValue(10L);

            _num.GetAndIncrement().Should().Be(10L);
            _num.GetValue().Should().Be(11L);

            _num.GetAndIncrement(5L).Should().Be(11L);
            _num.GetValue().Should().Be(16L);
        }

        [Fact]
        public void AtomicLong_CanGetAndReset()
        {
            _num.SetValue(32);
            _num.GetAndReset().Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void AtomicLong_CanGetAndSet()
        {
            _num.SetValue(32);
            _num.GetAndSet(64).Should().Be(32);
            _num.GetValue().Should().Be(64);
        }

        [Fact]
        public void AtomicLong_CanSetAndReadValue()
        {
            _num.SetValue(32);
            _num.GetValue().Should().Be(32);
        }

        [Fact]
        public void AtomicLong_DefaultsToZero()
        {
            _num.GetValue().Should().Be(0L);
        }

        [Fact]
        public void AtomicLong_ShouldHaveCorrectSize()
        {
            AtomicLong.SizeInBytes.Should().Be(Marshal.SizeOf<AtomicLong>());
        }
    }
}