// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;

namespace App.Metrics.Histogram.Abstractions
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
        /// <param name="value">The value to be added to the histogram.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Update(HistogramOptions options, long value, string userValue);
    }
}