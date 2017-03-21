// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Core.Internal;
using App.Metrics.Gauge;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Gauge
{
    public class GaugeMetricTests
    {
        [Theory]
        [InlineData(2.0, 4.0, 50.0)]
        [InlineData(0.0, 4.0, 0.0)]
        [InlineData(4.0, 2.0, 100.0)]
        public void can_calculate_percentage(double numerator, double denominator, double expectedPercentage)
        {
            var hitPercentage = new PercentageGauge(() => numerator, () => denominator);

            hitPercentage.Value.Should().Be(expectedPercentage);
        }

        [Fact]
        public void can_calculate_the_hit_ratio_as_a_guage()
        {
            var clock = new TestClock();
            var scheduler = new TestTaskScheduler(clock);
            var reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir(1028));

            var cacheHitMeter = new DefaultMeterMetric(clock, scheduler);
            var dbQueryTimer = new DefaultTimerMetric(reservoir, clock);

            foreach (var index in Enumerable.Range(0, 1000))
            {
                using (dbQueryTimer.NewContext())
                {
                    clock.Advance(TimeUnit.Milliseconds, 100);
                }

                if (index % 2 == 0)
                {
                    cacheHitMeter.Mark();
                }
            }

            var cacheHitRatioGauge = new HitRatioGauge(cacheHitMeter, dbQueryTimer, value => value.OneMinuteRate);

            cacheHitRatioGauge.Value.Should().BeGreaterThan(0.0);
        }

        [Fact]
        public void can_calculate_the_hit_ratio_as_a_guage_with_one_min_rate_as_default()
        {
            var clock = new TestClock();
            var scheduler = new TestTaskScheduler(clock);
            var reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir(1028));

            var cacheHitMeter = new DefaultMeterMetric(clock, scheduler);
            var dbQueryTimer = new DefaultTimerMetric(reservoir, clock);

            foreach (var index in Enumerable.Range(0, 1000))
            {
                using (dbQueryTimer.NewContext())
                {
                    clock.Advance(TimeUnit.Milliseconds, 100);
                }

                if (index % 2 == 0)
                {
                    cacheHitMeter.Mark();
                }
            }

            var cacheHitRatioGauge = new HitRatioGauge(cacheHitMeter, dbQueryTimer);

            cacheHitRatioGauge.Value.Should().BeGreaterThan(0.0);
        }

        [Fact]
        public void can_get_and_reset_value_gauge()
        {
            var valueGauge = new ValueGauge();
            valueGauge.SetValue(1.0);

            var value = valueGauge.GetValue(true);

            value.Should().Be(1.0);
            valueGauge.Value.Should().Be(0.0);
        }

        [Fact]
        public void can_reset_value_gauge()
        {
            var valueGauge = new ValueGauge();
            valueGauge.SetValue(1.0);

            valueGauge.Value.Should().Be(1.0);

            valueGauge.Reset();

            valueGauge.Value.Should().Be(0.0);
        }

        [Fact]
        public void should_report_nan_on_exception()
        {
            new FunctionGauge(() => throw new InvalidOperationException("test")).Value.Should().Be(double.NaN);

            new DerivedGauge(new FunctionGauge(() => 5.0), (d) => throw new InvalidOperationException("test")).Value.Should().Be(double.NaN);
        }

        [Fact]
        public void when_denomitator_is_zero_returns_NaN()
        {
            var hitPercentage = new PercentageGauge(() => 1, () => 0);

            hitPercentage.Value.Should().Be(double.NaN);
        }
    }
}