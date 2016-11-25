using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class StripedLongAdderTests
    {
        private readonly StripedLongAdder _num = new StripedLongAdder();

        [Fact]
        public void StripedLongAdder_CanAddValue()
        {
            _num.Add(7L);
            _num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void StripedLongAdder_CanBeCreatedWithValue()
        {
            new StripedLongAdder(5L).GetValue().Should().Be(5L);
        }

        [Fact]
        public void StripedLongAdder_CanBeDecremented()
        {
            _num.Decrement();
            _num.GetValue().Should().Be(-1L);
        }

        [Fact]
        public void StripedLongAdder_CanBeDecrementedMultipleTimes()
        {
            _num.Decrement();
            _num.Decrement();
            _num.Decrement();

            _num.GetValue().Should().Be(-3L);
        }

        [Fact]
        public void StripedLongAdder_CanBeIncremented()
        {
            _num.Increment();
            _num.GetValue().Should().Be(1L);
        }

        [Fact]
        public void StripedLongAdder_CanBeIncrementedMultipleTimes()
        {
            _num.Increment();
            _num.Increment();
            _num.Increment();

            _num.GetValue().Should().Be(3L);
        }

        [Fact]
        public void StripedLongAdder_CanGetAndReset()
        {
            _num.Add(32);
            long val = _num.GetAndReset();
            val.Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void StripedLongAdder_DefaultsToZero()
        {
            _num.GetValue().Should().Be(0L);
        }
    }
}