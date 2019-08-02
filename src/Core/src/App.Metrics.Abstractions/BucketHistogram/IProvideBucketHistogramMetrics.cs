// <copyright file="IProvideHistogramMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.BucketHistogram
{
    public interface IProvideBucketHistogramMetrics
    {
        /// <summary>
        ///     Instantiates an instance of a <see cref="IBucketHistogram" />
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <returns>A new instance of an <see cref="IBucketHistogram" /> or the existing registered instance of the histogram</returns>
        IBucketHistogram Instance(BucketHistogramOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IBucketHistogram" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IBucketHistogram" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="IBucketHistogram" /> that is being measured</param>
        /// <param name="builder">The function used to build the histogram metric.</param>
        /// <returns>A new instance of an <see cref="IBucketHistogram" /> or the existing registered instance of the histogram</returns>
        IBucketHistogram Instance<T>(BucketHistogramOptions options, Func<T> builder)
            where T : IBucketHistogramMetric;

        /// <summary>
        ///     Instantiates an instance of a <see cref="IBucketHistogram" />
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <returns>
        ///     A new instance of an <see cref="IBucketHistogram" /> or the existing registered instance of the histogram
        /// </returns>
        IBucketHistogram Instance(BucketHistogramOptions options, MetricTags tags);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IBucketHistogram" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IBucketHistogram" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="IBucketHistogram" /> that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="builder">The function used to build the histogram metric.</param>
        /// <returns>
        ///     A new instance of an <see cref="IBucketHistogram" /> or the existing registered instance of the histogram
        /// </returns>
        IBucketHistogram Instance<T>(BucketHistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketHistogramMetric;
    }
}