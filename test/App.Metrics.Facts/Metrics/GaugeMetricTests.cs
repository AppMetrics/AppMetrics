using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Internal;
using App.Metrics.Internal.Test;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class GaugeMetricTests
    {
        [Theory]
        [InlineData(2.0, 4.0, 50.0)]
        [InlineData(0.0, 4.0, 0.0)]
        [InlineData(4.0, 2.0, 100.0)]
        [InlineData(4.0, 0.0, 100.0)]
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

            var cacheHitMeter = new MeterMetric(clock, scheduler);
            var dbQueryTimer = new TimerMetric(
                SamplingType.LongTerm,
                Constants.ReservoirSampling.DefaultSampleSize,
                Constants.ReservoirSampling.DefaultExponentialDecayFactor,
                clock);

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
        public void should_report_nan_on_exception()
        {
            new FunctionGauge(() => { throw new InvalidOperationException("test"); }).Value.Should().Be(double.NaN);

            new DerivedGauge(new FunctionGauge(() => 5.0), (d) => { throw new InvalidOperationException("test"); }).Value.Should().Be(double.NaN);
        }
    }
}