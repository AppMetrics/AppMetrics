// <copyright file="ITimerMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Timer;

namespace App.Metrics.BucketTimer
{
    /// <summary>
    ///     Provides access to a timer metric implementation, allows custom timers to be implemented
    /// </summary>
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface IBucketTimerMetric : ITimer, IMetricValueProvider<BucketTimerValue>
    {
    }
}