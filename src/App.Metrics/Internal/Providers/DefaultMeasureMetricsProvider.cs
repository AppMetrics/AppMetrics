// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;
using App.Metrics.Internal.Managers;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Providers
{
    internal class DefaultMeasureMetricsProvider : IMeasureMetrics
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMeasureMetricsProvider" /> class.
        /// </summary>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="buideFactory">The buide factory.</param>
        /// <param name="clock">The clock.</param>
        public DefaultMeasureMetricsProvider(IMetricsRegistry registry, IBuildMetrics buideFactory, IClock clock)
        {
            Apdex = new DefaultApdexManager(buideFactory.Apdex, registry, clock);
            Counter = new DefaultCounterManager(buideFactory.Counter, registry);
            Gauge = new DefaultGaugeManager(buideFactory.Gauge, registry);
            Histogram = new DefaultHistogramManager(buideFactory.Histogram, registry);
            Meter = new DefaultMeterManager(buideFactory.Meter, registry, clock);
            Timer = new DefaultTimerManager(buideFactory.Timer, registry, clock);
        }

        /// <inheritdoc />
        public IMeasureApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IMeasureCounterMetrics Counter { get; }

        /// <inheritdoc />
        public IMeasureGaugeMetrics Gauge { get; }

        /// <inheritdoc />
        public IMeasureHistogramMetrics Histogram { get; }

        /// <inheritdoc />
        public IMeasureMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IMeasureTimerMetrics Timer { get; }
    }
}