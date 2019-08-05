// <copyright file="DefaultMeasureMetricsProvider.cs" company="App Metrics Contributors">
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

namespace App.Metrics.Internal
{
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class DefaultMeasureMetricsProvider : IMeasureMetrics
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMeasureMetricsProvider" /> class.
        /// </summary>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="buideFactory">The buide factory.</param>
        /// <param name="clock">The clock.</param>
        public DefaultMeasureMetricsProvider(IMetricsRegistry registry, IBuildMetrics buideFactory, IClock clock)
        {
            Apdex = new DefaultApdexManager(buideFactory.Apdex, registry, clock);
            Counter = new DefaultCounterManager(buideFactory.Counter, registry);
            Gauge = new DefaultGaugeManager(buideFactory.Gauge, registry);
            Histogram = new DefaultHistogramManager(buideFactory.Histogram, registry);
            BucketHistogram = new DefaultBucketHistogramManager(buideFactory.BucketHistogram, registry);
            Meter = new DefaultMeterManager(buideFactory.Meter, registry, clock);
            Timer = new DefaultTimerManager(buideFactory.Timer, registry, clock);
            BucketTimer = new DefaultBucketTimerManager(buideFactory.BucketTimer, registry, clock);
        }

        /// <inheritdoc />
        public IMeasureApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IMeasureCounterMetrics Counter { get; }

        /// <inheritdoc />
        public IMeasureGaugeMetrics Gauge { get; }

        /// <inheritdoc />
        public IMeasureHistogramMetrics Histogram { get; }

        /// <inheritdoc />
        public IMeasureBucketHistogramMetrics BucketHistogram { get; }

        /// <inheritdoc />
        public IMeasureMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IMeasureTimerMetrics Timer { get; }

        /// <inheritdoc />
        public IMeasureBucketTimerMetrics BucketTimer { get; }
    }
}