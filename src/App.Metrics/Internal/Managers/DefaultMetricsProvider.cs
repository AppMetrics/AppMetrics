// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Managers
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
            Apdex = new DefaultApdexProvider(buideFactory.Apdex, registry, clock);
            Counter = new DefaultCounterProvider(buideFactory.Counter, registry);
            Gauge = new DefaultGaugeProvider(registry);
            Histogram = new DefaultHistogramProvider(buideFactory.Histogram, registry);
            Meter = new DefaultMeterProvider(buideFactory.Meter, registry, clock);
            Timer = new DefaultTimerProvider(buideFactory.Timer, registry, clock);
        }

        /// <inheritdoc />
        public IProvideApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IProvidCounterMetrics Counter { get; }

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