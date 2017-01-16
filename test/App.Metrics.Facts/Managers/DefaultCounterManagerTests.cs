// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Internal.Managers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultCounterManagerTests : IClassFixture<MetricManagerTestFixture>
    {
        private readonly MetricManagerTestFixture _fixture;
        private readonly IMeasureCounterMetrics _manager;

        public DefaultCounterManagerTests(MetricManagerTestFixture fixture)
        {
            _fixture = fixture;
            _manager = new DefaultCounterManager(_fixture.Advanced, _fixture.Registry);
        }

        [Fact]
        public void can_decrement_counter()
        {
            var metricName = "test_decrement_counter";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Count.Should().Be(-1L);
        }

        [Fact]
        public void can_decrement_counter_by_amount()
        {
            var metricName = "test_decrement_counter_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options, 2L);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Count.Should().Be(-2L);
        }

        [Fact]
        public void can_decrement_counter_item()
        {
            var metricName = "test_decrement_counter_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options, "item1");

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(-1L);
        }

        [Fact]
        public void can_decrement_counter_item_by_amount()
        {
            var metricName = "test_decrement_counter_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(options, 3L, "item1");

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(-3L);
        }

        [Fact]
        public void can_decrement_counter_metric_item()
        {
            var metricName = "test_decrement_counter_metric_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(
                options,
                item => { item.With("item4", "value4"); });

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(-1L);
        }

        [Fact]
        public void can_decrement_counter_metric_item_by_amount()
        {
            var metricName = "test_decrement_counter_metric_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Decrement(
                options,
                3L,
                item => { item.With("item3", "value3"); });

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(-3L);
        }

        [Fact]
        public void can_increment_counter()
        {
            var metricName = "test_increment_counter";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void can_increment_counter_by_amount()
        {
            var metricName = "test_increment_counter_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, 2L);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Count.Should().Be(2L);
        }

        [Fact]
        public void can_increment_counter_item()
        {
            var metricName = "test_increment_counter_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, "item1");

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(1L);
        }

        [Fact]
        public void can_increment_counter_item_by_amount()
        {
            var metricName = "test_increment_counter_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(options, 3L, "item1");

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(3L);
        }

        [Fact]
        public void can_increment_counter_metric_item()
        {
            var metricName = "test_increment_counter_metric_item";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(
                options,
                item => { item.With("item4", "value4"); });

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(1L);
        }

        [Fact]
        public void can_increment_counter_metric_item_by_amount()
        {
            var metricName = "test_increment_counter_metric_item_by_amount";
            var options = new CounterOptions() { Name = metricName };

            _manager.Increment(
                options,
                3L,
                item => { item.With("item3", "value3"); });

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().CounterValueFor(metricName).Items.First().Count.Should().Be(3L);
        }
    }
}