// <copyright file="DefaultMetricsBuilderFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Abstractions;
using App.Metrics.Counter;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge;
using App.Metrics.Gauge.Abstractions;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter;
using App.Metrics.Meter.Abstractions;
using App.Metrics.ReservoirSampling;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Core.Internal
{
    // ReSharper disable ClassNeverInstantiated.Global
    internal sealed class DefaultMetricsBuilderFactory : IBuildMetrics
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public DefaultMetricsBuilderFactory()
        {
            var defaultReservoir = new DefaultSamplingReservoirProvider();
            Apdex = new DefaultApdexBuilder(defaultReservoir);
            Counter = new DefaultCounterBuilder();
            Gauge = new DefaultGaugeBuilder();
            Histogram = new DefaultHistogramBuilder(defaultReservoir);
            Meter = new DefaultMeterBuilder();
            Timer = new DefaultTimerBuilder(defaultReservoir);
        }

        /// <inheritdoc />
        public IBuildApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IBuildCounterMetrics Counter { get; }

        /// <inheritdoc />
        public IBuildGaugeMetrics Gauge { get; }

        /// <inheritdoc />
        public IBuildHistogramMetrics Histogram { get; }

        /// <inheritdoc />
        public IBuildMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IBuildTimerMetrics Timer { get; }
    }
}