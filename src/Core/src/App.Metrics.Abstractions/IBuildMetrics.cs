// <copyright file="IBuildMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Registry;
using App.Metrics.Timer;

namespace App.Metrics
{
    /// <summary>
    ///     Provides access to APIs which build instances of all available metric types. Metrics created are not added to the
    ///     <see cref="IMetricsRegistry" />.
    /// </summary>
    public interface IBuildMetrics
    {
        /// <summary>
        ///     Gets the Apdex API to build <see cref="IApdexMetric" />s
        /// </summary>
        /// <value>
        ///     The Apdex API for building <see cref="IApdexMetric" />s
        /// </value>
        IBuildApdexMetrics Apdex { get; }

        /// <summary>
        ///     Gets the Counter API to build <see cref="ICounterMetric" />s
        /// </summary>
        /// <value>
        ///     The Counter API for building <see cref="ICounterMetric" />s
        /// </value>
        IBuildCounterMetrics Counter { get; }

        /// <summary>
        ///     Gets the Gauge API to build <see cref="IGaugeMetric" />s
        /// </summary>
        /// <value>
        ///     The Gauge API for building <see cref="IGaugeMetric" />s
        /// </value>
        IBuildGaugeMetrics Gauge { get; }

        /// <summary>
        ///     Gets the Histogram API to build <see cref="IHistogramMetric" />s
        /// </summary>
        /// <value>
        ///     The Histogram API for building <see cref="IHistogramMetric" />s
        /// </value>
        IBuildHistogramMetrics Histogram { get; }

        /// <summary>
        ///     Gets the Bucket Histogram API to build <see cref="IBucketHistogramMetric" />s
        /// </summary>
        /// <value>
        ///     The Bucket Histogram API for building <see cref="IBucketHistogramMetric" />s
        /// </value>
        IBuildBucketHistogramMetrics BucketHistogram { get; }

        /// <summary>
        ///     Gets the Meter API to build <see cref="IMeterMetric" />s
        /// </summary>
        /// <value>
        ///     The Meter API for building <see cref="IMeterMetric" />s
        /// </value>
        IBuildMeterMetrics Meter { get; }

        /// <summary>
        ///     Gets the Timer API to build <see cref="ITimerMetric" />s
        /// </summary>
        /// <value>
        ///     The Timer API for building <see cref="ITimerMetric" />s
        /// </value>
        IBuildTimerMetrics Timer { get; }

        /// <summary>
        ///     Gets the Bucket Timer API to build <see cref="IBucketTimerMetric" />s
        /// </summary>
        /// <value>
        ///     The Bucket Timer API for building <see cref="IBucketTimerMetric" />s
        /// </value>
        IBuildBucketTimerMetrics BucketTimer { get; }
    }
}