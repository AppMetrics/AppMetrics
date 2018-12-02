// <copyright file="DefaultMetricValuesProviderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
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
        private readonly MetricTags[] _tags;

        public DefaultMetricValuesProviderTests(MetricCoreTestFixture fixture)
        {
            _provider = fixture.Snapshot;
            _measure = fixture.Managers;
            _clock = fixture.Clock;
            _tags = fixture.Tags;
        }

        [Fact]
        public void Can_get_apdex_value()
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
        public void Can_get_apdex_value_with_tags()
        {
            var metricName = "DefaultMetricValuesProviderTests_apdex";
            var options = new ApdexOptions
                          {
                              Name = metricName,
                              Context = Context
                          };
            var tags = new MetricTags("key", "value");

            _measure.Apdex.Track(options, tags, () => _clock.Advance(TimeUnit.Seconds, 3));

            _provider.GetApdexValue(Context, tags.AsMetricName(metricName)).Frustrating.Should().Be(1);
        }

        [Fact]
        public void Can_get_counter_value()
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
        public void Can_get_counter_value_with_tags()
        {
            var metricName = "DefaultMetricValuesProviderTests_counter";
            var options = new CounterOptions
                          {
                              Name = metricName,
                              Context = Context
                          };
            var tags = new MetricTags("key", "value");

            _measure.Counter.Increment(options, tags);

            _provider.GetCounterValue(Context, tags.AsMetricName(metricName)).Count.Should().Be(1);
        }

        [Fact]
        public void Can_get_gauge_value()
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
        public void Can_get_gauge_value_with_tags()
        {
            var metricName = "DefaultMetricValuesProviderTests_gauge";
            var options = new GaugeOptions
                          {
                              Name = metricName,
                              Context = Context
                          };
            var tags = new MetricTags("key", "value");

            _measure.Gauge.SetValue(options, tags, () => 1.0);

            _provider.GetGaugeValue(Context, tags.AsMetricName(metricName)).Should().Be(1);
        }

        [Fact]
        public void Can_get_histogram_value()
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
        public void Can_get_histogram_value_with_tags()
        {
            var metricName = "DefaultMetricValuesProviderTests_histogram";
            var options = new HistogramOptions
                          {
                              Name = metricName,
                              Context = Context
                          };
            var tags = new MetricTags("key", "value");

            _measure.Histogram.Update(options, tags, 1L);

            _provider.GetHistogramValue(Context, tags.AsMetricName(metricName)).Count.Should().Be(1);
        }

        [Fact]
        public void Can_get_meter_value()
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
        public void Can_get_meter_value_with_tags()
        {
            var metricName = "DefaultMetricValuesProviderTests_meter";
            var options = new MeterOptions
                          {
                              Name = metricName,
                              Context = Context
                          };
            var tags = new MetricTags("key", "value");

            _measure.Meter.Mark(options, tags, 1L);

            _provider.GetMeterValue(Context, tags.AsMetricName(metricName)).Count.Should().Be(1);
        }

        [Fact]
        public void Can_get_multidimensional_apdex_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_apdex_multi";
            var options = new ApdexOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Apdex.Track(options, _tags[0], () => _clock.Advance(TimeUnit.Seconds, 3));

            _provider.GetApdexValue(Context, _tags[0].AsMetricName(metricName)).Frustrating.Should().Be(1);
        }

        [Fact]
        public void Can_get_multidimensional_counter_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_counter_multi";
            var options = new CounterOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Counter.Increment(options, _tags[1]);

            _provider.GetCounterValue(Context, _tags[1].AsMetricName(metricName)).Count.Should().Be(1);
        }

        [Fact]
        public void Can_get_multidimensional_gauge_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_gauge_multi";
            var options = new GaugeOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Gauge.SetValue(options, _tags[0], () => 1.0);

            _provider.GetGaugeValue(Context, _tags[0].AsMetricName(metricName)).Should().Be(1);
        }

        [Fact]
        public void Can_get_multidimensional_histogram_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_histogram_multi";
            var options = new HistogramOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Histogram.Update(options, _tags[1], 1L);

            _provider.GetHistogramValue(Context, _tags[1].AsMetricName(metricName)).Count.Should().Be(1);
        }

        [Fact]
        public void Can_get_multidimensional_meter_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_meter_multi";
            var options = new MeterOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Meter.Mark(options, _tags[1], 1L);

            _provider.GetMeterValue(Context, _tags[1].AsMetricName(metricName)).Count.Should().Be(1);
        }

        [Fact]
        public void Can_get_multidimensional_timer_value()
        {
            var metricName = "DefaultMetricValuesProviderTests_timer_multi";
            var options = new TimerOptions
                          {
                              Name = metricName,
                              Context = Context
                          };

            _measure.Timer.Time(options, _tags[1], () => { _clock.Advance(TimeUnit.Milliseconds, 1000); });

            _provider.GetTimerValue(Context, _tags[1].AsMetricName(metricName)).Histogram.Count.Should().Be(1);
        }

        [Fact]
        public void Can_get_timer_value()
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
        public void Can_get_timer_value_with_tags()
        {
            var metricName = "DefaultMetricValuesProviderTests_timer";
            var options = new TimerOptions
                          {
                              Name = metricName,
                              Context = Context
                          };
            var tags = new MetricTags("key", "value");

            _measure.Timer.Time(options, tags, () => { _clock.Advance(TimeUnit.Milliseconds, 1000); });

            _provider.GetTimerValue(Context, tags.AsMetricName(metricName)).Histogram.Count.Should().Be(1);
        }

        [Fact]
        public void Context_doesnt_exist_returns_default_apdex()
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
        public void Context_doesnt_exist_returns_default_apdex_when_multidimensional()
        {
            var metricName = "DefaultMetricValuesProviderTests_apdex_without_context_multi";
            var options = new ApdexOptions
                          {
                              Name = metricName,
                              Context = "different"
                          };

            _measure.Apdex.Track(options, _tags[1], () => _clock.Advance(TimeUnit.Seconds, 3));

            _provider.GetApdexValue(Context, _tags[1].AsMetricName(metricName)).Should().Be(default(ApdexValue));
        }

        [Fact]
        public void Context_doesnt_exist_returns_default_counter()
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
        public void Context_doesnt_exist_returns_default_counter_when_multidimensional()
        {
            var metricName = "DefaultMetricValuesProviderTests_counter_without_context_multi";
            var options = new CounterOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Counter.Increment(options, _tags[1]);

            _provider.GetCounterValue(Context, _tags[1].AsMetricName(metricName)).Should().NotBe(1);
        }

        [Fact]
        public void Context_doesnt_exist_returns_default_gauge()
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
        public void Context_doesnt_exist_returns_default_gauge_when_multidimensional()
        {
            var metricName = "DefaultMetricValuesProviderTests_gauge_without_context_multi";
            var options = new GaugeOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Gauge.SetValue(options, _tags[1], () => 1.0);

            _provider.GetGaugeValue(Context, _tags[1].AsMetricName(metricName)).Should().NotBe(1);
        }

        [Fact]
        public void Context_doesnt_exist_returns_default_histgoram()
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
        public void Context_doesnt_exist_returns_default_histgoram_when_multidimensional()
        {
            var metricName = "DefaultMetricValuesProviderTests_histgoram_without_context_multi";
            var options = new HistogramOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Histogram.Update(options, _tags[1], 1L);

            _provider.GetHistogramValue(Context, _tags[1].AsMetricName(metricName)).Should().NotBe(1);
        }

        [Fact]
        public void Context_doesnt_exist_returns_default_meter()
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
        public void Context_doesnt_exist_returns_default_meter_when_multidimensional()
        {
            var metricName = "DefaultMetricValuesProviderTests_meter_without_context_multi";
            var options = new MeterOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Meter.Mark(options, _tags[0], 1L);

            _provider.GetMeterValue(Context, _tags[0].AsMetricName(metricName)).Should().Be(default(MeterValue));
        }

        [Fact]
        public void Context_doesnt_exist_returns_default_timer()
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

        [Fact]
        public void Context_doesnt_exist_returns_default_timer_when_multidimensional()
        {
            var metricName = "DefaultMetricValuesProviderTests_timer_without_context_multi";
            var options = new TimerOptions
                          {
                              Name = "different",
                              Context = Context
                          };

            _measure.Timer.Time(options, _tags[1], () => { _clock.Advance(TimeUnit.Milliseconds, 1000); });

            _provider.GetTimerValue(Context, _tags[1].AsMetricName(metricName)).Should().Be(default(TimerValue));
        }
    }
}