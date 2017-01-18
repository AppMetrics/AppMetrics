// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Builders
{
    internal class DefaultMetricsBuilder : IBuildMetrics
    {
        public DefaultMetricsBuilder()
        {
            Apdex = new DefaultApdexBuilder();
            Counter = new DefaultCounterBuilder();
            Gauge = new DefaultGaugeBuilder();
            Histogram = new DefaultHistogramBuilder();
            Meter = new DefaultMeterBuilder();
            Timer = new DefaultTimerBuilder();
        }

        /// <inheritdoc />
        public IBuildApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IBuildCounterMetrics Counter { get; }

        public IBuildGaugeMetrics Gauge { get; }

        /// <inheritdoc />
        public IBuildHistogramMetrics Histogram { get; }

        /// <inheritdoc />
        public IBuildMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IBuildTimerMetrics Timer { get; }
    }
}