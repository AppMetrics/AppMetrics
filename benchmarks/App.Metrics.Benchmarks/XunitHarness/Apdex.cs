// <copyright file="Apdex.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics;
using App.Metrics.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.XunitHarness
{
    [Trait("Benchmark-MetricType", "Apdex")]
    public class Apdex
    {
        private readonly ITestOutputHelper _output;

        public Apdex(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void CostOfMeasuringApdex() { BenchmarkTestRunner.CanCompileAndRun<MeasureApdexBenchmark>(_output); }
    }
}