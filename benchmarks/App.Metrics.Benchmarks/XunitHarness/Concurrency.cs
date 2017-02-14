// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency;
using BenchmarkDotNet.Running;
using Xunit;

namespace App.Metrics.Benchmarks.XunitHarness
{
    public class Concurrency
    {
        [Fact]
        public void AtomicIntegerBenchmark()
        {
            BenchmarkRunner.Run<AtomicIntegerBenchmark>();
        }

        [Fact]
        public void AtomicLongArrayBenchmark()
        {
            BenchmarkRunner.Run<AtomicLongArrayBenchmark>();
        }

        [Fact]
        public void AtomicLongBenchmark()
        {
            BenchmarkRunner.Run<AtomicLongBenchmark>();
        }

        [Fact]
        public void AtomicLongCompareBenchmark()
        {
            BenchmarkRunner.Run<AtomicLongCompareBenchmark>();
        }

        [Fact]
        public void PaddedAtomicLongBenchmark()
        {
            BenchmarkRunner.Run<PaddedAtomicLongBenchmark>();
        }

        [Fact]
        public void StripedLongAdderBenchmark()
        {
            BenchmarkRunner.Run<StripedLongAdderBenchmark>();
        }

        [Fact]
        public void ThreadLocalLongAdderBenchmark()
        {
            BenchmarkRunner.Run<ThreadLocalLongAdderBenchmark>();
        }
    }
}