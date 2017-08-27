// <copyright file="MeasureCounterBenchmark.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureCounterBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void Decrement() { Fixture.Metrics.Measure.Counter.Decrement(MetricOptions.Counter.Options); }

        [Benchmark]
        public void Increment() { Fixture.Metrics.Measure.Counter.Increment(MetricOptions.Counter.Options); }
    }
}