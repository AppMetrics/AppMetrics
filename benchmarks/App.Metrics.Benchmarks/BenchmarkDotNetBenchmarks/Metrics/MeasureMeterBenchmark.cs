// <copyright file="MeasureMeterBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureMeterBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void Mark() { Fixture.Metrics.Measure.Meter.Mark(MetricOptions.Meter.Options); }

        [Benchmark]
        public void MarkMetricItem()
        {
            Fixture.Metrics.Measure.Meter.Mark(
                MetricOptions.Meter.OptionsWithMetricItem,
                new MetricSetItem("key", "value"));
        }

        [Benchmark]
        public void MarkMetricItemWithMultipleTags()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "value1", "value2" };

            Fixture.Metrics.Measure.Meter.Mark(
                MetricOptions.Meter.OptionsWithMetricItem,
                new MetricSetItem(keys, values));
        }

        [Benchmark]
        public void MarkUserValue()
        {
            Fixture.Metrics.Measure.Meter.Mark(
                MetricOptions.Meter.OptionsWithUserValue,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }
    }
}