// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Builders
{
    internal class DefaultMetricsBuilderFactory : IBuildMetrics
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