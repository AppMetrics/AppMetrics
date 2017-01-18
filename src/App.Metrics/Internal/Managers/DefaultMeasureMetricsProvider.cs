// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Managers
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

        public IMeasureApdexMetrics Apdex { get; }

        public IMeasureCounterMetrics Counter { get; }

        public IMeasureGaugeMetrics Gauge { get; }

        public IMeasureHistogramMetrics Histogram { get; }

        public IMeasureMeterMetrics Meter { get; }

        public IMeasureTimerMetrics Timer { get; }
    }
}