// <copyright file="DefaultConfig.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace App.Metrics.Health.Benchmarks.Configs
{
    public class DefaultConfig : ManualConfig
    {
        public DefaultConfig()
        {
            Add(Job.Core);
            Add(MarkdownExporter.GitHub);
            Add(new MemoryDiagnoser());
        }
    }
}