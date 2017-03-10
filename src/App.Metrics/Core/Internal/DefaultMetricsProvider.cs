// <copyright file="DefaultMetricsProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Interfaces;
using App.Metrics.Counter;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge;
using App.Metrics.Gauge.Abstractions;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Core.Internal
{
    internal sealed class DefaultMetricsProvider : IProvideMetrics
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetricsProvider" /> class.
        /// </summary>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="builderFactory">The buide factory.</param>
        /// <param name="clock">The clock.</param>
        public DefaultMetricsProvider(IMetricsRegistry registry, IBuildMetrics builderFactory, IClock clock)
        {
            Apdex = new DefaultApdexMetricProvider(builderFactory.Apdex, registry, clock);
            Counter = new DefaultCounterMetricProvider(builderFactory.Counter, registry);
            Gauge = new DefaultGaugeMetricProvider(builderFactory.Gauge, registry);
            Histogram = new DefaultHistogramMetricProvider(builderFactory.Histogram, registry);
            Meter = new DefaultMeterMetricProvider(builderFactory.Meter, registry, clock);
            Timer = new DefaultTimerMetricProvider(builderFactory.Timer, registry, clock);
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