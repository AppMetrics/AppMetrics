// <copyright file="DefaultBenchmarkBase.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Concurrency.Benchmarks.Configs;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Concurrency.Benchmarks
{
    [Config(typeof(DefaultConfig))]
    public abstract class DefaultBenchmarkBase
    {
        [GlobalSetup]
        public virtual void Setup()
        {
        }
    }
}