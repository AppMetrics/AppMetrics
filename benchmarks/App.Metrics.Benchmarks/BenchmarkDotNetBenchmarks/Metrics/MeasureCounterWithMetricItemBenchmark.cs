// <copyright file="MeasureCounterWithMetricItemBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
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
        public void DecrementMetricItemWithMultipleTags()
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
        public void IncrementMetricItemWithMultipleTags()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "value1", "value2" };

            Fixture.Metrics.Measure.Counter.Increment(
                MetricOptions.Counter.OptionsWithMetricItem,
                new MetricSetItem(keys, values));
        }
    }
}