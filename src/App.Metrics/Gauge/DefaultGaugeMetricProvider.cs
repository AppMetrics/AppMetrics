// <copyright file="DefaultGaugeMetricProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;
using App.Metrics.Gauge.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;

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