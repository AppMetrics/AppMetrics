// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Counter;
using App.Metrics.Counter.Interfaces;
using App.Metrics.Data.Interfaces;

namespace App.Metrics.Abstractions.Metrics
{
    /// <summary>
    ///     Provides access to a counter metric implementation e.g. <see cref="DefaultCounterMetric" />, allows custom counters
    ///     to be
    ///     implemented
    /// </summary>
    /// <seealso cref="ICounter" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface ICounterMetric : ICounter, IMetricValueProvider<CounterValue>
    {
    }
}