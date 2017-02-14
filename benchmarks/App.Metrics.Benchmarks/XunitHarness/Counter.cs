// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics;
using App.Metrics.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.XunitHarness
{
    [Trait("Benchmark-MetricType", "Counter")]
    public class Counter
    {
        private readonly ITestOutputHelper _output;

        public Counter(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void CostOfMeasuringCounter() { BenchmarkTestRunner.CanCompileAndRun<MeasureCounterWithMetricItemBenchmark>(_output); }
    }
}