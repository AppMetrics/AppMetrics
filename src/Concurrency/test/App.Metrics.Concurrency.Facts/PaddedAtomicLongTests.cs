// <copyright file="PaddedAtomicLongTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class PaddedAtomicLongTests
    {
        private PaddedAtomicLong _num = new PaddedAtomicLong(0);

        [Fact]
        public void Can_add_value()
        {
            _num.Add(7L).Should().Be(7L);
            _num.GetValue().Should().Be(7L);
        }

        [Fact]
        public void Can_be_assigned()
        {
            _num.SetValue(10L);
            PaddedAtomicLong y = _num;
            y.GetValue().Should().Be(10L);
        }

        [Fact]
        public void Can_be_created_with()
        {
            new PaddedAtomicLong(5L).GetValue().Should().Be(5L);
        }

        [Fact]
        public void Can_be_decremented()
        {
            _num.Decrement().Should().Be(-1L);
            _num.GetValue().Should().Be(-1L);
        }

        [Fact]
        public void Can_be_decremented_multiple_times()
        {
            _num.Decrement().Should().Be(-1L);
            _num.Decrement().Should().Be(-2L);
            _num.Decrement().Should().Be(-3L);

            _num.GetValue().Should().Be(-3L);
        }

        [Fact]
        public void Can_be_incremented()
        {
            _num.Increment().Should().Be(1L);
            _num.GetValue().Should().Be(1L);
        }

        [Fact]
        public void Can_be_incremented_multiple_times()
        {
            _num.Increment().Should().Be(1L);
            _num.GetValue().Should().Be(1L);

            _num.Increment().Should().Be(2L);
            _num.GetValue().Should().Be(2L);

            _num.Increment().Should().Be(3L);
            _num.GetValue().Should().Be(3L);
        }

        [Fact]
        public void Can_compare_and_swap()
        {
            _num.SetValue(10L);

            _num.CompareAndSwap(5L, 11L).Should().Be(false);
            _num.GetValue().Should().Be(10L);

            _num.CompareAndSwap(10L, 11L).Should().Be(true);
            _num.GetValue().Should().Be(11L);
        }

        [Fact]
        public void Can_get_and_add()
        {
            _num.SetValue(10L);
            _num.GetAndAdd(5L).Should().Be(10L);
            _num.GetValue().Should().Be(15L);
        }

        [Fact]
        public void Can_get_and_decrement()
        {
            _num.SetValue(10L);

            _num.GetAndDecrement().Should().Be(10L);
            _num.GetValue().Should().Be(9L);

            _num.GetAndDecrement(5L).Should().Be(9L);
            _num.GetValue().Should().Be(4L);
        }

        [Fact]
        public void Can_get_and_increment()
        {
            _num.SetValue(10L);

            _num.GetAndIncrement().Should().Be(10L);
            _num.GetValue().Should().Be(11L);

            _num.GetAndIncrement(5L).Should().Be(11L);
            _num.GetValue().Should().Be(16L);
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
        public void Can_set_and_read_value()
        {
            _num.SetValue(32);
            _num.GetValue().Should().Be(32);
        }

        [Fact]
        public void Defaults_to_zero()
        {
            _num.GetValue().Should().Be(0L);
        }

#if !NET45
        [Fact]
        public void Should_have_correct_size()
        {
            PaddedAtomicLong.SizeInBytes.Should().Be(Marshal.SizeOf<PaddedAtomicLong>());
        }
#endif
    }
}