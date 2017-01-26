// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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