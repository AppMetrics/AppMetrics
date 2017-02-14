// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Concurrency;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency
{
    public class AtomicIntegerBenchmark : DefaultBenchmarkBase
    {
        private AtomicInteger _num;

        [Setup]
        public override void Setup()
        {
            _num = new AtomicInteger();
        }

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
    }
}