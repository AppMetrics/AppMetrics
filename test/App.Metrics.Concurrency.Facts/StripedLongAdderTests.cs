using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class StripedLongAdderTests
    {
        private readonly StripedLongAdder _num = new StripedLongAdder();

        [Fact]
        public void can_add_value()
        {
            _num.Add(7L);
            _num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void can_be_created_with_value()
        {
            new StripedLongAdder(5L).GetValue().Should().Be(5L);
        }

        [Fact]
        public void can_be_decremented()
        {
            _num.Decrement();
            _num.GetValue().Should().Be(-1L);
        }

        [Fact]
        public void can_be_decremented_multiple_times()
        {
            _num.Decrement();
            _num.Decrement();
            _num.Decrement();

            _num.GetValue().Should().Be(-3L);
        }

        [Fact]
        public void can_be_incremented()
        {
            _num.Increment();
            _num.GetValue().Should().Be(1L);
        }

        [Fact]
        public void can_be_incremented_multiple_times()
        {
            _num.Increment();
            _num.Increment();
            _num.Increment();

            _num.GetValue().Should().Be(3L);
        }

        [Fact]
        public void can_get_and_reset()
        {
            _num.Add(32);
            long val = _num.GetAndReset();
            val.Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void defaults_to_zero()
        {
            _num.GetValue().Should().Be(0L);
        }
    }
}