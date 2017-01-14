using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class ThreadLocalLongAdderTests
    {
        private readonly ThreadLocalLongAdder _num = new ThreadLocalLongAdder();

        [Fact]
        public void can_add_value()
        {
            _num.Add(7L);
            _num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void can_be_created_with() { new ThreadLocalLongAdder(5L).GetValue().Should().Be(5L); }

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
        public void can_be_incremented_and_decremented_by_value()
        {
            _num.Increment(2L);
            _num.Decrement(1L);
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
            var val = _num.GetAndReset();
            val.Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void can_get_estimated_size()
        {
            _num.Add(32);
            var result = ThreadLocalLongAdder.GetEstimatedFootprintInBytes(_num);

            result.Should().NotBe(0);
        }

        [Fact]
        public void can_get_without_volatile_read_fence_and_ordering()
        {
            _num.Add(1L);
            _num.Add(2L);
            _num.NonVolatileGetValue().Should().Be(3L);
        }

        [Fact]
        public void can_reset()
        {
            _num.Add(1L);
            _num.Add(2L);
            _num.Reset();

            _num.GetValue().Should().Be(0L);
        }

        [Fact]
        public void ThreadLocalLongAdder_DefaultsToZero() { _num.GetValue().Should().Be(0L); }
    }
}