// <copyright file="ICounterMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Abstractions;
using App.Metrics.Counter;
using App.Metrics.Counter.Abstractions;

namespace App.Metrics.Abstractions.MetricTypes
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