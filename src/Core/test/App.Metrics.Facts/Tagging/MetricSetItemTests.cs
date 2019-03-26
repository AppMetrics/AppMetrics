// <copyright file="MetricSetItemTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Tagging
{
    public class MetricSetItemTests
    {
        [Fact]
        public void Can_tostring_set_item_with_multiple_key_values()
        {
            var keys = new[] { "key1", "key2", "key3", "key4", "key5", "key6", "key7", "key8" };
            var values = new[] { "value1", "value2", "value3", "value4", "value5", "value6", "value7", "value8" };

            var setItem = new MetricSetItem(keys, values);

            setItem.ToString().Should().Be("key1:value1|key2:value2|key3:value3|key4:value4|key5:value5|key6:value6|key7:value7|key8:value8");
        }

        [Fact]
        public void Can_tostring_set_item_with_single_key_value()
        {
            var setItem = new MetricSetItem("key", "value");

            setItem.ToString().Should().Be("key:value");
        }

        [Fact]
        public void Can_tostring_set_item_with_single_key_value_array()
        {
            var keys = new[] { "key1" };
            var values = new[] { "machine-1" };

            var setItem = new MetricSetItem(keys, values);

            setItem.ToString().Should().Be("key1:machine-1");
        }

        [Fact]
        public void Can_tostring_set_item_with_single_value()
        {
            var setItem = new MetricSetItem("key", "value");

            setItem.ToString().Should().Be("key:value");
        }

        [Fact]
        public void Can_tostring_set_item_with_zero_count_should_be_null()
        {
            var keys = new string[0];
            var values = new string[0];

            var setItem = new MetricSetItem(keys, values);

            setItem.ToString().Should().BeNull();
        }

        [Fact]
        public void Count_should_be_one_when_single_key_value()
        {
            var setItem = new MetricSetItem("key", "value");

            var count = setItem.Count;

            count.Should().Be(1);
        }

        [Fact]
        public void Count_should_total_key_values()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var setItem = new MetricSetItem(keys, values);

            var count = setItem.Count;

            count.Should().Be(2);
        }

        [Fact]
        public void Keys_and_values_be_same_length()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricSetItem(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Keys_cannot_be_null()
        {
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricSetItem(null, values);
            };

            setup.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Keys_cannot_contain_empty_strings_or_whitespace(string key)
        {
            var keys = new[] { key };
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricSetItem(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Keys_cannot_contain_nulls()
        {
            var keys = new[] { null, "key2" };
            var values = new[] { "machine-1", "machine-2" };

            Action setup = () =>
            {
                var unused = new MetricSetItem(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Set_items_with_different_counts_should_not_be_equal()
        {
            var keysLeft = new[] { "key1", "key2" };
            var keysRight = new[] { "key1", "key2", "key3" };
            var valuesLeft = new[] { "machine-1", "machine-2" };
            var valuesRight = new[] { "machine-1", "machine-2", "machine-3" };

            var left = new MetricSetItem(keysLeft, valuesLeft);
            var right = new MetricSetItem(keysRight, valuesRight);

            left.Equals(right).Should().Be(false);
        }

        [Fact]
        public void Set_items_with_different_keys_should_not_be_equal()
        {
            var keysLeft = new[] { "key1", "key2" };
            var keysRight = new[] { "key1", "key3" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricSetItem(keysLeft, values);
            var right = new MetricSetItem(keysRight, values);

            left.Equals(right).Should().Be(false);
        }

        [Fact]
        public void Set_items_with_different_keys_should_not_be_equal_with_operator()
        {
            var keysLeft = new[] { "key1", "key2" };
            var keysRight = new[] { "key1", "key3" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricSetItem(keysLeft, values);
            var right = new MetricSetItem(keysRight, values);

            Assert.True(left != right);
        }

        [Fact]
        public void Set_items_with_different_values_should_not_be_equal()
        {
            var keys = new[] { "key1", "key2" };
            var valuesLeft = new[] { "machine-1", "machine-2" };
            var valuesRight = new[] { "machine-1", "machine-3" };

            var left = new MetricSetItem(keys, valuesLeft);
            var right = new MetricSetItem(keys, valuesRight);

            left.Equals(right).Should().Be(false);
        }

        [Fact]
        public void Set_items_with_same_key_values_should_be_equal()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricSetItem(keys, values);
            var right = new MetricSetItem(keys, values);

            left.Equals(right).Should().Be(true);
        }

        [Fact]
        public void Set_items_with_same_key_values_should_be_equal_with_operator()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricSetItem(keys, values);
            var right = new MetricSetItem(keys, values);

            Assert.True(left == right);
        }

        [Fact]
        public void Values_cannot_be_null()
        {
            var keys = new[] { "key1", "key2" };

            Action setup = () =>
            {
                var unused = new MetricSetItem(keys, null);
            };

            setup.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Values_cannot_contain_empty_strings_or_whitespace(string value)
        {
            var keys = new[] { "key1" };
            var values = new[] { value };

            Action setup = () =>
            {
                var unused = new MetricSetItem(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Values_cannot_contain_nulls()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", null };

            Action setup = () =>
            {
                var unused = new MetricSetItem(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }
    }
}