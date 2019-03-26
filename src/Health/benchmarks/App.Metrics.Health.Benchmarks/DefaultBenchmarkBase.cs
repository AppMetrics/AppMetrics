// <copyright file="DefaultBenchmarkBase.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Health.Benchmarks.Configs;
using App.Metrics.Health.Benchmarks.Fixtures;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Health.Benchmarks
{
    [Config(typeof(DefaultConfig))]
    public abstract class DefaultBenchmarkBase
    {
        protected HealthTestFixture FixtureWithHealth { get; private set; }

        [GlobalSetup]
        public virtual void Setup()
        {
            FixtureWithHealth = new HealthTestFixture();
        }
    }
}