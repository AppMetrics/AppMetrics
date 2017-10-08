// <copyright file="TestConfigs.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;

// ReSharper disable UnusedMember.Global
namespace App.Metrics.Benchmarks.Support
{
#pragma warning disable SA1649, SA1402
    public class SingleRunFastConfig : ManualConfig
    {
        public SingleRunFastConfig() { Add(Job.Dry); }
    }

    public class SingleRunMediumConfig : ManualConfig
    {
        public SingleRunMediumConfig() { Add(new Job(Job.Dry) { Run = { TargetCount = 5 } }); }
    }

    public class ThroughputFastConfig : ManualConfig
    {
        public ThroughputFastConfig() { Add(new Job(Job.Dry) { Run = { RunStrategy = RunStrategy.Throughput, TargetCount = 1 } }); }
    }

    public class DiagnoserConfig : ManualConfig
    {
        public DiagnoserConfig()
        {
            // Diagnosers need enough runs to collects the statistics!
            Add(new Job { Run = { LaunchCount = 1, WarmupCount = 1, TargetCount = 50 } });
        }
    }
#pragma warning restore SA1649, SA1402
    // ReSharper restore UnusedMember.Global
}