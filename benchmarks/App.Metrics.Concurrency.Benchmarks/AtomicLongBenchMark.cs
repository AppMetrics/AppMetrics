using App.Metrics.Concurrency.Benchmarks.Jobs;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Concurrency.Benchmarks
{
    [QuickRunJob]
    public class AtomicLongBenchmark
    {
        private AtomicLong _num;

        [Benchmark]
        public void Decrement()
        {
            _num.Decrement();
        }

        [Benchmark]
        public void Get()
        {
            var x = _num.GetValue();
        }

        [Benchmark]
        public void Increment()
        {
            _num.Increment();
        }

        [Setup]
        public void Setup()
        {
            _num = new AtomicLong();
        }
    }
}