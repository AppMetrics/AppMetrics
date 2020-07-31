// <copyright file="MeasureTimerBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.BucketTimer;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureBucketTimerBenchmark : DefaultBenchmarkBase
    {
        private const int NumberOfMetrics = 1000;
        private static readonly BucketTimerOptions[] Metrics;

        static MeasureBucketTimerBenchmark()
        {
            Metrics = new BucketTimerOptions[NumberOfMetrics];

            for (var i = 0; i < NumberOfMetrics; i++)
                Metrics[i] = new BucketTimerOptions
                {
                    Name = $"metric_{i:D4}",
                    Buckets = new[] {.005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10}
                };
        }

        [Benchmark]
        public void Many()
        {
            for (var i = 0; i < NumberOfMetrics; i++)
                using (Fixture.Metrics.Measure.BucketTimer.Time(Metrics[i]))
                {
                }
        }
    }
}