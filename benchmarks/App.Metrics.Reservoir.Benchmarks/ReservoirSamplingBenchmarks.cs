// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using App.Metrics.Reservoir.Benchmarks.Jobs;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Reservoir.Benchmarks
{
    [MediumRunJob]
    public class ReservoirSamplingBenchmarks
    {
        private const double ExponentialDecayFactor = 0.015;
        private const int NumberOfRuns = 100;
        private const int SampleSize = 100;
        private const int ThreadCount = 8;

        [Benchmark]
        public void ExponentiallyDecayingReservoir()
        {
            var reservoir = new DefaultForwardDecayingReservoir(SampleSize, ExponentialDecayFactor);
            Run(reservoir);
        }

        [Benchmark]
        public void SlidingWindowReservoir()
        {
            var reservoir = new DefaultSlidingWindowReservoir(SampleSize);
            Run(reservoir);
        }

        [Benchmark(Baseline = true)]
        public void UniformReservoir()
        {
            var reservoir = new DefaultAlgorithmRReservoir(SampleSize);
            Run(reservoir);
        }

        private static void Run(IReservoir reservoir)
        {
            var thread = new List<Thread>();

            for (var i = 0; i < ThreadCount; i++)
            {
                thread.Add(
                    new Thread(
                        () =>
                        {
                            for (long j = 0; j < NumberOfRuns; j++)
                            {
                                reservoir.Update(1, $"user-value-{j}");

                                if (j % 4 == 0)
                                {
                                    reservoir.GetSnapshot();
                                }

                                if (j == NumberOfRuns / 2)
                                {
                                    reservoir.Reset();
                                }
                            }
                        }));
            }

            thread.ForEach(t => t.Start());
            thread.ForEach(t => t.Join());
        }
    }
}