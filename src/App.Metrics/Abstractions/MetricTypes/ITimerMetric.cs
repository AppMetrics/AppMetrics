// <copyright file="ITimerMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Abstractions;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Abstractions.MetricTypes
{
    /// <summary>
    ///     Provides access to a timer metric implementation e.g. <see cref="DefaultTimerMetric" />, allows custom timers to be
    ///     implemented
    /// </summary>
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface ITimerMetric : ITimer, IMetricValueProvider<TimerValue>
    {
    }
}