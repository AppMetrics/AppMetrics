// <copyright file="DefaultHistogramManager.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Registry;

namespace App.Metrics.BucketHistogram
{
    internal sealed class DefaultBucketHistogramManager : IMeasureBucketHistogramMetrics
    {
        private readonly IBuildBucketHistogramMetrics _histogramBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultBucketHistogramManager" /> class.
        /// </summary>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="histogramBuilder">The histogram builder.</param>
        public DefaultBucketHistogramManager(IBuildBucketHistogramMetrics histogramBuilder, IMetricsRegistry registry)
        {
            _registry = registry;
            _histogramBuilder = histogramBuilder;
        }

        /// <inheritdoc />
        public void Update(BucketHistogramOptions options, long value)
        {
            _registry.BucketHistogram(options, () => _histogramBuilder.Build(options.Buckets)).Update(value);
        }

        /// <inheritdoc />
        public void Update(BucketHistogramOptions options, MetricTags tags, long value)
        {
            _registry.BucketHistogram(options, tags, () => _histogramBuilder.Build(options.Buckets)).Update(value);
        }
    }
}