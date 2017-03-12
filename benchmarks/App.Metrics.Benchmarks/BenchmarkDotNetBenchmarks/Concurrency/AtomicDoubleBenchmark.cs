// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Concurrency;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency
{
    public class AtomicDoubleBenchmark : DefaultBenchmarkBase
    {
        private AtomicDouble _num;

        [Setup]
        public override void Setup()
        {
            _num = new AtomicDouble();
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