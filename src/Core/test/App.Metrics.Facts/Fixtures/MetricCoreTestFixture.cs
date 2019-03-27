// <copyright file="MetricCoreTestFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.FactsCommon;
using App.Metrics.Internal;
using App.Metrics.Internal.NoOp;
using App.Metrics.Registry;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricCoreTestFixture : IDisposable
    {
        public MetricCoreTestFixture()
        {
            var options = new MetricsOptions();
            Clock = new TestClock();
            Builder = new DefaultMetricsBuilderFactory(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir()));

            IMetricContextRegistry ContextRegistrySetup(string context) => new DefaultMetricContextRegistry(context);

            var registry = new DefaultMetricsRegistry(options.DefaultContextLabel, Clock, ContextRegistrySetup);

            Registry = registry;
            Providers = new DefaultMetricsProvider(Registry, Builder, Clock);
            Snapshot = new DefaultMetricValuesProvider(new NullMetricsFilter(), Registry);
            Managers = new DefaultMeasureMetricsProvider(Registry, Builder, Clock);
            Context = options.DefaultContextLabel;
        }

        public IBuildMetrics Builder { get; }

        public IClock Clock { get; }

        public string Context { get; }

        public IMeasureMetrics Managers { get; }

        public IProvideMetrics Providers { get; }

        public IMetricsRegistry Registry { get; }

        public IProvideMetricValues Snapshot { get; }

        public MetricTags[] Tags => new[]
                                    {
                                        new MetricTags("key1", "key2"),
                                        new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" })
                                    };

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}