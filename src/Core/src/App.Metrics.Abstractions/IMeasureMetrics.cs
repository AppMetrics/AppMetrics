// <copyright file="IMeasureMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics
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
        ///     Gets the available Bucket Histogram API allowing Bucket Histogram metrics to be measured
        /// </summary>
        /// <value>
        ///     The Bucket Histogram API for measuring Bucket Histogram metrics
        /// </value>
        IMeasureBucketHistogramMetrics BucketHistogram { get; }

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

        /// <summary>
        ///     Gets the available Timer API allowing Timer metrics to be measured
        /// </summary>
        /// <value>
        ///     The Timer API for measuring Timer metrics
        /// </value>
        IMeasureBucketTimerMetrics BucketTimer { get; }
    }
}