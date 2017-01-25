// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex.Interfaces;
using App.Metrics.Counter.Interfaces;
using App.Metrics.Gauge.Interfaces;
using App.Metrics.Histogram.Interfaces;
using App.Metrics.Meter.Interfaces;
using App.Metrics.Timer.Interfaces;

namespace App.Metrics.Interfaces
{
    /// <summary>
    ///     Provides access to measure/record available metric types
    /// </summary>
    public interface IMeasureMetrics
    {
        /// <summary>
        ///     Gets the available Apdex API allowing Apdex metrics to be measured
        /// </summary>
        /// <value>
        ///     The Apdex API for measuring Apdex metrics
        /// </value>
        IMeasureApdexMetrics Apdex { get; }

        /// <summary>
        ///     Gets the available Counter API allowing Counter metrics to be measured
        /// </summary>
        /// <value>
        ///     The Counter API for measuring Counter metrics
        /// </value>
        IMeasureCounterMetrics Counter { get; }

        /// <summary>
        ///     Gets the available Gauge API allowing Gauge metrics to be measured
        /// </summary>
        /// <value>
        ///     The Gauge API for measuring Gauge metrics
        /// </value>
        IMeasureGaugeMetrics Gauge { get; }

        /// <summary>
        ///     Gets the available Histogram API allowing Histogram metrics to be measured
        /// </summary>
        /// <value>
        ///     The Histogram API for measuring Histogram metrics
        /// </value>
        IMeasureHistogramMetrics Histogram { get; }

        /// <summary>
        ///     Gets the available Meter API allowing Meter metrics to be measured
        /// </summary>
        /// <value>
        ///     The Meter API for measuring Meter metrics
        /// </value>
        IMeasureMeterMetrics Meter { get; }

        /// <summary>
        ///     Gets the available Timer API allowing Timer metrics to be measured
        /// </summary>
        /// <value>
        ///     The Timer API for measuring Timer metrics
        /// </value>
        IMeasureTimerMetrics Timer { get; }
    }
}