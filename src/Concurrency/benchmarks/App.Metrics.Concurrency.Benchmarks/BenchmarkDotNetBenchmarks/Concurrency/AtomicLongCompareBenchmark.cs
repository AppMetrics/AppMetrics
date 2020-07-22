// <copyright file="AtomicLongCompareBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using BenchmarkDotNet.Attributes;

namespace App.Metrics.Concurrency.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency
{
    public class AtomicLongCompareBenchmark : DefaultBenchmarkBase
    {
        private AtomicLong _atomicLong;
        private PaddedAtomicLong _paddedAtomicLong;
        private StripedLongAdder _stripedLongAdder;
        private ThreadLocalLongAdder _threadLocalLongAdder;

        public override void Setup()
        {
            _atomicLong = new AtomicLong();
            _paddedAtomicLong = new PaddedAtomicLong();
            _stripedLongAdder = new StripedLongAdder();
            _threadLocalLongAdder = new ThreadLocalLongAdder();
        }

        [Benchmark(Baseline = true)]
        public void AtomicLong()
        {
            _atomicLong.Increment();
            _atomicLong.Decrement();
            _atomicLong.GetAndReset();
            _atomicLong.NonVolatileGetValue();
            _atomicLong.Increment();
            _atomicLong.Decrement();
        }

        [Benchmark]
        public void PaddedAtomicLong()
        {
            _paddedAtomicLong.Increment();
            _paddedAtomicLong.Decrement();
            _paddedAtomicLong.GetAndReset();
            _paddedAtomicLong.NonVolatileGetValue();
            _paddedAtomicLong.Increment();
            _paddedAtomicLong.Decrement();
        }

        [Benchmark]
        public void StripedLongAdder()
        {
            _stripedLongAdder.Increment();
            _stripedLongAdder.Decrement();
            _stripedLongAdder.GetAndReset();
            _stripedLongAdder.NonVolatileGetValue();
            _stripedLongAdder.Increment();
            _stripedLongAdder.Decrement();
        }

        [Benchmark]
        public void ThreadLocalLongAdder()
        {
            _threadLocalLongAdder.Increment();
            _threadLocalLongAdder.Decrement();
            _threadLocalLongAdder.GetAndReset();
            _threadLocalLongAdder.NonVolatileGetValue();
            _threadLocalLongAdder.Increment();
            _threadLocalLongAdder.Decrement();
        }
    }
}