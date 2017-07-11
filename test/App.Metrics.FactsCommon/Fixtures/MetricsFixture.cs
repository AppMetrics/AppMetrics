// <copyright file="MetricsFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.Filtering;
using App.Metrics.Core.Internal;
using App.Metrics.Filters;
using App.Metrics.Registry;
using Microsoft.Extensions.Logging;

namespace App.Metrics.FactsCommon.Fixtures
{
    public class MetricsFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public MetricsFixture()
        {
            Clock = new TestClock();
            var options = new AppMetricsOptions();

            IMetricContextRegistry NewContextRegistry(string name) => new DefaultMetricContextRegistry(name);

            var registry = new DefaultMetricsRegistry(_loggerFactory, options, Clock, NewContextRegistry);
            var metricBuilderFactory = new DefaultMetricsBuilderFactory();
            var filter = new DefaultMetricsFilter();
            var dataManager = new DefaultMetricValuesProvider(filter, registry);
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
                metricsManager);
        }

        public IClock Clock { get; }

        public Func<IMetrics, MetricsDataValueSource> CurrentData =>
            ctx => Metrics.Snapshot.Get();

        public Func<IMetrics, IFilterMetrics, MetricsDataValueSource> CurrentDataWithFilter
            => (ctx, filter) => Metrics.Snapshot.Get(filter);

        public IMetrics Metrics { get; }

        public void Dispose() { Metrics?.Manage.Reset(); }
    }
}