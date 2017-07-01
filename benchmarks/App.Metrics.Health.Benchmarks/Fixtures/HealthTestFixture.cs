// <copyright file="HealthTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Health.Benchmarks.Support;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Benchmarks.Fixtures
{
    public class HealthTestFixture : IDisposable
    {
        public HealthTestFixture()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services
                .AddHealthChecks()
                .AddChecks((registry, metrics) =>
                {
                        registry.AddMetricCheck(
                            "counter metric check",
                            metrics,
                            MetricOptions.Counter.Options,
                            passing: value => { return (message: $"OK. < 5 ({value.Count})", result: value.Count < 5); },
                            warning: value => { return (message: $"WARNING. < 7 ({value.Count})", result: value.Count < 10); },
                            failing: value => { return (message: $"FAILED. >=7 ({value.Count})", result: value.Count >= 10); });
                    });

            services.AddMetrics();

            var provider = services.BuildServiceProvider();

            var metricsInstance = provider.GetRequiredService<IMetrics>();
            Health = provider.GetRequiredService<IProvideHealth>();

            metricsInstance.Measure.Counter.Increment(MetricOptions.Counter.Options);
        }

        public IProvideHealth Health { get; }

        public void Dispose() { }
    }
}
