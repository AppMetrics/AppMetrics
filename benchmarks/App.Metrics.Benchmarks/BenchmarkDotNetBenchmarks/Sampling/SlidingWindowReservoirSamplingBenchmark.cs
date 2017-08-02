// <copyright file="SlidingWindowReservoirSamplingBenchmark.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.Scheduling;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Sampling
{
    public class SlidingWindowReservoirSamplingBenchmark : DefaultBenchmarkBase
    {
        private MetricsCoreTestFixture _fixture;
        private DefaultSlidingWindowReservoir _reservoir;
        private IScheduler _scheduler;

        [GlobalSetup]
        public override void Setup()
        {
            _fixture = new MetricsCoreTestFixture();

            _reservoir = new DefaultSlidingWindowReservoir();

            _scheduler = new DefaultTaskScheduler();

            _scheduler.Interval(
                TimeSpan.FromMilliseconds(10),
                TaskCreationOptions.None,
                () =>
                {
                    _reservoir.GetSnapshot();
                    _reservoir.Reset();
                });
        }

        [GlobalCleanup]
        public void Cleanup() { _scheduler.Dispose(); }

        [Benchmark]
        public void Update() { _reservoir.Update(_fixture.Rnd.Next(0, 1000), _fixture.RandomUserValue); }
    }
}