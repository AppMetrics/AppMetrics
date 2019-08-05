// <copyright file="IHistogram.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Histogram;

namespace App.Metrics.BucketHistogram
{
    /// <summary>
    ///     A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a
    ///     search.
    /// </summary>
    public interface IBucketHistogram : IHistogram
    {
    }
}