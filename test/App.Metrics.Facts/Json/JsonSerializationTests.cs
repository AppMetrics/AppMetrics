using System;
using System.Linq;
using App.Metrics.Infrastructure;
using App.Metrics.Json;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace App.Metrics.Facts.Json
{
    public class JsonSerializationTests
    {
        private readonly CounterValueSource _counter;
        private static readonly IClock Clock = new Clock.TestClock();

        private readonly CounterValue _counterValue = new CounterValue(7, new[]
        {
            new CounterValue.SetItem("item", 6, 0.9)
        });

        private readonly GaugeValueSource _gauge = new GaugeValueSource("test", Provider(0.5), Unit.MegaBytes, MetricTags.None);
        private readonly HistogramValueSource _histogram;

        private readonly HistogramValue _histogramValue = new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
        private readonly JsonMetricsContext _jsonContext;
        private readonly MeterValueSource _meter;
        private readonly JsonMetricsGroup _defaultGroup;

        private readonly MeterValue _meterValue = new MeterValue(5, 1, 2, 3, 4, TimeUnit.Seconds, new[]
        {
            new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
        });

        private readonly TimerValueSource _timer;

        public JsonSerializationTests()
        {
            var environment = new EnvironmentInfo(string.Empty, string.Empty, "test", String.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, 4);
            var timerValue = new TimerValue(_meterValue, _histogramValue, 0, 1, TimeUnit.Nanoseconds);

            _counter = new CounterValueSource("test1", Provider(_counterValue), Unit.Errors, MetricTags.None);
            _meter = new MeterValueSource("test2", Provider(_meterValue), Unit.Calls, TimeUnit.Seconds, MetricTags.None);
            _histogram = new HistogramValueSource("test3", Provider(_histogramValue), Unit.Items, MetricTags.None);
            _timer = new TimerValueSource("test4", Provider(timerValue), Unit.Requests, TimeUnit.Seconds, TimeUnit.Milliseconds, MetricTags.None);

            var group = new MetricsDataGroup("test", new[] { _gauge }, new[] { _counter }, new[] { _meter }, new[] { _histogram }, new[] { _timer });

            var data = new MetricsData("test_context", new DateTime(2014, 2, 17), environment, new [] { group});
            _jsonContext = JsonMetricsContext.FromContext(data, "1");
            _defaultGroup = _jsonContext.Groups.Single();
        }

        [Fact]
        public void JsonSerialization_CanSerializeContext()
        {
            _jsonContext.Version.Should().Be("1");
            _jsonContext.Timestamp.Should().Be(new DateTime(2014, 2, 17));
            _jsonContext.Environment.Should().HaveCount(10);

            var json = _jsonContext.ToJsonObject(Clock).AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Version.Should().Be(_jsonContext.Version);
            result.Timestamp.Should().Be(_jsonContext.Timestamp);
            result.Environment.Should().Equal(_jsonContext.Environment);
        }

        [Fact]
        public void JsonSerialization_CanSerializeCounter()
        {
            _defaultGroup.Counters.Should().HaveCount(1);
            _defaultGroup.Counters[0].Name.Should().Be(_counter.Name);
            _defaultGroup.Counters[0].Count.Should().Be(_counter.Value.Count);
            _defaultGroup.Counters[0].Unit.Should().Be(_counter.Unit.Name);
            _defaultGroup.Counters[0].Items.Should().HaveCount(1);
            _defaultGroup.Counters[0].Items[0].Item.Should().Be("item");
            _defaultGroup.Counters[0].Items[0].Count.Should().Be(6);
            _defaultGroup.Counters[0].Items[0].Percent.Should().Be(0.9);

            var json = _jsonContext.ToJsonObject(Clock).AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Groups.Single().Counters.ShouldBeEquivalentTo(_defaultGroup.Counters);
        }

        [Fact]
        public void JsonSerialization_CanSerializeGauge()
        {
            _defaultGroup.Gauges.Should().HaveCount(1);
            _defaultGroup.Gauges[0].Name.Should().Be(_gauge.Name);
            _defaultGroup.Gauges[0].Value.Should().Be(_gauge.Value);
            _defaultGroup.Gauges[0].Unit.Should().Be(_gauge.Unit.Name);

            var json = _jsonContext.ToJsonObject(Clock).AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Groups.Single().Gauges.ShouldBeEquivalentTo(_defaultGroup.Gauges);
        }

        [Fact]
        public void JsonSerialization_CanSerializeHistogram()
        {
            _defaultGroup.Histograms.Should().HaveCount(1);
            _defaultGroup.Histograms[0].Name.Should().Be(_histogram.Name);
            _defaultGroup.Histograms[0].Count.Should().Be(_histogram.Value.Count);
            _defaultGroup.Histograms[0].LastValue.Should().Be(_histogram.Value.LastValue);
            _defaultGroup.Histograms[0].LastUserValue.Should().Be(_histogram.Value.LastUserValue);
            _defaultGroup.Histograms[0].Max.Should().Be(_histogram.Value.Max);
            _defaultGroup.Histograms[0].MaxUserValue.Should().Be(_histogram.Value.MaxUserValue);
            _defaultGroup.Histograms[0].Mean.Should().Be(_histogram.Value.Mean);
            _defaultGroup.Histograms[0].Min.Should().Be(_histogram.Value.Min);
            _defaultGroup.Histograms[0].MinUserValue.Should().Be(_histogram.Value.MinUserValue);
            _defaultGroup.Histograms[0].StdDev.Should().Be(_histogram.Value.StdDev);
            _defaultGroup.Histograms[0].Median.Should().Be(_histogram.Value.Median);
            _defaultGroup.Histograms[0].Percentile75.Should().Be(_histogram.Value.Percentile75);
            _defaultGroup.Histograms[0].Percentile95.Should().Be(_histogram.Value.Percentile95);
            _defaultGroup.Histograms[0].Percentile98.Should().Be(_histogram.Value.Percentile98);
            _defaultGroup.Histograms[0].Percentile99.Should().Be(_histogram.Value.Percentile99);
            _defaultGroup.Histograms[0].Percentile999.Should().Be(_histogram.Value.Percentile999);
            _defaultGroup.Histograms[0].SampleSize.Should().Be(_histogram.Value.SampleSize);
            _defaultGroup.Histograms[0].Unit.Should().Be(_histogram.Unit.Name);


            var json = _jsonContext.ToJsonObject(Clock).AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Groups.Single().Histograms.ShouldBeEquivalentTo(_defaultGroup.Histograms);
        }

        [Fact]
        public void JsonSerialization_CanSerializeMeter()
        {
            _defaultGroup.Meters.Should().HaveCount(1);
            _defaultGroup.Meters[0].Name.Should().Be(_meter.Name);

            _defaultGroup.Meters[0].Count.Should().Be(_meter.Value.Count);
            _defaultGroup.Meters[0].MeanRate.Should().Be(_meter.Value.MeanRate);
            _defaultGroup.Meters[0].OneMinuteRate.Should().Be(_meter.Value.OneMinuteRate);
            _defaultGroup.Meters[0].FiveMinuteRate.Should().Be(_meter.Value.FiveMinuteRate);
            _defaultGroup.Meters[0].FifteenMinuteRate.Should().Be(_meter.Value.FifteenMinuteRate);
            _defaultGroup.Meters[0].Unit.Should().Be(_meter.Unit.Name);
            _defaultGroup.Meters[0].RateUnit.Should().Be(_meter.RateUnit.Unit());

            _defaultGroup.Meters[0].Items.Should().HaveCount(1);
            _defaultGroup.Meters[0].Items[0].Item.Should().Be("item");
            _defaultGroup.Meters[0].Items[0].Count.Should().Be(1);
            _defaultGroup.Meters[0].Items[0].Percent.Should().Be(0.5);
            _defaultGroup.Meters[0].Items[0].MeanRate.Should().Be(2);
            _defaultGroup.Meters[0].Items[0].OneMinuteRate.Should().Be(3);
            _defaultGroup.Meters[0].Items[0].FiveMinuteRate.Should().Be(4);
            _defaultGroup.Meters[0].Items[0].FifteenMinuteRate.Should().Be(5);

            var json = _jsonContext.ToJsonObject(Clock).AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Groups.Single().Meters.ShouldBeEquivalentTo(_defaultGroup.Meters);
        }

        [Fact]
        public void JsonSerialization_CanSerializeTimer()
        {
            _defaultGroup.Timers.Should().HaveCount(1);
            _defaultGroup.Timers[0].Name.Should().Be(_timer.Name);
            _defaultGroup.Timers[0].Count.Should().Be(5);
            _defaultGroup.Timers[0].Unit.Should().Be(_timer.Unit.Name);
            _defaultGroup.Timers[0].RateUnit.Should().Be(_timer.RateUnit.Unit());
            _defaultGroup.Timers[0].DurationUnit.Should().Be(_timer.DurationUnit.Unit());

            var json = _jsonContext.ToJsonObject(Clock).AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Groups.Single().Histograms.ShouldBeEquivalentTo(_defaultGroup.Histograms);
        }

        private static IMetricValueProvider<T> Provider<T>(T value)
        {
            return new ConstantProvider<T>(value);
        }

        private class ConstantProvider<T> : IMetricValueProvider<T>
        {
            public ConstantProvider(T value)
            {
                Value = value;
            }

            public T Value { get; }

            public T GetValue(bool resetMetric = false)
            {
                return Value;
            }

            public bool Merge(IMetricValueProvider<T> other)
            {
                return false;
            }
        }
    }
}