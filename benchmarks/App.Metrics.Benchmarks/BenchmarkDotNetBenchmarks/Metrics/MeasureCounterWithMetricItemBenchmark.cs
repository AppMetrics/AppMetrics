// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Support;
using App.Metrics.Tagging;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureCounterWithMetricItemBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void DecrementMetricItem()
        {
            Fixture.Metrics.Measure.Counter.Decrement(
                MetricOptions.Counter.OptionsWithMetricItem,
                new MetricSetItem("key", "value"));
        }

        [Benchmark]
        public void DecrementMetricItemWithMulitpleTags()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "value1", "value2" };

            Fixture.Metrics.Measure.Counter.Decrement(
                MetricOptions.Counter.OptionsWithMetricItem,
                new MetricSetItem(keys, values));
        }

        [Benchmark]
        public void IncrementMetricItem()
        {
            Fixture.Metrics.Measure.Counter.Increment(
                MetricOptions.Counter.OptionsWithMetricItem,
                new MetricSetItem("key", "value"));
        }

        [Benchmark]
        public void IncrementMetricItemWithMulitpleTags()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "value1", "value2" };

            Fixture.Metrics.Measure.Counter.Increment(
                MetricOptions.Counter.OptionsWithMetricItem,
                new MetricSetItem(keys, values));
        }
    }
}