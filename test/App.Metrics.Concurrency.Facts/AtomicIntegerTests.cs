using System;
using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class AtomicIntegerArrayTests
    {
        [Fact]
        public void throws_when_lenght_smaller_than_zero()
        {
            Action action = () =>
            {
                var atomicIntArray = new AtomicIntArray(-2);
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void can_create_from_readonly_list()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            list.Length.Should().Be(array.Length);
        }

        [Fact]
        public void can_get_estimated_bytes()
        {
            var list = new[] { 1, 2, 3 };
            var expected = list.Length * sizeof(int) + 16 + IntPtr.Size;
            var array = new AtomicIntArray(list);

            var estimate = AtomicIntArray.GetEstimatedFootprintInBytes(array);

            estimate.Should().Be(expected);
        }

        [Fact]
        public void can_add_value_to_item()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            array.Add(0, 4);

            array.GetValue(0).Should().Be(5);
        }

        [Fact]
        public void can_decrement_item()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            array.Decrement(0);

            array.GetValue(0).Should().Be(0);
        }

        [Fact]
        public void can_increment_item()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            array.Increment(0);

            array.GetValue(0).Should().Be(2);
        }
    }

    public class AtomicIntegerTests
    {
        private AtomicInteger _num = new AtomicInteger();

        [Fact]
        public void can_add_value()
        {
            _num.Add(7).Should().Be(7);
            _num.GetValue().Should().Be(7);
        }

        [Fact]
        public void can_be_assigned()
        {
            _num.SetValue(10);
            AtomicInteger y = _num;
            y.GetValue().Should().Be(10);
        }

        [Fact]
        public void can_be_created_with_value()
        {
            new AtomicInteger(5).GetValue().Should().Be(5);
        }

        [Fact]
        public void can_be_decremented()
        {
            _num.Decrement().Should().Be(-1);
            _num.GetValue().Should().Be(-1);
        }

        [Fact]
        public void can_be_decremented_multiple_times()
        {
            _num.Decrement().Should().Be(-1);
            _num.Decrement().Should().Be(-2);
            _num.Decrement().Should().Be(-3);

            _num.GetValue().Should().Be(-3);
        }

        [Fact]
        public void can_be_incremented()
        {
            _num.Increment().Should().Be(1);
            _num.GetValue().Should().Be(1);
        }

        [Fact]
        public void can_be_incremented_multiple_times()
        {
            _num.Increment().Should().Be(1);
            _num.GetValue().Should().Be(1);

            _num.Increment().Should().Be(2);
            _num.GetValue().Should().Be(2);

            _num.Increment().Should().Be(3);
            _num.GetValue().Should().Be(3);
        }

        [Fact]
        public void can_compare_and_swap()
        {
            _num.SetValue(10);

            _num.CompareAndSwap(5, 11).Should().Be(false);
            _num.GetValue().Should().Be(10);

            _num.CompareAndSwap(10, 11).Should().Be(true);
            _num.GetValue().Should().Be(11);
        }

        [Fact]
        public void can_get_and_decrement()
        {
            _num.SetValue(10);

            _num.GetAndDecrement().Should().Be(10);
            _num.GetValue().Should().Be(9);

            _num.GetAndDecrement(5).Should().Be(9);
            _num.GetValue().Should().Be(4);
        }

        [Fact]
        public void can_get_and_increment()
        {
            _num.SetValue(10);

            _num.GetAndIncrement().Should().Be(10);
            _num.GetValue().Should().Be(11);

            _num.GetAndIncrement(5).Should().Be(11);
            _num.GetValue().Should().Be(16);
        }

        [Fact]
        public void can_get_and_reset()
        {
            _num.SetValue(32);
            _num.GetAndReset().Should().Be(32);
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void can_get_and_set()
        {
            _num.SetValue(32);
            _num.GetAndSet(64).Should().Be(32);
            _num.GetValue().Should().Be(64);
        }

        [Fact]
        public void can_get_or_add()
        {
            _num.SetValue(10);
            _num.GetAndAdd(5).Should().Be(10);
            _num.GetValue().Should().Be(15);
        }

        [Fact]
        public void can_set_and_read_value()
        {
            _num.SetValue(32);
            _num.GetValue().Should().Be(32);
        }

        [Fact]
        public void defaults_to_zero()
        {
            _num.GetValue().Should().Be(0);
        }

        [Fact]
        public void should_have_correct_size()
        {
            AtomicInteger.SizeInBytes.Should().Be(Marshal.SizeOf<AtomicInteger>());
        }
    }
}