// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Counter;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Meter.Extensions;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultMetricValuesProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private static readonly string Context = "DefaultMetricValuesProviderTests";
        private readonly IClock _clock;
        private readonly IMeasureMetrics _measure;
        private readonly IProvideMetricValues _provider;

        public DefaultMetricValuesProviderTests(MetricCoreTestFixture fixture)
        {
            _provider = fixture.Snapshot;
            _measure = fixture.Managers;
            _clock = fixture.Clock;
        }

        [Fact]
        public void can_get_apdex_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_apdex";
            var options = new ApdexOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Apdex.Track(options, () => _clock.Advance(TimeUnit.Seconds, 3));

            _provider.GetApdexValue(Context, metricName).Frustrating.Should().Be(1);
        }

        [Fact]
        public void can_get_counter_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_counter";
            var options = new CounterOptions
                          {
                              Name = metricName,
                              Context = Context
                          };
            _measure.Counter.Increment(options);

            _provider.GetCounterValue(Context, metricName).Count.Should().Be(1);
        }

        [Fact]
        public void can_get_gauge_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_gauge";
            var options = new GaugeOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Gauge.SetValue(options, () => 1.0);

            _provider.GetGaugeValue(Context, metricName).Should().Be(1);
        }

        [Fact]
        public void can_get_histogram_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_histogram";
            var options = new HistogramOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Histogram.Update(options, 1L);

            _provider.GetHistogramValue(Context, metricName).Count.Should().Be(1);
        }

        [Fact]
        public void can_get_meter_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_meter";
            var options = new MeterOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Meter.Mark(options, 1L);

            _provider.GetMeterValue(Context, metricName).Count.Should().Be(1);
        }

        [Fact]
        public void can_get_timer_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_timer";
            var options = new TimerOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Timer.Time(options, () => { _clock.Advance(TimeUnit.Milliseconds, 1000); });

            _provider.GetTimerValue(Context, metricName).Histogram.Count.Should().Be(1);
        }

        [Fact]
        public void context_doesnt_exist_returns_default_apdex()
        {
            var metricName = "DefaultMetricValuesProviderTests_apdex_without_context";
            var options = new ApdexOptions
                          {
                              Name = metricName,
                              Context = "different"
                          };

            _measure.Apdex.Track(options, () => _clock.Advance(TimeUnit.Seconds, 3));

            _provider.GetApdexValue(Context, metricName).Should().Be(default(ApdexValue));
        }

        [Fact]
        public void context_doesnt_exist_returns_default_counter()
        {
            var metricName = "DefaultMetricValuesProviderTests_counter_without_context";
            var options = new CounterOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Counter.Increment(options);

            _provider.GetCounterValue(Context, metricName).Should().NotBe(1);
        }

        [Fact]
        public void context_doesnt_exist_returns_default_gauge()
        {
            var metricName = "DefaultMetricValuesProviderTests_gauge_without_context";
            var options = new GaugeOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Gauge.SetValue(options, () => 1.0);

            _provider.GetGaugeValue(Context, metricName).Should().NotBe(1);
        }

        [Fact]
        public void context_doesnt_exist_returns_default_histgoram()
        {
            var metricName = "DefaultMetricValuesProviderTests_histgoram_without_context";
            var options = new HistogramOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Histogram.Update(options, 1L);

            _provider.GetHistogramValue(Context, metricName).Should().NotBe(1);
        }

        [Fact]
        public void context_doesnt_exist_returns_default_meter()
        {
            var metricName = "DefaultMetricValuesProviderTests_meter_without_context";
            var options = new MeterOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Meter.Mark(options, 1L);

            _provider.GetMeterValue(Context, metricName).Should().Be(default(MeterValue));
        }

        [Fact]
        public void context_doesnt_exist_returns_default_timer()
        {
            var metricName = "DefaultMetricValuesProviderTests_timer_without_context";
            var options = new TimerOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Timer.Time(options, () => { _clock.Advance(TimeUnit.Milliseconds, 1000); });

            _provider.GetTimerValue(Context, metricName).Should().Be(default(TimerValue));
        }
    }
}