using App.Metrics.Concurrency.Benchmarks.Benchmarks;
using BenchmarkDotNet.Running;
using Xunit;

namespace App.Metrics.Concurrency.Benchmarks
{
    public class BenchmarkHarness
    {
        [Fact]
        public void AtomicIntegerBenchmarks()
        {
            BenchmarkRunner.Run<AtomicIntegerBenchmark>();
        }

        [Fact]
        public void AtomicLongArrayBenchMark()
        {
            BenchmarkRunner.Run<AtomicLongArrayBenchMark>();
        }

        [Fact]
        public void AtomicLongBenchMark()
        {
            BenchmarkRunner.Run<AtomicLongBenchMark>();
        }

        [Fact]
        public void PaddedAtomicLongBenchMark()
        {
            BenchmarkRunner.Run<PaddedAtomicLongBenchMark>();
        }

        [Fact]
        public void StripedLongAdderBenchMark()
        {
            BenchmarkRunner.Run<StripedLongAdderBenchMark>();
        }

        [Fact]
        public void ThreadLocalLongAdderBenchMark()
        {
            BenchmarkRunner.Run<ThreadLocalLongAdderBenchMark>();
        }
    }
}