// <copyright file="AlgorithmRReservoirSamplingBenchmark.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Scheduling;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Sampling
{
    public class AlgorithmRReservoirSamplingBenchmark : DefaultBenchmarkBase
    {
        private MetricsCoreTestFixture _fixture;
        private DefaultAlgorithmRReservoir _reservoir;
        private IScheduler _scheduler;

        [GlobalSetup]
        public override void Setup()
        {
            _fixture = new MetricsCoreTestFixture();

            _reservoir = new DefaultAlgorithmRReservoir();

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

        [Benchmark]
        public void Update() { _reservoir.Update(_fixture.Rnd.Next(0, 1000), _fixture.RandomUserValue); }

        [GlobalCleanup]
        public void Cleanup()
        {
            _scheduler.Dispose();
        }
    }
}