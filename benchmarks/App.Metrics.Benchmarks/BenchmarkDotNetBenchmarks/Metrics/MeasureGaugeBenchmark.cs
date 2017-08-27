// <copyright file="MeasureGaugeBenchmark.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureGaugeBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void SetValue()
        {
            Fixture.Metrics.Measure.Gauge.SetValue(MetricOptions.Gauge.Options, () => Fixture.Rnd.NextDouble());
        }

        [Benchmark]
        public void SetValueNotLazy()
        {
            Fixture.Metrics.Measure.Gauge.SetValue(MetricOptions.Gauge.OptionsNotLazy, Fixture.Rnd.NextDouble());
        }
    }
}