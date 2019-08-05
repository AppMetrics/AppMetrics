// <copyright file="MetricType.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    /// <summary>
    ///     Available metric types
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        ///     A <see href="http://app-metrics.io/getting-started/metric-types/gauges.html">Gauge</see>
        ///     Metric
        /// </summary>
        Gauge,

        /// <summary>
        ///     A <see href="http://app-metrics.io/getting-started/metric-types/counters.html">Counter</see>
        ///     Metric
        /// </summary>
        Counter,

        /// <summary>
        ///     A <see href="http://app-metrics.io/getting-started/metric-types/meters.html">Meter</see>
        ///     Metric
        /// </summary>
        Meter,

        /// <summary>
        ///     A
        ///     <see href="http://app-metrics.io/getting-started/metric-types/histograms.html">Histogram</see>
        ///     Metric
        /// </summary>
        Histogram,

        /// <summary>
        ///     A
        ///     <see href="http://app-metrics.io/getting-started/metric-types/histograms.html">BucketHistogram</see>
        ///     Metric
        /// </summary>
        BucketHistogram,

        /// <summary>
        ///     A <see href="http://app-metrics.io/getting-started/metric-types/histograms.html">Timer</see>
        ///     Metric
        /// </summary>
        Timer,

        /// <summary>
        ///     A <see href="http://app-metrics.io/getting-started/metric-types/histograms.html">Timer</see>
        ///     Metric
        /// </summary>
        BucketTimer,

        /// <summary>
        ///     An <see href="http://app-metrics.io/getting-started/metric-types/apdex.html">Apdex</see>
        ///     Metric
        /// </summary>
        Apdex
    }
}