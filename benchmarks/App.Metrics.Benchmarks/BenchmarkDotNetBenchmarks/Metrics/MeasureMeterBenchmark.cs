// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Support;
using App.Metrics.Tagging;
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