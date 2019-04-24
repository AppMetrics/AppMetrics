// <copyright file="DefaultGaugeMetricProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Registry;

namespace App.Metrics.Gauge
{
    public class DefaultGaugeMetricProvider : IProvideGaugeMetrics
    {
        private readonly IBuildGaugeMetrics _gaugeBuilder;
        private readonly IMetricsRegistry _registry;

        public DefaultGaugeMetricProvider(IBuildGaugeMetrics gaugeBuilder, IMetricsRegistry registry)
        {
            _gaugeBuilder = gaugeBuilder;
            _registry = registry;
        }

        /// <inheritdoc />
        public IGauge Instance(GaugeOptions options)
        {
            return Instance(options, () => _gaugeBuilder.Build());
        }

        /// <inheritdoc />
        public IGauge Instance(GaugeOptions options, MetricTags tags)
        {
            return Instance(options, tags, () => _gaugeBuilder.Build());
        }

        /// <inheritdoc />
        public IGauge Instance<T>(GaugeOptions options, Func<T> builder)
            where T : IGaugeMetric
        {
            return _registry.Gauge(options, builder);
        }

        /// <inheritdoc />
        public IGauge Instance<T>(GaugeOptions options, MetricTags tags, Func<T> builder)
            where T : IGaugeMetric
        {
            return _registry.Gauge(options, tags, builder);
        }
    }
}