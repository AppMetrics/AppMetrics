// <copyright file="IHistogramMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.BucketHistogram
{
    /// <summary>
    ///     Provides access to a histgram metric implementation, allows custom
    ///     histograms to be implemented
    /// </summary>
    /// <seealso cref="IBucketHistogram" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface IBucketHistogramMetric : IBucketHistogram, IMetricValueProvider<BucketHistogramValue>, IDisposable
    {
    }
}