// <copyright file="ReservoirSampling.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Sampling;
using App.Metrics.Benchmarks.Support;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.XunitHarness
{
    public class ReservoirSampling
    {
        private readonly ITestOutputHelper _output;

        public ReservoirSampling(ITestOutputHelper output) { _output = output; }

        [Fact]
        public void AlgorithmRReservoir() { BenchmarkTestRunner.CanCompileAndRun<AlgorithmRReservoirSamplingBenchmark>(_output); }

        [Fact]
        public void ForwardDecayReservoir() { BenchmarkTestRunner.CanCompileAndRun<ForwardDecayReservoirSamplingBenchmark>(_output); }

        [Fact]
        public void SlidingWindowReservoir() { BenchmarkTestRunner.CanCompileAndRun<SlidingWindowReservoirSamplingBenchmark>(_output); }
    }
}