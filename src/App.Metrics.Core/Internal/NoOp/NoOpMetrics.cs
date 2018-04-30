// <copyright file="NoOpMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Filters;
using App.Metrics.Infrastructure;
using App.Metrics.Registry;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;

namespace App.Metrics.Internal.NoOp
{
    internal class NoOpMetrics : IMetrics
    {
        private static readonly IMetricsRegistry Registry = new NullMetricsRegistry();

        public IBuildMetrics Build => new DefaultMetricsBuilderFactory(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir()));

        public IClock Clock => new StopwatchClock();

        public IFilterMetrics Filter => new NullMetricsFilter();

        public IManageMetrics Manage => new DefaultMetricsManager(Registry);

        public IMeasureMetrics Measure => new DefaultMeasureMetricsProvider(Registry, Build, Clock);

        public IProvideMetrics Provider => new DefaultMetricsProvider(Registry, Build, Clock);

        public IProvideMetricValues Snapshot => new DefaultMetricValuesProvider(Filter, Registry);
    }
}