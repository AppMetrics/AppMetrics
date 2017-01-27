// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Configuration;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Registry.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricCoreTestFixture : IDisposable
    {
        public MetricCoreTestFixture()
        {
            var loggerFactory = new LoggerFactory();
            var options = new AppMetricsOptions();

            Clock = new TestClock();
            Builder = new DefaultMetricsBuilderFactory();

            Func<string, IMetricContextRegistry> contextRegistrySetup = context => new DefaultMetricContextRegistry(context);
            var registry = new DefaultMetricsRegistry(loggerFactory, options, Clock, new EnvironmentInfoProvider(), contextRegistrySetup);

            Registry = registry;
            Providers = new DefaultMetricsProvider(Registry, Builder, Clock);
            Snapshot = new DefaultMetricValuesProvider(new NoOpMetricsFilter(), Registry);
            Managers = new DefaultMeasureMetricsProvider(Registry, Builder, Clock);
            Context = options.DefaultContextLabel;
        }

        public IBuildMetrics Builder { get; }

        public IClock Clock { get; }

        public IMeasureMetrics Managers { get; }

        public IProvideMetrics Providers { get; }

        public IMetricsRegistry Registry { get; }

        public IProvideMetricValues Snapshot { get; }

        public string Context { get; }

        /// <inheritdoc />
        public void Dispose() { }
    }
}