using System;
using System.Linq;
using App.Metrics.Json;
using App.Metrics.MetricData;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace App.Metrics.Facts.Json
{
    public class JsonSerializationTests
    {
        private readonly CounterValueSource counter;

        private readonly CounterValue counterValue = new CounterValue(7, new[]
        {
            new CounterValue.SetItem("item", 6, 0.9)
        });

        private readonly MetricsData data;

        private readonly GaugeValueSource gauge = new GaugeValueSource("test", Provider(0.5), Unit.MegaBytes, MetricTags.None);
        private readonly HistogramValueSource histogram;

        private readonly HistogramValue histogramValue = new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
        private readonly JsonMetricsContext jsonContext;
        private readonly MeterValueSource meter;

        private readonly MeterValue meterValue = new MeterValue(5, 1, 2, 3, 4, TimeUnit.Seconds, new[]
        {
            new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
        });

        private readonly TimerValueSource timer;

        private readonly TimerValue timerValue;

        public JsonSerializationTests()
        {
            this.timerValue = new TimerValue(this.meterValue, this.histogramValue, 0, 1, TimeUnit.Nanoseconds);

            this.counter = new CounterValueSource("test1", Provider(counterValue), Unit.Errors, MetricTags.None);
            this.meter = new MeterValueSource("test2", Provider(meterValue), Unit.Calls, TimeUnit.Seconds, MetricTags.None);
            this.histogram = new HistogramValueSource("test3", Provider(histogramValue), Unit.Items, MetricTags.None);
            this.timer = new TimerValueSource("test4", Provider(timerValue), Unit.Requests, TimeUnit.Seconds, TimeUnit.Milliseconds, MetricTags.None);

            this.data = new MetricsData("test", new DateTime(2014, 2, 17), new[] { new EnvironmentEntry("name", "1") },
                new[] { gauge }, new[] { counter }, new[] { meter }, new[] { histogram }, new[] { timer },
                Enumerable.Empty<MetricsData>()
            );
            this.jsonContext = JsonMetricsContext.FromContext(this.data, "1");
        }

        [Fact]
        public void JsonSerialization_CanSerializeContext()
        {
            jsonContext.Version.Should().Be("1");
            jsonContext.Timestamp.Should().Be(new DateTime(2014, 2, 17));
            jsonContext.Environment.Should().HaveCount(1);
            jsonContext.Environment.Single().Key.Should().Be("name");

            var json = jsonContext.ToJsonObject().AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Version.Should().Be(jsonContext.Version);
            result.Timestamp.Should().Be(jsonContext.Timestamp);
            result.Environment.Should().Equal(jsonContext.Environment);
        }

        [Fact]
        public void JsonSerialization_CanSerializeCounter()
        {
            jsonContext.Counters.Should().HaveCount(1);
            jsonContext.Counters[0].Name.Should().Be(counter.Name);
            jsonContext.Counters[0].Count.Should().Be(counter.Value.Count);
            jsonContext.Counters[0].Unit.Should().Be(counter.Unit.Name);
            jsonContext.Counters[0].Items.Should().HaveCount(1);
            jsonContext.Counters[0].Items[0].Item.Should().Be("item");
            jsonContext.Counters[0].Items[0].Count.Should().Be(6);
            jsonContext.Counters[0].Items[0].Percent.Should().Be(0.9);

            var json = jsonContext.ToJsonObject().AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Counters.ShouldBeEquivalentTo(jsonContext.Counters);
        }

        [Fact]
        public void JsonSerialization_CanSerializeGauge()
        {
            jsonContext.Gauges.Should().HaveCount(1);
            jsonContext.Gauges[0].Name.Should().Be(gauge.Name);
            jsonContext.Gauges[0].Value.Should().Be(gauge.Value);
            jsonContext.Gauges[0].Unit.Should().Be(gauge.Unit.Name);

            var json = jsonContext.ToJsonObject().AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Gauges.ShouldBeEquivalentTo(jsonContext.Gauges);
        }

        [Fact]
        public void JsonSerialization_CanSerializeHistogram()
        {
            jsonContext.Histograms.Should().HaveCount(1);
            jsonContext.Histograms[0].Name.Should().Be(histogram.Name);
            jsonContext.Histograms[0].Count.Should().Be(histogram.Value.Count);

            jsonContext.Histograms[0].LastValue.Should().Be(histogram.Value.LastValue);
            jsonContext.Histograms[0].LastUserValue.Should().Be(histogram.Value.LastUserValue);

            jsonContext.Histograms[0].Max.Should().Be(histogram.Value.Max);
            jsonContext.Histograms[0].MaxUserValue.Should().Be(histogram.Value.MaxUserValue);

            jsonContext.Histograms[0].Mean.Should().Be(histogram.Value.Mean);

            jsonContext.Histograms[0].Min.Should().Be(histogram.Value.Min);
            jsonContext.Histograms[0].MinUserValue.Should().Be(histogram.Value.MinUserValue);

            jsonContext.Histograms[0].StdDev.Should().Be(histogram.Value.StdDev);

            jsonContext.Histograms[0].Median.Should().Be(histogram.Value.Median);
            jsonContext.Histograms[0].Percentile75.Should().Be(histogram.Value.Percentile75);
            jsonContext.Histograms[0].Percentile95.Should().Be(histogram.Value.Percentile95);
            jsonContext.Histograms[0].Percentile98.Should().Be(histogram.Value.Percentile98);
            jsonContext.Histograms[0].Percentile99.Should().Be(histogram.Value.Percentile99);
            jsonContext.Histograms[0].Percentile999.Should().Be(histogram.Value.Percentile999);

            jsonContext.Histograms[0].SampleSize.Should().Be(histogram.Value.SampleSize);

            jsonContext.Histograms[0].Unit.Should().Be(histogram.Unit.Name);


            var json = jsonContext.ToJsonObject().AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Histograms.ShouldBeEquivalentTo(jsonContext.Histograms);
        }

        [Fact]
        public void JsonSerialization_CanSerializeMeter()
        {
            jsonContext.Meters.Should().HaveCount(1);
            jsonContext.Meters[0].Name.Should().Be(meter.Name);

            jsonContext.Meters[0].Count.Should().Be(meter.Value.Count);
            jsonContext.Meters[0].MeanRate.Should().Be(meter.Value.MeanRate);
            jsonContext.Meters[0].OneMinuteRate.Should().Be(meter.Value.OneMinuteRate);
            jsonContext.Meters[0].FiveMinuteRate.Should().Be(meter.Value.FiveMinuteRate);
            jsonContext.Meters[0].FifteenMinuteRate.Should().Be(meter.Value.FifteenMinuteRate);
            jsonContext.Meters[0].Unit.Should().Be(meter.Unit.Name);
            jsonContext.Meters[0].RateUnit.Should().Be(meter.RateUnit.Unit());

            jsonContext.Meters[0].Items.Should().HaveCount(1);
            jsonContext.Meters[0].Items[0].Item.Should().Be("item");
            jsonContext.Meters[0].Items[0].Count.Should().Be(1);
            jsonContext.Meters[0].Items[0].Percent.Should().Be(0.5);
            jsonContext.Meters[0].Items[0].MeanRate.Should().Be(2);
            jsonContext.Meters[0].Items[0].OneMinuteRate.Should().Be(3);
            jsonContext.Meters[0].Items[0].FiveMinuteRate.Should().Be(4);
            jsonContext.Meters[0].Items[0].FifteenMinuteRate.Should().Be(5);

            var json = jsonContext.ToJsonObject().AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Meters.ShouldBeEquivalentTo(jsonContext.Meters);
        }

        [Fact]
        public void JsonSerialization_CanSerializeTimer()
        {
            jsonContext.Timers.Should().HaveCount(1);
            jsonContext.Timers[0].Name.Should().Be(timer.Name);
            jsonContext.Timers[0].Count.Should().Be(5);
            jsonContext.Timers[0].Unit.Should().Be(timer.Unit.Name);
            jsonContext.Timers[0].RateUnit.Should().Be(timer.RateUnit.Unit());
            jsonContext.Timers[0].DurationUnit.Should().Be(timer.DurationUnit.Unit());

            var json = jsonContext.ToJsonObject().AsJson(true);

            var result = JsonConvert.DeserializeObject<JsonMetricsContext>(json);

            result.Histograms.ShouldBeEquivalentTo(jsonContext.Histograms);
        }

        private static IMetricValueProvider<T> Provider<T>(T value)
        {
            return new ConstantProvider<T>(value);
        }

        private class ConstantProvider<T> : IMetricValueProvider<T>
        {
            public ConstantProvider(T value)
            {
                this.Value = value;
            }

            public T Value { get; set; }

            public T GetValue(bool resetMetric = false)
            {
                return this.Value;
            }

            public bool Merge(IMetricValueProvider<T> other)
            {
                return false;
            }
        }
    }
}