// <copyright file="MetricsFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Internal;
using App.Metrics.Registry;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace App.Metrics.FactsCommon.Fixtures
{
    public class MetricsFixture : IDisposable
    {
        public MetricsFixture()
        {
            Clock = new TestClock();
            var options = new Mock<IOptions<MetricsOptions>>();
            options
                .SetupGet(o => o.Value)
                .Returns(new MetricsOptions());

            IMetricContextRegistry NewContextRegistry(string name) => new DefaultMetricContextRegistry(name);

            var registry = new DefaultMetricsRegistry(options.Object, Clock, NewContextRegistry);
            var metricBuilderFactory = new DefaultMetricsBuilderFactory();
            var filter = new DefaultMetricsFilter();
            var dataManager = new DefaultMetricValuesProvider(filter, registry);
            var metricsManagerFactory = new DefaultMeasureMetricsProvider(registry, metricBuilderFactory, Clock);
            var metricsManagerAdvancedFactory = new DefaultMetricsProvider(registry, metricBuilderFactory, Clock);
            var metricsManager = new DefaultMetricsManager(registry);
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