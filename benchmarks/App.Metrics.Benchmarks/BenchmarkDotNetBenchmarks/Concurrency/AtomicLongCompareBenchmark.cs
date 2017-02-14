using System;
using System.Collections.Generic;
using System.Threading;
using App.Metrics.Concurrency;
using App.Metrics.Concurrency.Internal;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency
{    
    public class AtomicLongCompareBenchmark : DefaultBenchmarkBase
    {
        private const int NumberOfRuns = 50;
        private int _threadCount;

        [Setup]
        public override void Setup()
        {
            _threadCount = Environment.ProcessorCount;
        }

        [Benchmark(Baseline = true)]
        public void AtomicLong()
        {
            Run<AtomicLong>();
        }

        [Benchmark]
        public void PaddedAtomicLong()
        {
            Run<PaddedAtomicLong>();
        }

        [Benchmark]
        public void StripedLongAdder()
        {
            Run<StripedLongAdder>();
        }

        [Benchmark]
        public void ThreadLocalLongAdder()
        {
            Run<ThreadLocalLongAdder>();
        }

        private void Run<T>() where T : IValueAdder<long>, new()
        {
            var value = new T();
            var thread = new List<Thread>();

            for (var i = 0; i < _threadCount; i++)
            {
                thread.Add(new Thread(() =>
                {
                    for (long j = 0; j < NumberOfRuns; j++)
                    {
                        value.Increment();
                        value.Decrement();
                        value.GetAndReset();
                        value.NonVolatileGetValue();
                        value.Increment();
                        value.Decrement();
                    }
                }));
            }

            thread.ForEach(t => t.Start());
            thread.ForEach(t => t.Join());

            var result = value.GetValue();
        }
    }
}