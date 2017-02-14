// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Concurrency;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency
{
    public class AtomicLongArrayBenchmark : DefaultBenchmarkBase
    {
        private AtomicLongArray _num;

        [Setup]
        public override void Setup()
        {
            _num = new AtomicLongArray(10);
        }

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
    }
}