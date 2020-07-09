// <copyright file="MeasureCounterBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
using App.Metrics.Counter;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureCounterBenchmark : DefaultBenchmarkBase
    {
        private const int NumberOfMetrics = 1000;
        private static readonly CounterOptions[] Metrics;

        static MeasureCounterBenchmark()
        {
            Metrics = new CounterOptions[NumberOfMetrics];

            for (var i = 0; i < NumberOfMetrics; i++)
            {
                Metrics[i] = new CounterOptions {Name = $"metric_{i:D4}"};
            }
        }
        
        [Benchmark]
        public void Many()
        {
            for (var i = 0; i < NumberOfMetrics; i++)
            {
                Fixture.Metrics.Measure.Counter.Increment(Metrics[i]);
            }
        }
        
        [Benchmark]
        public void Decrement() { Fixture.Metrics.Measure.Counter.Decrement(MetricOptions.Counter.Options); }

        [Benchmark]
        public void Increment() { Fixture.Metrics.Measure.Counter.Increment(MetricOptions.Counter.Options); }
    }
}