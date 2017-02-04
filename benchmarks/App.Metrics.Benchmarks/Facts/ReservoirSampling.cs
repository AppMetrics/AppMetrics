// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Scheduling;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class ReservoirSampling : IClassFixture<MetricsCoreTestFixture>
    {
        private readonly MetricsCoreTestFixture _fixture;

        public ReservoirSampling(MetricsCoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void AlorithmR() { RunReservoir(new DefaultAlgorithmRReservoir()); }

        [Fact]
        public void ForwardDecaying() { RunReservoir(new DefaultForwardDecayingReservoir()); }

        [Fact]
        public void SlidingWindow() { RunReservoir(new DefaultSlidingWindowReservoir()); }

        private void RunReservoir(IReservoir reservoir)
        {
            var scheduler = new DefaultTaskScheduler();

            scheduler.Interval(
                TimeSpan.FromMilliseconds(20),
                TaskCreationOptions.None,
                () =>
                {
                    reservoir.GetSnapshot();
                    reservoir.Reset();
                });

            SimpleBenchmarkRunner.Run(
                () => { reservoir.Update(_fixture.Rnd.Next(0, 1000), _fixture.RandomUserValue); });

            scheduler.Dispose();
        }
    }
}