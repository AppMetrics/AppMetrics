using System.Collections.Generic;
using System.Threading;
using App.Metrics.Reservoir.Benchmarks.Jobs;
using App.Metrics.Sampling;
using App.Metrics.Sampling.Interfaces;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Reservoir.Benchmarks
{
    [MediumRunJob]
    public class ReservoirSamplingBenchmarks
    {
        private const int NumberOfRuns = 100;
        private const int SampleSize = 100;
        private const double ExponentialDecayFactor = 0.015;
        private const int ThreadCount = 8;

        [Benchmark(Baseline = true)]
        public void UniformReservoir()
        {
            var reservoir = new UniformReservoir(SampleSize);
            Run(reservoir);
        }

        [Benchmark]
        public void SlidingWindowReservoir()
        {
            var reservoir = new SlidingWindowReservoir(SampleSize);
            Run(reservoir);
        }

        [Benchmark]
        public void ExponentiallyDecayingReservoir()
        {
            var reservoir = new ExponentiallyDecayingReservoir(SampleSize, ExponentialDecayFactor);
            Run(reservoir);
        }

        [Benchmark]
        public void HdrHistogramReservoir()
        {
            var reservoir = new HdrHistogramReservoir();
            Run(reservoir);
        }

        private static void Run(IReservoir reservoir)
        {
            var thread = new List<Thread>();

            for (var i = 0; i < ThreadCount; i++)
            {
                thread.Add(new Thread(() =>
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