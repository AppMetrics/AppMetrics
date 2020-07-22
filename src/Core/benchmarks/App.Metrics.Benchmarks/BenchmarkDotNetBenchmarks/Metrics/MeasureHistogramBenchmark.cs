// <copyright file="MeasureHistogramBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
using App.Metrics.Histogram;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureHistogramBenchmark : DefaultBenchmarkBase
    {
        private const int NumberOfMetrics = 1000;
        private static readonly HistogramOptions[] Metrics;

        static MeasureHistogramBenchmark()
        {
            Metrics = new HistogramOptions[NumberOfMetrics];

            for (var i = 0; i < NumberOfMetrics; i++)
            {
                Metrics[i] = new HistogramOptions {Name = $"metric_{i:D4}"};
            }
        }
        
        [Benchmark]
        public void Many()
        {
            for (var i = 0; i < NumberOfMetrics; i++)
            {
                Fixture.Metrics.Measure.Histogram.Update(Metrics[i], 1);
            }
        }
        
        [Benchmark]
        public void UpdateAlgorithmR()
        {
            Fixture.Metrics.Measure.Histogram.Update(
                MetricOptions.Histogram.OptionsAlgorithmR,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }

        [Benchmark]
        public void UpdateForwardDecaying()
        {
            Fixture.Metrics.Measure.Histogram.Update(
                MetricOptions.Histogram.OptionsForwardDecaying,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }

        [Benchmark]
        public void UpdateSlidingWindow()
        {
            Fixture.Metrics.Measure.Histogram.Update(
                MetricOptions.Histogram.OptionsSlidingWindow,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }
    }
}