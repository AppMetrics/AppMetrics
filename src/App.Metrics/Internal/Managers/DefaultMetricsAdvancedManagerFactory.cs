// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultMetricsAdvancedManagerFactory : IMetricsAdvancedManagerFactory
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetricsAdvancedManagerFactory" /> class.
        /// </summary>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="buideFactory">The buide factory.</param>
        /// <param name="clock">The clock.</param>
        public DefaultMetricsAdvancedManagerFactory(IMetricsRegistry registry, IBuildMetrics buideFactory, IClock clock)
        {
            Apdex = new DefaultApdexAdvancedManager(buideFactory.Apdex, registry, clock);
            Counter = new DefaultCounterAdvancedManager(buideFactory.Counter, registry);
            Gauge = new DefaultGaugeAdvancedManager(registry);
            Histogram = new DefaultHistogramAdvancedManager(buideFactory.Histogram, registry);
            Meter = new DefaultMeterAdvancedManager(buideFactory.Meter, registry, clock);
            Timer = new DefaultTimerAdvancedManager(buideFactory.Timer, registry, clock);
        }

        /// <inheritdoc />
        public IMeasureApdexMetricsAdvanced Apdex { get; }

        /// <inheritdoc />
        public IMeasureCounterMetricsAdvanced Counter { get; }

        /// <inheritdoc />
        public IMeasureGaugeMetricsAdvanced Gauge { get; }

        /// <inheritdoc />
        public IMeasureHistogramMetricsAdvanced Histogram { get; }

        /// <inheritdoc />
        public IMeasureMeterMetricsAdvanced Meter { get; }

        /// <inheritdoc />
        public IMeasureTimerMetricsAdvanced Timer { get; }
    }
}