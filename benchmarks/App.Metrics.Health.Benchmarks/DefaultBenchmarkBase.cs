// <copyright file="DefaultBenchmarkBase.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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