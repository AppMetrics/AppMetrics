// <copyright file="DefaultMeterManagerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Internal.NoOp;
using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultMeterManagerTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly string _context;
        private readonly MetricCoreTestFixture _fixture;
        private readonly IMeasureMeterMetrics _manager;

        public DefaultMeterManagerTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _manager = _fixture.Managers.Meter;
            _context = _fixture.Context;
        }

        [Fact]
        public void Can_mark()
        {
            var metricName = "test_mark_meter";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options);

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().Meters.Count(x => x.Name == metricName).Should().Be(1);
        }

        [Fact]
        public void Can_mark_by_amount()
        {
            var metricName = "test_mark_meter_by_amount";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, 2L);

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().Meters.Count(x => x.Name == metricName).Should().Be(1);
        }

        [Fact]
        public void Can_mark_multidimensional()
        {
            var metricName = "test_mark_meter_multi";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, _fixture.Tags[0]);
            _manager.Mark(options, _fixture.Tags[1]);

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().Meters.Count(x => x.Name == _fixture.Tags[0].AsMetricName(metricName)).Should().Be(1);
            data.Contexts.Single().Meters.Count(x => x.Name == _fixture.Tags[1].AsMetricName(metricName)).Should().Be(1);
        }

        [Fact]
        public void Can_mark_multidimensional_by_amount()
        {
            var metricName = "test_mark_meter_by_amount_multi";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, _fixture.Tags[0], 2L);
            _manager.Mark(options, _fixture.Tags[1], 5L);

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().Meters.Count(x => x.Name == _fixture.Tags[0].AsMetricName(metricName)).Should().Be(1);
            data.Contexts.Single().Meters.First(x => x.Name == _fixture.Tags[0].AsMetricName(metricName)).Value.Count.Should().Be(2);
            data.Contexts.Single().Meters.Count(x => x.Name == _fixture.Tags[1].AsMetricName(metricName)).Should().Be(1);
            data.Contexts.Single().Meters.First(x => x.Name == _fixture.Tags[1].AsMetricName(metricName)).Value.Count.Should().Be(5);
        }

        [Fact]
        public void Can_mark_multidimensional_with_item()
        {
            var metricName = "test_mark_meter_with_item_multi";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, _fixture.Tags[0], "item1");
            _manager.Mark(options, _fixture.Tags[1], "item1");

            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items.Length.Should().Be(1);
        }

        [Fact]
        public void Can_mark_multidimensional_with_item_by_amount()
        {
            var metricName = "test_mark_meter_with_item_by_amount_multi";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, _fixture.Tags[0], 5L, "item1");
            _manager.Mark(options, _fixture.Tags[1], 500L, "item1");

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items.Length.Should().Be(1);
            data.Contexts.Single().Meters.First(x => x.Name == _fixture.Tags[0].AsMetricName(metricName)).Value.Count.Should().Be(5);
            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items.Length.Should().Be(1);
            data.Contexts.Single().Meters.First(x => x.Name == _fixture.Tags[1].AsMetricName(metricName)).Value.Count.Should().Be(500);
        }

        [Fact]
        public void Can_mark_when_multidimensional_with_metric_item()
        {
            var metricName = "test_mark_meter_with_metric_item_multi";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, _fixture.Tags[0], new MetricSetItem("tagKey", "tagvalue"));
            _manager.Mark(options, _fixture.Tags[1], new MetricSetItem("tagKey", "tagvalue"));

            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items.Length.Should().Be(1);
        }

        [Fact]
        public void Can_mark_when_multidimensional_with_metric_item_by_amount()
        {
            var metricName = "test_mark_meter_with_metric_item_by_amount_multi";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, _fixture.Tags[0], 5L, new MetricSetItem("tagKey", "tagvalue"));
            _manager.Mark(options, _fixture.Tags[1], 20L, new MetricSetItem("tagKey", "tagvalue"));

            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(5);
            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Items.Length.Should().Be(1);
            _fixture.Snapshot.GetMeterValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(20);
        }

        [Fact]
        public void Can_mark_with_item()
        {
            var metricName = "test_mark_meter_with_item";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, "item1");

            _fixture.Snapshot.GetMeterValue(_context, metricName).Items.Length.Should().Be(1);
        }

        [Fact]
        public void Can_mark_with_item_by_amount()
        {
            var metricName = "test_mark_meter_with_item_by_amount";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, 5L, "item1");

            _fixture.Snapshot.GetMeterValue(_context, metricName).Items.Length.Should().Be(1);
        }

        [Fact]
        public void Can_mark_with_metric_item()
        {
            var metricName = "test_mark_meter_with_metric_item";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, new MetricSetItem("tagKey", "tagvalue"));

            _fixture.Snapshot.GetMeterValue(_context, metricName).Items.Length.Should().Be(1);
        }

        [Fact]
        public void Can_mark_with_metric_item_by_amount()
        {
            var metricName = "test_mark_meter_with_metric_item_by_amount";
            var options = new MeterOptions { Name = metricName };

            _manager.Mark(options, 5L, new MetricSetItem("tagKey", "tagvalue"));

            _fixture.Snapshot.GetMeterValue(_context, metricName).Items.Length.Should().Be(1);
        }
    }
}