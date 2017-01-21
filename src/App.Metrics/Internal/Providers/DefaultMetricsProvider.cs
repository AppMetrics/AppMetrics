// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions;
using App.Metrics.Interfaces;

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
        public IProviderMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IProvideTimerMetrics Timer { get; }
    }
}