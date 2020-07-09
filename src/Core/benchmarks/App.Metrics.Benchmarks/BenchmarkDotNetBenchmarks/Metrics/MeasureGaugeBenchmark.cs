// <copyright file="MeasureGaugeBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
using App.Metrics.Gauge;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureGaugeBenchmark : DefaultBenchmarkBase
    {
        private const int NumberOfMetrics = 1000;
        private static readonly GaugeOptions[] Metrics;
        
        static MeasureGaugeBenchmark()
        {
            Metrics = new GaugeOptions[NumberOfMetrics];

            for (var i = 0; i < NumberOfMetrics; i++)
            {
                Metrics[i] = new GaugeOptions {Name = $"metric_{i:D4}"};
            }
        }
        
        [Benchmark]
        public void Many()
        {
            for (var i = 0; i < NumberOfMetrics; i++)
            {
                Fixture.Metrics.Measure.Gauge.SetValue(Metrics[i], 1);
            }
        }
        
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