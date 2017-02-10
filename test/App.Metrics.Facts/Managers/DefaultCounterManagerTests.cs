// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Options;
using App.Metrics.Counter;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Tagging;
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
        public void can_decrement_counter()
        {
            var metricName = "test_decrement_counter";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-1L);
        }

        [Fact]
        public void can_decrement_counter_by_amount()
        {
            var metricName = "test_decrement_counter_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options, 2L);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-2L);
        }

        [Fact]
        public void can_decrement_counter_item()
        {
            var metricName = "test_decrement_counter_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-1L);
        }

        [Fact]
        public void can_decrement_counter_item_by_amount()
        {
            var metricName = "test_decrement_counter_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options, 3L, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-3L);
        }

        [Fact]
        public void can_decrement_counter_metric_item()
        {
            var metricName = "test_decrement_counter_metric_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(
                options,
                new MetricSetItem("item4", "value4"));

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(-1L);
        }

        [Fact]
        public void can_decrement_counter_metric_item_by_amount()
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
        public void can_increment_counter()
        {
            var metricName = "test_increment_counter";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void can_increment_counter_by_amount()
        {
            var metricName = "test_increment_counter_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, 2L);

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(2L);
        }

        [Fact]
        public void can_increment_counter_item()
        {
            var metricName = "test_increment_counter_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void can_increment_counter_item_by_amount()
        {
            var metricName = "test_increment_counter_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, 3L, "item1");

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(3L);
        }

        [Fact]
        public void can_increment_counter_metric_item()
        {
            var metricName = "test_increment_counter_metric_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(
                options,
                new MetricSetItem("item4", "value4"));

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void can_increment_counter_metric_item_by_amount()
        {
            var metricName = "test_increment_counter_metric_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(
                options,
                3L,
                new MetricSetItem("item3", "value3"));

            _fixture.Snapshot.GetCounterValue(_context, metricName).Count.Should().Be(3L);
        }
    }
}