// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex;
using App.Metrics.Apdex.Interfaces;
using App.Metrics.Counter;
using App.Metrics.Counter.Interfaces;
using App.Metrics.Gauge;
using App.Metrics.Gauge.Interfaces;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Meter;
using App.Metrics.Meter.Interfaces;
using App.Metrics.Registry.Interfaces;
using App.Metrics.Timer;
using App.Metrics.Timer.Interfaces;

namespace App.Metrics.Internal.Providers
{
    internal class DefaultMetricsProvider : IProvideMetrics
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetricsProvider" /> class.
        /// </summary>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="buideFactory">The buide factory.</param>
        /// <param name="clock">The clock.</param>
        public DefaultMetricsProvider(IMetricsRegistry registry, IBuildMetrics buideFactory, IClock clock)
        {
            Apdex = new DefaultApdexMetricProvider(buideFactory.Apdex, registry, clock);
            Counter = new DefaultCounterMetricProvider(buideFactory.Counter, registry);
            Gauge = new DefaultGaugeMetricProvider(registry);
            Histogram = new DefaultHistogramMetricProvider(buideFactory.Histogram, registry);
            Meter = new DefaultMeterMetricProvider(buideFactory.Meter, registry, clock);
            Timer = new DefaultTimerMetricProvider(buideFactory.Timer, registry, clock);
        }

        /// <inheritdoc />
        public IProvideApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IProvideCounterMetrics Counter { get; }

        /// <inheritdoc />
        public IProvideGaugeMetrics Gauge { get; }

        /// <inheritdoc />
        public IProvideHistogramMetrics Histogram { get; }

        /// <inheritdoc />
        public IProvideMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IProvideTimerMetrics Timer { get; }
    }
}