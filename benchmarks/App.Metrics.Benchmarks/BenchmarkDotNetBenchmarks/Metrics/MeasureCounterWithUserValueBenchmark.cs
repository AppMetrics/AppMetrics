// <copyright file="MeasureCounterWithUserValueBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureCounterWithUserValueBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void DecrementUserValue()
        {
            Fixture.Metrics.Measure.Counter.Decrement(
                MetricOptions.Counter.OptionsWithUserValue,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }

        [Benchmark]
        public void IncrementUserValue()
        {
            Fixture.Metrics.Measure.Counter.Increment(
                MetricOptions.Counter.OptionsWithUserValue,
                Fixture.Rnd.Next(0, 1000),
                Fixture.RandomUserValue);
        }
    }
}