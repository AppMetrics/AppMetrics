// <copyright file="IHistogram.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Histogram
{
    /// <summary>
    ///     A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a
    ///     search.
    /// </summary>
    public interface IHistogram : IResetableMetric
    {
        /// <summary>
        ///     Records a value.
        /// </summary>
        /// <param name="value">Value to be added to the histogram.</param>
        /// <param name="userValue">
        ///     A custom user value that will be associated to the results.
        ///     Useful for tracking (for example) for which id the max or min value was recorded.
        /// </param>
        void Update(long value, string userValue);

        /// <summary>
        ///     Records a value.
        /// </summary>
        /// <param name="value">Value to be added to the histogram.</param>
        void Update(long value);
    }
}