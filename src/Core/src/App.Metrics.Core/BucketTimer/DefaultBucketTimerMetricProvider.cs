// <copyright file="DefaultTimerMetricProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.BucketHistogram;
using App.Metrics.Registry;
using App.Metrics.Timer;

namespace App.Metrics.BucketTimer
{
    public class DefaultBucketTimerMetricProvider : IProvideBucketTimerMetrics
    {
        private readonly IClock _clock;
        private readonly IMetricsRegistry _registry;
        private readonly IBuildBucketTimerMetrics _timerBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultBucketTimerMetricProvider" /> class.
        /// </summary>
        /// <param name="timerBuilder">The timer builder.</param>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="clock">The clock.</param>
        public DefaultBucketTimerMetricProvider(IBuildBucketTimerMetrics timerBuilder, IMetricsRegistry registry, IClock clock)
        {
            _registry = registry;
            _clock = clock;
            _timerBuilder = timerBuilder;
        }

        /// <inheritdoc />
        public ITimer Instance(BucketTimerOptions options)
        {
            return Instance(
                options,
                () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit));
        }

        /// <inheritdoc />
        public ITimer Instance(BucketTimerOptions options, MetricTags tags)
        {
            return Instance(
                options,
                tags,
                () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit));
        }

        /// <inheritdoc />
        public ITimer Instance<T>(BucketTimerOptions options, Func<T> builder)
            where T : IBucketTimerMetric
        {
            return _registry.BucketTimer(options, builder);
        }

        public ITimer Instance<T>(BucketTimerOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketTimerMetric
        {
            return _registry.BucketTimer(options, tags, builder);
        }

        /// <inheritdoc />
        public ITimer WithHistogram<T>(BucketTimerOptions options, Func<T> histogramMetricBuilder)
            where T : IBucketHistogramMetric
        {
            return Instance(
                options,
                () => _timerBuilder.Build(histogramMetricBuilder(), _clock, options.DurationUnit));
        }

        /// <inheritdoc />
        public ITimer WithHistogram<T>(BucketTimerOptions options, MetricTags tags, Func<T> histogramMetricBuilder)
            where T : IBucketHistogramMetric
        {
            return Instance(
                options,
                tags,
                () => _timerBuilder.Build(histogramMetricBuilder(), _clock, options.DurationUnit));
        }
    }
}