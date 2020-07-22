// <copyright file="Concurrency.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Concurrency.Benchmarks.BenchmarkDotNetBenchmarks.Concurrency;
using BenchmarkDotNet.Running;
using Xunit;

namespace App.Metrics.Concurrency.Benchmarks.XunitHarness
{
    public class Concurrency
    {
        [Fact]
        public void AtomicDoubleBenchmark()
        {
            BenchmarkRunner.Run<AtomicDoubleBenchmark>();
        }

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