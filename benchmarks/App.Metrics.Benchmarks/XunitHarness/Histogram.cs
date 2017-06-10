// <copyright file="Histogram.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics;
using App.Metrics.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.XunitHarness
{
    [Trait("Benchmark-MetricType", "Histogram")]
    public class Histogram
    {
        private readonly ITestOutputHelper _output;

        public Histogram(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void CostOfMeasuringHistogram() { BenchmarkTestRunner.CanCompileAndRun<MeasureHistogramBenchmark>(_output); }
    }
}