// <copyright file="DefaultMetricsBuilderFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Core.Apdex;
using App.Metrics.Core.Counter;
using App.Metrics.Core.Gauge;
using App.Metrics.Core.Histogram;
using App.Metrics.Core.Meter;
using App.Metrics.Core.ReservoirSampling;
using App.Metrics.Core.Timer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

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