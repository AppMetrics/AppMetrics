using BenchmarkDotNet.Running;
using Xunit;

namespace App.Metrics.Concurrency.Benchmarks.Harness
{
    public class BenchmarkHarness
    {
        [Fact, Trait("MediumRun", "AtomicLongCompare")]
        public void AtomicLongCompareBenchmark()
        {
            BenchmarkRunner.Run<AtomicLongCompareBenchmark>();
        }

        [Fact, Trait("QuickRun", "AtomicInt")]
        public void AtomicIntegerBenchmark()
        {
            BenchmarkRunner.Run<AtomicIntegerBenchmark>();
        }

        [Fact, Trait("QuickRun", "LongArray")]
        public void AtomicLongArrayBenchmark()
        {
            BenchmarkRunner.Run<AtomicLongArrayBenchmark>();
        }

        [Fact, Trait("QuickRun", "AtomicLong")]
        public void AtomicLongBenchmark()
        {
            BenchmarkRunner.Run<AtomicLongBenchmark>();
        }

        [Fact, Trait("QuickRun", "PaddedAtomicLong")]
        public void PaddedAtomicLongBenchmark()
        {
            BenchmarkRunner.Run<PaddedAtomicLongBenchmark>();
        }

        [Fact, Trait("QuickRun", "StripedLongAdder")]
        public void StripedLongAdderBenchmark()
        {
            BenchmarkRunner.Run<StripedLongAdderBenchmark>();
        }

        [Fact, Trait("QuickRun", "ThreadLocalLongAdder")]
        public void ThreadLocalLongAdderBenchmark()
        {
            BenchmarkRunner.Run<ThreadLocalLongAdderBenchmark>();
        }
    }
}