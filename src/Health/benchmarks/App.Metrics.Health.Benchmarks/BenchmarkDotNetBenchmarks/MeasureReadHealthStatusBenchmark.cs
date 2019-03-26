// <copyright file="MeasureReadHealthStatusBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Health.Benchmarks.BenchmarkDotNetBenchmarks
{
    public class MeasureReadHealthStatusBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void ReadHealth()
        {
            var result = FixtureWithHealth.HealthCheckRunner.ReadAsync().GetAwaiter().GetResult();
            Debug.WriteLine("Health status: " + result.Status);
        }
    }
}
