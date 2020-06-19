// <copyright file="DefaultConfig.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace App.Metrics.Concurrency.Benchmarks.Configs
{
    public class DefaultConfig : ManualConfig
    {
        public DefaultConfig()
        {
            AddJob(Job.Default.WithRuntime(CoreRuntime.Core31));
            AddExporter(MarkdownExporter.GitHub);
            AddDiagnoser(MemoryDiagnoser.Default);
        }
    }
}