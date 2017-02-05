// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Support;
using App.Metrics.Tagging;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks
{
    public class MeasureCounterBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void Decrement() { Fixture.Metrics.Measure.Counter.Decrement(MetricOptions.Counter.Options); }

        [Benchmark]
        public void DecrementMetricItem()
        {
            Fixture.Metrics.Measure.Counter.Decrement(
                MetricOptions.Counter.OptionsWithMetricItem,
                () => new MetricItem("key", "value"));
        }

        [Benchmark]
        public void DecrementUserValue()
        {
            Fixture.Metrics.Measure.Counter.Decrement(
                MetricOptions.Counter.OptionsWithUserValue,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }

        [Benchmark]
        public void Increment() { Fixture.Metrics.Measure.Counter.Increment(MetricOptions.Counter.Options); }

        [Benchmark]
        public void IncrementMetricItem()
        {
            Fixture.Metrics.Measure.Counter.Increment(
                MetricOptions.Counter.OptionsWithMetricItem,
                () => new MetricItem("key", "value"));
        }

        [Benchmark]
        public void IncrementUserValue()
        {
            Fixture.Metrics.Measure.Counter.Increment(
                MetricOptions.Counter.OptionsWithUserValue,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }
    }
}