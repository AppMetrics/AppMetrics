// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultGaugeManager : IMeasureGaugeMetrics
    {
        private readonly IAdvancedMetrics _advanced;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultGaugeManager" /> class.
        /// </summary>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="advanced">The advanced metrics manager.</param>
        public DefaultGaugeManager(IAdvancedMetrics advanced, IMetricsRegistry registry)
        {
            _advanced = advanced;
            _registry = registry;
        }

        /// <inheritdoc />
        public void SetValue(GaugeOptions options, Func<double> valueProvider)
        {
            _registry.Gauge(options, () => _advanced.BuildGauge(options, valueProvider));
        }
    }
}