using App.Metrics.Concurrency.Benchmarks.Jobs;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Concurrency.Benchmarks.Benchmarks
{
    [FastAndDirtyJob]
    public class AtomicLongArrayBenchMark
    {
        private AtomicLongArray _num;

        [Benchmark]
        public void Decrement()
        {
            _num.Decrement(1);
        }

        [Benchmark]
        public void Get()
        {
            var x = _num.GetValue(1);
        }

        [Benchmark]
        public void Increment()
        {
            _num.Increment(1);
        }

        [Setup]
        public void Setup()
        {
            _num = new AtomicLongArray(10);
        }
    }
}