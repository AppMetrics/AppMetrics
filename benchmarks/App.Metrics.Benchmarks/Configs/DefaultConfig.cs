// <copyright file="DefaultConfig.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace App.Metrics.Benchmarks.Configs
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