// <copyright file="BenchmarkTestRunner.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Concurrency.Benchmarks.Support
{
    internal static class BenchmarkTestRunner
    {
        internal static void CanCompileAndRun<TBenchmark>(ITestOutputHelper output)
        {
            var summary = BenchmarkRunner.Run<TBenchmark>(new SingleRunFastConfig(output));

            Assert.True(summary.Reports.Any());
            Assert.True(summary.Reports.All(report => report.ExecuteResults.All(executeResult => executeResult.FoundExecutable)));
            Assert.True(summary.Reports.All(report => report.AllMeasurements.Any()), "There are no available measurements");
        }

        private class SingleRunFastConfig : ManualConfig
        {
            internal SingleRunFastConfig(ITestOutputHelper output)
            {
                AddJob(Job.Dry);
                AddLogger(ConsoleLogger.Default);
                AddLogger(new OutputLogger(output));
            }
        }
    }
}