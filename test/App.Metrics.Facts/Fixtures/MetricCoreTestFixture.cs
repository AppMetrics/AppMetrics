// <copyright file="MetricCoreTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.FactsCommon;
using App.Metrics.Filtering;
using App.Metrics.Internal;
using App.Metrics.Registry;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricCoreTestFixture : IDisposable
    {
        public MetricCoreTestFixture()
        {
            var options = new Mock<IOptions<MetricsOptions>>();
            options
                .SetupGet(o => o.Value)
                .Returns(new MetricsOptions());

            Clock = new TestClock();
            Builder = new DefaultMetricsBuilderFactory();

            IMetricContextRegistry ContextRegistrySetup(string context) => new DefaultMetricContextRegistry(context);

            var registry = new DefaultMetricsRegistry(options.Object, Clock, ContextRegistrySetup);

            Registry = registry;
            Providers = new DefaultMetricsProvider(Registry, Builder, Clock);
            Snapshot = new DefaultMetricValuesProvider(new NoOpMetricsFilter(), Registry);
            Managers = new DefaultMeasureMetricsProvider(Registry, Builder, Clock);
            Context = options.Object.Value.DefaultContextLabel;
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