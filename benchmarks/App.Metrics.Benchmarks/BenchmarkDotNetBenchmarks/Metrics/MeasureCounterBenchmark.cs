// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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