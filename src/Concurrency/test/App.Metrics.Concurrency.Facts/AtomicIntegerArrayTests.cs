// <copyright file="AtomicIntegerArrayTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class AtomicIntegerArrayTests
    {
        [Fact]
        public void Can_add_value_to_item()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            array.Add(0, 4);

            array.GetValue(0).Should().Be(5);
        }

        [Fact]
        public void Can_create_from_readonly_list()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            list.Length.Should().Be(array.Length);
        }

        [Fact]
        public void Can_decrement()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            array.Decrement(0);

            array.GetValue(0).Should().Be(0);
        }

        [Fact]
        public void Can_decrement_by_value()
        {
            var array = new AtomicIntArray(1);

            array.Increment(0, 10);
            array.Decrement(0, 2);

            array.GetValue(0).Should().Be(8);
        }

        [Fact]
        public void Can_get_and_add()
        {
            var array = new AtomicIntArray(1);

            var value = array.GetAndAdd(0, 2);

            value.Should().Be(0);
            array.GetValue(0).Should().Be(2);
        }

        [Fact]
        public void Can_get_and_decrement()
        {
            var array = new AtomicIntArray(1);

            var value = array.GetAndDecrement(0);

            value.Should().Be(0);
            array.GetValue(0).Should().Be(-1);
        }

        [Fact]
        public void Can_get_and_decrement_by_value()
        {
            var array = new AtomicIntArray(1);

            var value = array.GetAndDecrement(0, 2);

            value.Should().Be(0);
            array.GetValue(0).Should().Be(-2);
        }

        [Fact]
        public void Can_get_and_increment()
        {
            var array = new AtomicIntArray(1);

            var value = array.GetAndIncrement(0);

            value.Should().Be(0);
            array.GetValue(0).Should().Be(1);
        }

        [Fact]
        public void Can_get_and_increment_by_value()
        {
            var array = new AtomicIntArray(1);

            var value = array.GetAndIncrement(0, 2);

            value.Should().Be(0);
            array.GetValue(0).Should().Be(2);
        }

        [Fact]
        public void Can_get_and_reset()
        {
            var array = new AtomicIntArray(1);
            array.Increment(0);

            var value = array.GetAndReset(0);

            value.Should().Be(1);
            array.Length.Should().Be(1);
            array.GetValue(0).Should().Be(0);
        }

        [Fact]
        public void Can_get_estimated_bytes()
        {
            var list = new[] { 1, 2, 3 };
            var expected = (list.Length * sizeof(int)) + 16 + IntPtr.Size;
            var array = new AtomicIntArray(list);

            var estimate = AtomicIntArray.GetEstimatedFootprintInBytes(array);

            estimate.Should().Be(expected);
        }

        [Fact]
        public void Can_get_without_volatile_read_fence_and_ordering()
        {
            var array = new AtomicIntArray(1);

            array.Add(0, 1);
            var val = array.NonVolatileGetValue(0);
            val.Should().Be(1);
        }

        [Fact]
        public void Can_increment_value()
        {
            var list = new[] { 1, 2, 3 };
            var array = new AtomicIntArray(list);

            array.Increment(0);

            array.GetValue(0).Should().Be(2);
        }

        [Fact]
        public void Can_set_value()
        {
            var array = new AtomicIntArray(1);

            array.SetValue(0, 1);
            array.GetValue(0).Should().Be(1);
        }

        [Fact]
        public void Can_set_without_volatile_read_fence_and_ordering()
        {
            var array = new AtomicIntArray(1);

            array.NonVolatileSetValue(0, 1);
            array.GetValue(0).Should().Be(1);
        }

        [Fact]
        public void Throws_when_lenght_smaller_than_zero()
        {
            Action action = () =>
            {
                var unused = new AtomicIntArray(-2);
            };

            action.Should().Throw<ArgumentException>();
        }
    }
}