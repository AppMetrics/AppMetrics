// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Configs;
using App.Metrics.Benchmarks.Fixtures;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks
{
    [Config(typeof(DefaultConfig))]
    public abstract class DefaultBenchmarkBase
    {
        public MetricsCoreTestFixture Fixture { get; private set; }

        [Setup]
        public virtual void Setup() { Fixture = new MetricsCoreTestFixture(); }
    }
}