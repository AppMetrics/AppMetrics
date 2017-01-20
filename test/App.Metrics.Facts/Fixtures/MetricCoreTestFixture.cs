// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Configuration;
using App.Metrics.Infrastructure;
using App.Metrics.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Internal.Builders;
using App.Metrics.Internal.Providers;
using App.Metrics.Utils;
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

            Managers = new DefaultMeasureMetricsProvider(Registry, Builder, Clock);
        }

        public IBuildMetrics Builder { get; }

        public IClock Clock { get; }

        public IMeasureMetrics Managers { get; }

        public IProvideMetrics Providers { get; }

        public IMetricsRegistry Registry { get; }

        /// <inheritdoc />
        public void Dispose() { }
    }
}