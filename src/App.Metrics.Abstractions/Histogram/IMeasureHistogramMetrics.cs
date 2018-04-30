// <copyright file="IMeasureHistogramMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Histogram
{
    /// <summary>
    ///     Provides access to the API allowing Histogram Metrics to be measured/recorded.
    /// </summary>
    public interface IMeasureHistogramMetrics
    {
        /// <summary>
        ///     Updates a <see cref="IHistogramMetric" /> which measures the distribution of values in a stream of data. Records
        ///     the min, mean,
        ///     max and standard deviation of values and also quantiles such as the medium, 95th percentile, 98th percentile, 99th
        ///     percentile and 99.9th percentile
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <param name="value">The value to be added to the histogram.</param>
        void Update(HistogramOptions options, long value);

        /// <summary>
        ///     Updates a <see cref="IHistogramMetric" /> which measures the distribution of values in a stream of data. Records
        ///     the min, mean,
        ///     max and standard deviation of values and also quantiles such as the medium, 95th percentile, 98th percentile, 99th
        ///     percentile and 99.9th percentile
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="value">The value to be added to the histogram.</param>
        void Update(HistogramOptions options, MetricTags tags, long value);

        /// <summary>
        ///     Updates a <see cref="IHistogramMetric" /> which measures the distribution of values in a stream of data. Records
        ///     the min, mean,
        ///     max and standard deviation of values and also quantiles such as the medium, 95th percentile, 98th percentile, 99th
        ///     percentile and 99.9th percentile
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <param name="value">The value to be added to the histogram.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Update(HistogramOptions options, long value, string userValue);

        /// <summary>
        ///     Updates a <see cref="IHistogramMetric" /> which measures the distribution of values in a stream of data. Records
        ///     the min, mean,
        ///     max and standard deviation of values and also quantiles such as the medium, 95th percentile, 98th percentile, 99th
        ///     percentile and 99.9th percentile
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="value">The value to be added to the histogram.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Update(HistogramOptions options, MetricTags tags, long value, string userValue);
    }
}