// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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