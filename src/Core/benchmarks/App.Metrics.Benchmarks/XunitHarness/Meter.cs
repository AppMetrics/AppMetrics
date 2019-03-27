// <copyright file="Meter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics;
using App.Metrics.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.XunitHarness
{
    [Trait("Benchmark-MetricType", "Meter")]
    public class Meter
    {
        private readonly ITestOutputHelper _output;

        public Meter(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void CostOfMeasuringCounter() { BenchmarkTestRunner.CanCompileAndRun<MeasureMeterBenchmark>(_output); }
    }
}