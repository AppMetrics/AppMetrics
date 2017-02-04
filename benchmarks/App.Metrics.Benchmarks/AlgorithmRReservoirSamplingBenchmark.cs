// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Scheduling;
using App.Metrics.Scheduling.Abstractions;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks
{
    public class AlgorithmRReservoirSamplingBenchmark : DefaultBenchmarkBase
    {
        private MetricsCoreTestFixture _fixture;
        private DefaultAlgorithmRReservoir _reservoir;
        private IScheduler _scheduler;

        [Setup]
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

        [Cleanup]
        public void Cleanup()
        {
            _scheduler.Dispose();
        }
    }
}