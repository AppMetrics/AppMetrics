using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class ThreadLocalLongAdderTests
    {
        private readonly ThreadLocalLongAdder _num = new ThreadLocalLongAdder();

        [Fact]
        public void ThreadLocalLongAdder_CanAddValue()
        {
            this._num.Add(7L);
            this._num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void ThreadLocalLongAdder_CanBeCreatedWithValue()
        {
            new ThreadLocalLongAdder(5L).GetValue().Should().Be(5L);
        }

        [Fact]
        public void ThreadLocalLongAdder_CanBeDecremented()
        {
            this._num.Decrement();
            this._num.GetValue().Should().Be(-1L);
        }

        [Fact]
        public void ThreadLocalLongAdder_CanBeDecrementedMultipleTimes()
        {
            this._num.Decrement();
            this._num.Decrement();
            this._num.Decrement();

            this._num.GetValue().Should().Be(-3L);
        }

        [Fact]
        public void ThreadLocalLongAdder_CanBeIncremented()
        {
            this._num.Increment();
            this._num.GetValue().Should().Be(1L);
        }

        [Fact]
        public void ThreadLocalLongAdder_CanBeIncrementedMultipleTimes()
        {
            this._num.Increment();
            this._num.Increment();
            this._num.Increment();

            this._num.GetValue().Should().Be(3L);
        }

        [Fact]
        public void ThreadLocalLongAdder_CanGetAndReset()
        {
            this._num.Add(32);
            long val = this._num.GetAndReset();
            val.Should().Be(32);
            this._num.GetValue().Should().Be(0);
        }

        [Fact]
        public void ThreadLocalLongAdder_DefaultsToZero()
        {
            this._num.GetValue().Should().Be(0L);
        }
    }
}