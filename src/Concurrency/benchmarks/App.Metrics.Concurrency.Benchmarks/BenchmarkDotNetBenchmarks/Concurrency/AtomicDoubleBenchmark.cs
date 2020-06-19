// <copyright file="AtomicDoubleBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using BenchmarkDotNet.Attributes;

namespace App.Metrics.Concurrency.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency
{
    public class AtomicDoubleBenchmark : DefaultBenchmarkBase
    {
        private AtomicDouble _num;

        public override void Setup()
        {
            _num = new AtomicDouble(0);
        }

        [Benchmark]
        public void Decrement()
        {
            _num.Decrement();
        }

        [Benchmark]
        public void Get()
        {
            // ReSharper disable UnusedVariable
            var x = _num.GetValue();
            // ReSharper restore UnusedVariable
        }

        [Benchmark]
        public void Increment()
        {
            _num.Increment();
        }

        [Benchmark]
        public void IncrementValue()
        {
            _num.Increment(2);
        }
    }
}