using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class PaddedAtomicLongTests
    {
        private PaddedAtomicLong _num = new PaddedAtomicLong();

        [Fact]
        public void PaddedAtomicLong_CanAddValue()
        {
            _num.Add(7L).Should().Be(7L);
            _num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void PaddedAtomicLong_CanBeAssigned()
        {
            _num.SetValue(10L);
            PaddedAtomicLong y = _num;
            y.GetValue().Should().Be(10L);
        }

        [Fact]
        public void PaddedAtomicLong_CanBeCreatedWithValue()
        {
            new PaddedAtomicLong(5L).GetValue().Should().Be(5L);
        }

        [Fact]
        public void PaddedAtomicLong_CanBeDecremented()
        {
            _num.Decrement().Should().Be(-1L);
            _num.GetValue().Should().Be(-1L);
        }

        [Fact]
        public void PaddedAtomicLong_CanBeDecrementedMultipleTimes()
        {
            _num.Decrement().Should().Be(-1L);
            _num.Decrement().Should().Be(-2L);
            _num.Decrement().Should().Be(-3L);

            _num.GetValue().Should().Be(-3L);
        }

        [Fact]
        public void PaddedAtomicLong_CanBeIncremented()
        {
            _num.Increment().Should().Be(1L);
            _num.GetValue().Should().Be(1L);
        }

        [Fact]
        public void PaddedAtomicLong_CanBeIncrementedMultipleTimes()
        {
            _num.Increment().Should().Be(1L);
            _num.GetValue().Should().Be(1L);

            _num.Increment().Should().Be(2L);
            _num.GetValue().Should().Be(2L);

            _num.Increment().Should().Be(3L);
            _num.GetValue().Should().Be(3L);
        }

        [Fact]
        public void PaddedAtomicLong_CanCompareAndSwap()
        {
            _num.SetValue(10L);

            _num.CompareAndSwap(5L, 11L).Should().Be(false);
            _num.GetValue().Should().Be(10L);

            _num.CompareAndSwap(10L, 11L).Should().Be(true);
            _num.GetValue().Should().Be(11L);
        }

        [Fact]
        public void PaddedAtomicLong_CanGetAndAdd()
        {
            _num.SetValue(10L);
            _num.GetAndAdd(5L).Should().Be(10L);
            _num.GetValue().Should().Be(15L);
        }

        [Fact]
        public void PaddedAtomicLong_CanGetAndDecrement()
        {
            _num.SetValue(10L);

            _num.GetAndDecrement().Should().Be(10L);
            _num.GetValue().Should().Be(9L);

            _num.GetAndDecrement(5L).Should().Be(9L);
            _num.GetValue().Should().Be(4L);
        }

        [Fact]
        public void PaddedAtomicLong_CanGetAndIncrement()
        {
            _num.SetValue(10L);

            _num.GetAndIncrement().Should().Be(10L);
            _num.GetValue().Should().Be(11L);

            _num.GetAndIncrement(5L).Should().Be(11L);
            _num.GetValue().Should().Be(16L);
        }

        [Fact]
        public void PaddedAtomicLong_CanGetAndReset()
        {
            _num.SetValue(32);
            _num.GetAndReset().Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void PaddedAtomicLong_CanGetAndSet()
        {
            _num.SetValue(32);
            _num.GetAndSet(64).Should().Be(32);
            _num.GetValue().Should().Be(64);
        }

        [Fact]
        public void PaddedAtomicLong_CanSetAndReadValue()
        {
            _num.SetValue(32);
            _num.GetValue().Should().Be(32);
        }

        [Fact]
        public void PaddedAtomicLong_DefaultsToZero()
        {
            _num.GetValue().Should().Be(0L);
        }

        [Fact]
        public void PaddedAtomicLong_ShouldHaveCorrectSize()
        {
            PaddedAtomicLong.SizeInBytes.Should().Be(Marshal.SizeOf<PaddedAtomicLong>());
        }
    }
}