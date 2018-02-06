// <copyright file="Timer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics;
using App.Metrics.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.XunitHarness
{
    [Trait("Benchmark-MetricType", "Timer")]
    public class Timer
    {
        private readonly ITestOutputHelper _output;

        public Timer(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void CostOfMeasuringTimer() { BenchmarkTestRunner.CanCompileAndRun<MeasureTimerBenchmark>(_output); }
    }
}