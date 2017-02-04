// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;

namespace App.Metrics.Benchmarks.Support
{
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
}