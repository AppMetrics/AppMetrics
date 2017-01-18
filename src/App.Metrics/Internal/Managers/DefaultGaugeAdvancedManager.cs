// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Options;
using App.Metrics.Data.Interfaces;
using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Managers
{
    public class DefaultGaugeAdvancedManager : IMeasureGaugeMetricsAdvanced
    {
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultGaugeAdvancedManager" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public DefaultGaugeAdvancedManager(IMetricsRegistry registry) { _registry = registry; }

        /// <inheritdoc />
        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider) { _registry.Gauge(options, valueProvider); }
    }
}