// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Configuration;
using App.Metrics.Core.Interfaces;
using App.Metrics.Infrastructure;
using App.Metrics.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricManagerTestFixture : IDisposable
    {
        public MetricManagerTestFixture()
        {
            var loggerFactory = new LoggerFactory();
            var clock = new TestClock();
            var filter = new DefaultMetricsFilter();
            var options = new AppMetricsOptions();

            var healthCheckFactory = new HealthCheckFactory(loggerFactory.CreateLogger<HealthCheckFactory>());
            Func<string, IMetricContextRegistry> contextRegistrySetup = context => new DefaultMetricContextRegistry(context);
            var registry = new DefaultMetricsRegistry(loggerFactory, options, clock, new EnvironmentInfoProvider(), contextRegistrySetup);

            Advanced = new DefaultAdvancedMetrics(
                loggerFactory.CreateLogger<DefaultAdvancedMetrics>(),
                options,
                clock,
                filter,
                registry,
                healthCheckFactory);

            Registry = registry;
        }

        public IAdvancedMetrics Advanced { get; }

        public IMetricsRegistry Registry { get; }

        /// <inheritdoc />
        public void Dispose() { }
    }
}