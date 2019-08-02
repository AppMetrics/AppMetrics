// <copyright file="DefaultHistogramMetricProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;
using App.Metrics.Registry;

namespace App.Metrics.BucketHistogram
{
    public class DefaultBucketHistogramMetricProvider : IProvideBucketHistogramMetrics
    {
        private readonly IBuildBucketHistogramMetrics _histogramBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHistogramMetricProvider" /> class.
        /// </summary>
        /// <param name="histogramBuilder">The histogram builder.</param>
        /// <param name="registry">The registry.</param>
        public DefaultBucketHistogramMetricProvider(IBuildBucketHistogramMetrics histogramBuilder, IMetricsRegistry registry)
        {
            _registry = registry;
            _histogramBuilder = histogramBuilder;
        }

        /// <inheritdoc />
        public IBucketHistogram Instance(BucketHistogramOptions options)
        {
            return Instance(options, () => _histogramBuilder.Build(options.Buckets));
        }

        /// <inheritdoc />
        public IBucketHistogram Instance<T>(BucketHistogramOptions options, Func<T> builder)
            where T : IBucketHistogramMetric
        {
            return _registry.BucketHistogram(options, builder);
        }

        /// <inheritdoc />
        public IBucketHistogram Instance(BucketHistogramOptions options, MetricTags tags)
        {
            return Instance(options, tags, () => _histogramBuilder.Build(options.Buckets));
        }

        /// <inheritdoc />
        public IBucketHistogram Instance<T>(BucketHistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketHistogramMetric
        {
            return _registry.BucketHistogram(options, tags, builder);
        }
    }
}