// <copyright file="CounterValueTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Counter
{
    public class CounterValueTests
    {
        private MetricTags Tags => new MetricTags(new[] { "host", "env" }, new[] { "server1", "staging" });

        [Fact]
        public void Can_determine_if_are_same()
        {
            var first = new CounterValue(10L);
            var second = new CounterValue(10L);

            first.Should().Be(second);
        }

        [Fact]
        public void Can_determine_if_diff()
        {
            var first = new CounterValue(10L);
            var second = new CounterValue(1L);

            first.Should().NotBe(second);
        }

        [Fact]
        public void Can_determine_if_diff_using_operator()
        {
            var first = new CounterValue(10L);
            var second = new CounterValue(1L);

            Assert.True(first != second);
        }

        [Fact]
        public void Can_determine_if_multidimensional()
        {
            var valueSource = new GaugeValueSource("test_gauge|host:server1,env:staging", ConstantValue.Provider(0.5), Unit.Calls, Tags);

            valueSource.IsMultidimensional.Should().BeTrue();
        }

        [Fact]
        public void Can_determine_if_same_using_operator()
        {
            var first = new CounterValue(10L);
            var second = new CounterValue(10L);

            Assert.True(first == second);
        }

        [Fact]
        public void Can_get_multidimensional_metric_name()
        {
            var valueSource = new GaugeValueSource("test_gauge|host:server1,env:staging", ConstantValue.Provider(0.5), Unit.Calls, Tags);

            valueSource.MultidimensionalName.Should().Be("test_gauge");
        }

        [Fact]
        public void Counter_value_with_null_set_items_throws()
        {
            Action setupAction = () =>
            {
                var unused = new CounterValue(10L, null);
            };

            setupAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Hash_codes_differ_between_instances()
        {
            var first = new CounterValue(10L).GetHashCode();
            var second = new CounterValue(1L).GetHashCode();

            Assert.NotEqual(first, second);
        }

        [Fact]
        public void Hash_codes_same_for_same_instance()
        {
            var first = new CounterValue(1);
            var second = first;

            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void Reference_equality_should_be_correct()
        {
            var first = new CounterValue(10L);
            var second = new CounterValue(1L);

            Assert.False(first.Equals((object)second));
        }
    }
}