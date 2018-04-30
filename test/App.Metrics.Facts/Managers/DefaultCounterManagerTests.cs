// <copyright file="DefaultCounterManagerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Counter;
using App.Metrics.Facts.Fixtures;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultCounterManagerTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly string _context;
        private readonly MetricCoreTestFixture _fixture;
        private readonly IMeasureCounterMetrics _manager;

        public DefaultCounterManagerTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _manager = _fixture.Managers.Counter;
            _context = _fixture.Context;
        }

        [Fact]
        public void Can_decrement_counter()
        {
            var metricName = "test_decrement_counter";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(options);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-1L);
        }

        [Fact]
        public void Can_decrement_counter_by_amount()
        {
            var metricName = "test_decrement_counter_by_amount";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(options, 2L);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-2L);
        }

        [Fact]
        public void Can_decrement_counter_item()
        {
            var metricName = "test_decrement_counter_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-1L);
        }

        [Fact]
        public void Can_decrement_counter_item_by_amount()
        {
            var metricName = "test_decrement_counter_item_by_amount";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(options, 3L, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-3L);
        }

        [Fact]
        public void Can_decrement_counter_metric_item()
        {
            var metricName = "test_decrement_counter_metric_item";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(
                options,
                new MetricSetItem("item4", "value4"));

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-1L);
        }

        [Fact]
        public void Can_decrement_counter_metric_item_by_amount()
        {
            var metricName = "test_decrement_counter_metric_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(
                options,
                3L,
                new MetricSetItem("item3", "value3"));

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-3L);
        }

        [Fact]
        public void Can_decrement_multidimensional_counter()
        {
            var metricName = "test_decrement_counter_multi";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(options, _fixture.Tags[0]);
            _manager.Decrement(options, _fixture.Tags[1]);

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(-1L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(-1L);
        }

        [Fact]
        public void Can_decrement_multidimensional_counter_by_amount()
        {
            var metricName = "test_decrement_counter_by_amount_multi";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(options, _fixture.Tags[0], 2L);
            _manager.Decrement(options, _fixture.Tags[1], 3L);

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(-2L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(-3L);
        }

        [Fact]
        public void Can_decrement_multidimensional_counter_item()
        {
            var metricName = "test_decrement_counter_item_multi";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(options, _fixture.Tags[0], "item1");
            _manager.Decrement(options, _fixture.Tags[1], "item1");

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(-1L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items[0].Item.Should().Be("item1");
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(-1L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items[0].Item.Should().Be("item1");
        }

        [Fact]
        public void Can_decrement_multidimensional_counter_item_by_amount()
        {
            var metricName = "test_decrement_counter_item_by_amount_multi";
            var options = new CounterOptions { Name = metricName };

            _manager.Decrement(options, _fixture.Tags[0], 3L, "item1");
            _manager.Decrement(options, _fixture.Tags[1], 5L, "item1");

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(-3L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(-5L);
        }

        [Fact]
        public void Can_increment_counter()
        {
            var metricName = "test_increment_counter";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void Can_increment_counter_by_amount()
        {
            var metricName = "test_increment_counter_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, 2L);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(2L);
        }

        [Fact]
        public void Can_increment_counter_item()
        {
            var metricName = "test_increment_counter_item";
            var options = new CounterOptions { Name = metricName };

            _manager.Increment(options, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void Can_increment_counter_item_by_amount()
        {
            var metricName = "test_increment_counter_item_by_amount";
            var options = new CounterOptions { Name = metricName };

            _manager.Increment(options, 3L, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(3L);
        }

        [Fact]
        public void Can_increment_counter_metric_item()
        {
            var metricName = "test_increment_counter_metric_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(
                options,
                new MetricSetItem("item4", "value4"));

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void Can_increment_counter_metric_item_by_amount()
        {
            var metricName = "test_increment_counter_metric_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(
                options,
                3L,
                new MetricSetItem("item3", "value3"));

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(3L);
        }

        [Fact]
        public void Can_increment_multidimensional_counter()
        {
            var metricName = "test_increment_counter_multi";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, _fixture.Tags[0]);
            _manager.Increment(options, _fixture.Tags[1]);

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(1L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(1L);
        }

        [Fact]
        public void Can_increment_multidimensional_counter_by_amount()
        {
            var metricName = "test_increment_counter_by_amount_multi";
            var options = new CounterOptions { Name = metricName };

            _manager.Increment(options, _fixture.Tags[0], 2L);
            _manager.Increment(options, _fixture.Tags[1], 3L);

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(2L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(3L);
        }

        [Fact]
        public void Can_increment_multidimensional_counter_item()
        {
            var metricName = "test_increment_counter_item_multi";
            var options = new CounterOptions { Name = metricName };

            _manager.Increment(options, _fixture.Tags[0], "item1");
            _manager.Increment(options, _fixture.Tags[1], "item1");

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(1L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items[0].Item.Should().Be("item1");
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(1L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items[0].Item.Should().Be("item1");
        }

        [Fact]
        public void Can_increment_multidimensional_counter_item_by_amount()
        {
            var metricName = "test_increment_counter_item_by_amount_multi";
            var options = new CounterOptions { Name = metricName };

            _manager.Increment(options, _fixture.Tags[0], 3L, "item1");
            _manager.Increment(options, _fixture.Tags[1], 5L, "item1");

            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(3L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items[0].Item.Should().Be("item1");
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(5L);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetCounterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items[0].Item.Should().Be("item1");
        }
    }
}