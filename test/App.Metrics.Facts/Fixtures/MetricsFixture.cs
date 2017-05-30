// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Core.Internal;
using App.Metrics.Filtering;
using App.Metrics.Health.Abstractions;
using App.Metrics.Health.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Registry.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricsFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public MetricsFixture()
        {
            var healthFactoryLogger = _loggerFactory.CreateLogger<HealthCheckFactory>();
            Clock = new TestClock();
            var options = new AppMetricsOptions();

            IMetricContextRegistry NewContextRegistry(string name) => new DefaultMetricContextRegistry(name);

            var registry = new DefaultMetricsRegistry(_loggerFactory, options, Clock, new EnvironmentInfoProvider(), NewContextRegistry);
            HealthCheckFactory = new HealthCheckFactory(healthFactoryLogger, new Lazy<IMetrics>(() => Metrics));
            var metricBuilderFactory = new DefaultMetricsBuilderFactory();
            var filter = new DefaultMetricsFilter();
            var dataManager = new DefaultMetricValuesProvider(filter, registry);
            var healthStatusProvider = new DefaultHealthProvider(new Lazy<IMetrics>(() => Metrics), _loggerFactory.CreateLogger<DefaultHealthProvider>(), HealthCheckFactory);
            var metricsManagerFactory = new DefaultMeasureMetricsProvider(registry, metricBuilderFactory, Clock);
            var metricsManagerAdvancedFactory = new DefaultMetricsProvider(registry, metricBuilderFactory, Clock);
            var metricsManager = new DefaultMetricsManager(registry, _loggerFactory.CreateLogger<DefaultMetricsManager>());
            Metrics = new DefaultMetrics(
                Clock,
                filter,
                metricsManagerFactory,
                metricBuilderFactory,
                metricsManagerAdvancedFactory,
                dataManager,
                metricsManager,
                healthStatusProvider);
        }

        public Func<IMetrics, MetricsDataValueSource> CurrentData =>
            ctx => Metrics.Snapshot.Get();

        public Func<IMetrics, IFilterMetrics, MetricsDataValueSource> CurrentDataWithFilter
            => (ctx, filter) => Metrics.Snapshot.Get(filter);

        public IMetrics Metrics { get; }

        public IHealthCheckFactory HealthCheckFactory { get; }

        public IClock Clock { get; }

        public void Dispose()
        {
            Metrics?.Manage.Reset();
        }
    }
}