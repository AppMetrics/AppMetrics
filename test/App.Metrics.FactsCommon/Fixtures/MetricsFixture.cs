// <copyright file="MetricsFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Internal;
using App.Metrics.Registry;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;

namespace App.Metrics.FactsCommon.Fixtures
{
    public class MetricsFixture : IDisposable
    {
        public MetricsFixture()
        {
            Clock = new TestClock();
            var options = new MetricsOptions();

            IMetricContextRegistry NewContextRegistry(string name) => new DefaultMetricContextRegistry(name);

            var registry = new DefaultMetricsRegistry(options.DefaultContextLabel, Clock, NewContextRegistry);
            var metricBuilderFactory = new DefaultMetricsBuilderFactory(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir()));
            var filter = new MetricsFilter();
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