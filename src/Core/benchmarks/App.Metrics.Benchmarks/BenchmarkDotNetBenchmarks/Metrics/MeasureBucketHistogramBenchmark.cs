// <copyright file="MeasureBucketHistogramBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.BucketHistogram;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureBucketHistogramBenchmark : DefaultBenchmarkBase
    {
        private const int NumberOfMetrics = 1000;
        private static readonly BucketHistogramOptions[] Metrics;

        static MeasureBucketHistogramBenchmark()
        {
            Metrics = new BucketHistogramOptions[NumberOfMetrics];

            for (var i = 0; i < NumberOfMetrics; i++)
                Metrics[i] = new BucketHistogramOptions
                {
                    Name = $"metric_{i:D4}",
                    Buckets = new[] {.005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10}
                };
        }

        [Benchmark]
        public void Many()
        {
            for (var i = 0; i < NumberOfMetrics; i++) Fixture.Metrics.Measure.BucketHistogram.Update(Metrics[i], 123);
        }
    }
}