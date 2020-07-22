// <copyright file="AtomicIntegerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class AtomicIntegerTests
    {
        private AtomicInteger _num = new AtomicInteger(0);

        [Fact]
        public void Can_add_value()
        {
            _num.Add(7).Should().Be(7);
            _num.GetValue().Should().Be(7);
        }

        [Fact]
        public void Can_be_assigned()
        {
            _num.SetValue(10);
            AtomicInteger y = _num;
            y.GetValue().Should().Be(10);
        }

        [Fact]
        public void Can_be_created_with_value()
        {
            new AtomicInteger(5).GetValue().Should().Be(5);
        }

        [Fact]
        public void Can_be_decremented()
        {
            _num.Decrement().Should().Be(-1);
            _num.GetValue().Should().Be(-1);
        }

        [Fact]
        public void Can_be_decremented_multiple_times()
        {
            _num.Decrement().Should().Be(-1);
            _num.Decrement().Should().Be(-2);
            _num.Decrement().Should().Be(-3);

            _num.GetValue().Should().Be(-3);
        }

        [Fact]
        public void Can_be_incremented()
        {
            _num.Increment().Should().Be(1);
            _num.GetValue().Should().Be(1);
        }

        [Fact]
        public void Can_be_incremented_multiple_times()
        {
            _num.Increment().Should().Be(1);
            _num.GetValue().Should().Be(1);

            _num.Increment().Should().Be(2);
            _num.GetValue().Should().Be(2);

            _num.Increment().Should().Be(3);
            _num.GetValue().Should().Be(3);
        }

        [Fact]
        public void Can_compare_and_swap()
        {
            _num.SetValue(10);

            _num.CompareAndSwap(5, 11).Should().Be(false);
            _num.GetValue().Should().Be(10);

            _num.CompareAndSwap(10, 11).Should().Be(true);
            _num.GetValue().Should().Be(11);
        }

        [Fact]
        public void Can_get_and_decrement()
        {
            _num.SetValue(10);

            _num.GetAndDecrement().Should().Be(10);
            _num.GetValue().Should().Be(9);

            _num.GetAndDecrement(5).Should().Be(9);
            _num.GetValue().Should().Be(4);
        }

        [Fact]
        public void Can_get_and_increment()
        {
            _num.SetValue(10);

            _num.GetAndIncrement().Should().Be(10);
            _num.GetValue().Should().Be(11);

            _num.GetAndIncrement(5).Should().Be(11);
            _num.GetValue().Should().Be(16);
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
        public void Can_get_or_add()
        {
            _num.SetValue(10);
            _num.GetAndAdd(5).Should().Be(10);
            _num.GetValue().Should().Be(15);
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
            _num.GetValue().Should().Be(0);
        }

#if !NET45
        [Fact]
        public void Should_have_correct_size()
        {
            AtomicInteger.SizeInBytes.Should().Be(Marshal.SizeOf<AtomicInteger>());
        }
#endif
    }
}