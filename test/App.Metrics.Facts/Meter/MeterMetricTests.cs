// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Meter.Extensions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Meter
{
    public class MeterMetricTests
    {
        private readonly IClock _clock = new TestClock();
        private readonly DefaultMeterMetric _meter;

        public MeterMetricTests()
        {
            var scheduler = new TestTaskScheduler(_clock);
            _meter = new DefaultMeterMetric(_clock, scheduler);
        }

        [Fact]
        public void can_calculate_mean_rate()
        {
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 1);

            _meter.Value.MeanRate.Should().Be(1);

            _clock.Advance(TimeUnit.Seconds, 1);

            _meter.Value.MeanRate.Should().Be(0.5);
        }

        [Fact]
        public void can_compute_multiple_rates()
        {
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 10);
            _meter.Mark(2);

            var value = _meter.Value;

            value.MeanRate.Should().BeApproximately(0.3, 0.001);
            value.OneMinuteRate.Should().BeApproximately(0.1840, 0.001);
            value.FiveMinuteRate.Should().BeApproximately(0.1966, 0.001);
            value.FifteenMinuteRate.Should().BeApproximately(0.1988, 0.001);
        }

        [Fact]
        public void can_compute_percent_with_zero_total()
        {
            _meter.Mark("A");
            _meter.Mark("A", -1);

            _meter.Value.Count.Should().Be(0);

            _meter.Value.Items[0].Item.Should().Be("A");
            _meter.Value.Items[0].Value.Count.Should().Be(0);
            _meter.Value.Items[0].Percent.Should().Be(0);
        }

        [Fact]
        public void can_count()
        {
            _meter.Mark();

            _meter.Value.Count.Should().Be(1L);

            _meter.Mark();
            _meter.Value.Count.Should().Be(2L);

            _meter.Mark(8L);
            _meter.Value.Count.Should().Be(10L);
        }

        [Fact]
        public void can_count_for_multiple_set_items()
        {
            _meter.Mark("A");
            _meter.Mark("B");

            _meter.Value.Count.Should().Be(2L);
            _meter.Value.Items.Should().HaveCount(2);

            _meter.Value.Items[0].Item.Should().Be("A");
            _meter.Value.Items[0].Value.Count.Should().Be(1);
            _meter.Value.Items[0].Percent.Should().Be(50);

            _meter.Value.Items[1].Item.Should().Be("B");
            _meter.Value.Items[1].Value.Count.Should().Be(1);
            _meter.Value.Items[1].Percent.Should().Be(50);
        }

        [Fact]
        public void can_count_for_set_item()
        {
            _meter.Mark("A");
            _meter.Value.Count.Should().Be(1L);
            _meter.Value.Items.Should().HaveCount(1);

            _meter.Value.Items[0].Item.Should().Be("A");
            _meter.Value.Items[0].Value.Count.Should().Be(1);
            _meter.Value.Items[0].Percent.Should().Be(100);
        }

        [Fact]
        public void can_reset()
        {
            _meter.Mark();
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 10);
            _meter.Mark(2);

            _meter.Reset();
            _meter.Value.Count.Should().Be(0L);
            _meter.Value.OneMinuteRate.Should().Be(0);
            _meter.Value.FiveMinuteRate.Should().Be(0);
            _meter.Value.FifteenMinuteRate.Should().Be(0);
        }

        [Fact]
        public void can_reset_set_item()
        {
            _meter.Mark("A");
            _meter.Value.Items[0].Value.Count.Should().Be(1);
            _meter.Reset();
            _meter.Value.Items[0].Value.Count.Should().Be(0L);
        }

        [Fact]
        public void rates_should_start_at_zero()
        {
            _meter.Value.MeanRate.Should().Be(0L);
            _meter.Value.OneMinuteRate.Should().Be(0L);
            _meter.Value.FiveMinuteRate.Should().Be(0L);
            _meter.Value.FifteenMinuteRate.Should().Be(0L);
        }

        [Fact]
        public void returns_empty_meter_if_not_meter_metric()
        {
            var meter = new CustomMeter();
            var value = meter.GetValueOrDefault();
            value.Should().NotBeNull();
        }

        [Fact]
        public void value_can_scale_down()
        {
            _meter.Mark();
            _clock.Advance(TimeUnit.Milliseconds, 1);
            _meter.Mark();
            _clock.Advance(TimeUnit.Milliseconds, 1);

            var scaledValue = _meter.Value.Scale(TimeUnit.Milliseconds);

            scaledValue.MeanRate.Should().Be(1);
        }

        [Fact]
        public void value_can_scale_down_to_decimal()
        {
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 1);
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 1);

            var scaledValue = _meter.Value.Scale(TimeUnit.Milliseconds);

            scaledValue.MeanRate.Should().Be(0.001);
        }

        [Fact]
        public void value_can_scale_up()
        {
            _meter.Mark();
            _clock.Advance(TimeUnit.Minutes, 1);
            _meter.Mark();
            _clock.Advance(TimeUnit.Minutes, 1);

            var scaledValue = _meter.GetValueOrDefault().Scale(TimeUnit.Minutes);

            scaledValue.MeanRate.Should().Be(1);
        }
    }
}