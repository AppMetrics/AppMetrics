// <copyright file="MeasureReadHealthStatusBenchmark.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
            var result = FixtureWithHealth.Health.ReadStatusAsync().GetAwaiter().GetResult();
            Debug.WriteLine("Health status: " + result.Status);
        }
    }
}
