// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Support;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureGaugeBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void SetValue() { Fixture.Metrics.Measure.Gauge.SetValue(MetricOptions.Gauge.Options, () => Fixture.Rnd.NextDouble()); }
    }
}