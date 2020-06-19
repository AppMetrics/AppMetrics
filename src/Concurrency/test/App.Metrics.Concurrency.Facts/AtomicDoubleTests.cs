// <copyright file="AtomicDoubleTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class AtomicDoubleTests
    {
        private AtomicDouble _num = new AtomicDouble(0);

        [Fact]
        public void Can_add_value()
        {
            _num.Add(7.0).Should().Be(7.0);
            _num.GetValue().Should().Be(7.0);
        }

        [Fact]
        public void Can_add_value_without_lossing_precision()
        {
            _num.Add(7.00000000123).Should().Be(7.00000000123);
            _num.Add(0.12345678901).Should().Be(7.12345679024);
            _num.GetValue().Should().Be(7.12345679024);
        }

        [Fact]
        public void Can_be_assigned()
        {
            _num.SetValue(10.0);
            var y = _num;
            y.GetValue().Should().Be(10.0);
        }

        [Fact]
        public void Can_be_created_with_value()
        {
            new AtomicDouble(5.0).GetValue().Should().Be(5.0);
        }

        [Fact]
        public void Can_be_decremented()
        {
            _num.Decrement().Should().Be(-1.0);
            _num.GetValue().Should().Be(-1.0);
        }

        [Fact]
        public void Can_be_decremented_mulitple_times()
        {
            _num.Decrement().Should().Be(-1.0);
            _num.Decrement().Should().Be(-2.0);
            _num.Decrement().Should().Be(-3.0);

            _num.GetValue().Should().Be(-3.0);
        }

        [Fact]
        public void Can_be_incremented()
        {
            _num.Increment().Should().Be(1.0);
            _num.GetValue().Should().Be(1.0);
        }

        [Fact]
        public void Can_be_incremented_multiple_times()
        {
            _num.Increment().Should().Be(1.0);
            _num.GetValue().Should().Be(1.0);

            _num.Increment().Should().Be(2.0);
            _num.GetValue().Should().Be(2.0);

            _num.Increment().Should().Be(3.0);
            _num.GetValue().Should().Be(3.0);
        }

        [Fact]
        public void Can_compare_and_swap()
        {
            _num.SetValue(10.0);

            _num.CompareAndSwap(5.0, 11.0).Should().Be(false);
            _num.GetValue().Should().Be(10.0);

            _num.CompareAndSwap(10.0, 11.0).Should().Be(true);
            _num.GetValue().Should().Be(11.0);
        }

        [Fact]
        public void Can_get_and_add()
        {
            _num.SetValue(10.0);
            _num.GetAndAdd(5.0).Should().Be(10.0);
            _num.GetValue().Should().Be(15.0);
        }

        [Fact]
        public void Can_get_and_decrement()
        {
            _num.SetValue(10.0);

            _num.GetAndDecrement().Should().Be(10.0);
            _num.GetValue().Should().Be(9.0);

            _num.GetAndDecrement(5.0).Should().Be(9.0);
            _num.GetValue().Should().Be(4.0);
        }

        [Fact]
        public void Can_get_and_increment()
        {
            _num.SetValue(10.0);

            _num.GetAndIncrement().Should().Be(10.0);
            _num.GetValue().Should().Be(11.0);

            _num.GetAndIncrement(5.0).Should().Be(11.0);
            _num.GetValue().Should().Be(16.0);
        }

        [Fact]
        public void Can_get_and_reset()
        {
            _num.SetValue(32);
            _num.GetAndReset().Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void Can_get_and_set()
        {
            _num.SetValue(32);
            _num.GetAndSet(64).Should().Be(32);
            _num.GetValue().Should().Be(64);
        }

        [Fact]
        public void Can_get_without_volatile_read_fence_and_ordering()
        {
            _num.Add(1);
            var val = _num.NonVolatileGetValue();
            val.Should().Be(1);
        }

        [Fact]
        public void Can_set_and_read_value()
        {
            _num.SetValue(32);
            _num.GetValue().Should().Be(32);
        }

        [Fact]
        public void Defaults_to_zero()
        {
            _num.GetValue().Should().Be(0.0);
        }
    }
}