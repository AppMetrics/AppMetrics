using System.Collections.Generic;
using System.Threading;
using App.Metrics.Concurrency.Benchmarks.Jobs;
using App.Metrics.Concurrency.Internal;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Concurrency.Benchmarks
{
    [QuickRunJob]
    public class AtomicLongCompareBenchmark
    {
        private const int NumberOfRuns = 100;
        private const int ThreadCount = 8;

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

        private static void Run<T>() where T : IValueAdder<long>, new()
        {
            var value = new T();
            var thread = new List<Thread>();

            for (var i = 0; i < ThreadCount; i++)
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