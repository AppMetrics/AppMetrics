// <copyright file="AtomicLongArrayTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class AtomicLongArrayTests
    {
        [Fact]
        public void Can_compare_and_set()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 10);

            array.CompareAndSwap(1, 5, 11).Should().Be(false);
            array.GetValue(1).Should().Be(10);

            array.CompareAndSwap(1, 10, 11).Should().Be(true);
            array.GetValue(1).Should().Be(11);
        }

        [Fact]
        public void Can_create_array()
        {
            var array = new AtomicLongArray(10);
            array.Length.Should().Be(10);
        }

        [Fact]
        public void Can_decrement()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 10);

            array.Decrement(1).Should().Be(9);
            array.Decrement(1, 4).Should().Be(5);

            array.GetAndDecrement(1).Should().Be(5);
            array.GetValue(1).Should().Be(4);
        }

        [Fact]
        public void Can_estimate_size()
        {
            var list = new ReadOnlyCollection<long>(new List<long> { 1, 2, 3 });
            var array = new AtomicLongArray(list);

            AtomicLongArray.GetEstimatedFootprintInBytes(array).Should().NotBe(0);
        }

        [Fact]
        public void Can_get_and_add()
        {
            var array = new AtomicLongArray(1);
            array.Add(0, 1);
            var value = array.GetAndAdd(0, 2);

            value.Should().Be(1);
            array.GetValue(0).Should().Be(3);
        }

        [Fact]
        public void Can_get_and_decrement()
        {
            var array = new AtomicLongArray(1);
            array.Add(0, 10);
            var value = array.GetAndDecrement(0, 2);

            value.Should().Be(10);
            array.GetValue(0).Should().Be(8);
        }

        [Fact]
        public void Can_get_and_increment()
        {
            var array = new AtomicLongArray(1);
            array.Add(0, 10);
            var value = array.GetAndIncrement(0, 2);

            value.Should().Be(10);
            array.GetValue(0).Should().Be(12);
        }

        [Fact]
        public void Can_get_and_set_value()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 3);
            array.GetValue(1).Should().Be(3);

            array.GetAndSet(1, 4).Should().Be(3);
            array.GetValue(1).Should().Be(4);

            array.GetAndReset(1).Should().Be(4);
            array.GetValue(1).Should().Be(0);
        }

        [Fact]
        public void Can_increment()
        {
            var array = new AtomicLongArray(10);

            array.SetValue(1, 3);

            array.Increment(1).Should().Be(4);
            array.Increment(1, 4).Should().Be(8);

            array.GetAndIncrement(1).Should().Be(8);
            array.GetValue(1).Should().Be(9);
        }

        [Fact]
        public void Can_instiate_from_list()
        {
            var list = new ReadOnlyCollection<long>(new List<long> { 1, 2, 3 });
            var array = new AtomicLongArray(list);

            array.Length.Should().Be(3);
            array.GetValue(0).Should().Be(1);
            array.GetValue(1).Should().Be(2);
            array.GetValue(2).Should().Be(3);
        }

        [Fact]
        public void Negetive_length_throws()
        {
            Action setupAction = () =>
            {
                var unused = new AtomicLongArray(-1);
            };

            setupAction.Should().Throw<ArgumentException>();
        }
    }
}