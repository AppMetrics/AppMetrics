// <copyright file="StripedLongAdderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class StripedLongAdderTests
    {
        private readonly StripedLongAdder _num = new StripedLongAdder();

        [Fact]
        public void Can_add_value()
        {
            _num.Add(7L);
            _num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void Can_be_created_with_value() { new StripedLongAdder(5L).GetValue().Should().Be(5L); }

        [Fact]
        public void Can_be_decremented()
        {
            _num.Decrement();
            _num.GetValue().Should().Be(-1L);
        }

        [Fact]
        public void Can_be_decremented_by_value()
        {
            _num.Add(7L);
            _num.Decrement(2L);
            _num.GetValue().Should().Be(5L);
        }

        [Fact]
        public void Can_be_decremented_multiple_times()
        {
            _num.Decrement();
            _num.Decrement();
            _num.Decrement();

            _num.GetValue().Should().Be(-3L);
        }

        [Fact]
        public void Can_be_incremented()
        {
            _num.Increment();
            _num.GetValue().Should().Be(1L);
        }

        [Fact]
        public void Can_be_incremented_by_value()
        {
            _num.Increment(2L);
            _num.GetValue().Should().Be(2L);
        }

        [Fact]
        public void Can_be_incremented_multiple_times()
        {
            _num.Increment();
            _num.Increment();
            _num.Increment();

            _num.GetValue().Should().Be(3L);
        }

        [Fact]
        public void Can_get_and_reset()
        {
            _num.Add(32);
            var val = _num.GetAndReset();
            val.Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void Can_get_estimated_size()
        {
            _num.Add(7L);
            StripedLongAdder.GetEstimatedFootprintInBytes(_num).Should().NotBe(0);
        }

        [Fact]
        public void Can_get_without_volatile_read_fence_and_ordering()
        {
            Parallel.For(
                1,
                1000,
                l =>
                {
                    _num.Add(l);
                    var val = _num.NonVolatileGetValue();
                    val.Should().BeGreaterOrEqualTo(l);
                });
        }

        [Fact]
        public void Can_reset()
        {
            Parallel.For(
                1,
                1000,
                l =>
                {
                    _num.Add(l);

                    if (l % 2 == 0)
                    {
                        _num.Reset();
                    }
                });

            _num.GetValue().Should().BeLessThan(499500);
        }

        [Fact]
        public void Defaults_to_zero() { _num.GetValue().Should().Be(0L); }
    }
}