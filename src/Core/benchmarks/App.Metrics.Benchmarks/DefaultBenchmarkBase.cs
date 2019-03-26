// <copyright file="DefaultBenchmarkBase.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Configs;
using App.Metrics.Benchmarks.Fixtures;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks
{
    [Config(typeof(DefaultConfig))]
    public abstract class DefaultBenchmarkBase
    {
        protected MetricsCoreTestFixture Fixture { get; private set; }

        [GlobalSetup]
        public virtual void Setup()
        {
            Fixture = new MetricsCoreTestFixture();
        }
    }
}