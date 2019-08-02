// <copyright file="IHistogram.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.BucketHistogram
{
    /// <summary>
    ///     A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a
    ///     search.
    /// </summary>
    public interface IBucketHistogram : IResetableMetric
    {
        /// <summary>
        ///     Records a value.
        /// </summary>
        /// <param name="value">Value to be added to the histogram.</param>
        void Update(long value);
    }
}