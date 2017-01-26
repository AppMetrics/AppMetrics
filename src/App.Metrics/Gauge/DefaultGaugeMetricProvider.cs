// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Gauge.Abstractions;
using App.Metrics.Registry.Abstractions;

namespace App.Metrics.Gauge
{
    public class DefaultGaugeMetricProvider : IProvideGaugeMetrics
    {
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultGaugeMetricProvider" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public DefaultGaugeMetricProvider(IMetricsRegistry registry) { _registry = registry; }

        /// <inheritdoc />
        public void Instance(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider) { _registry.Gauge(options, valueProvider); }
    }
}