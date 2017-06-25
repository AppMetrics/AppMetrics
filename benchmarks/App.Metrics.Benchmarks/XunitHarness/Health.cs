// <copyright file="Health.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Health;
using App.Metrics.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.XunitHarness
{
    [Trait("Benchmark-Health", "ReadStatus")]
    public class Health
    {
        private readonly ITestOutputHelper _output;

        public Health(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void CostOfReadingHealth() { BenchmarkTestRunner.CanCompileAndRun<MeasureReadHealthStatusBenchmark>(_output); }
    }
}
