// <copyright file="IMeasureMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex.Abstractions;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge.Abstractions;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Timer.Abstractions;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace App.Metrics.Core.Abstractions
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

    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}