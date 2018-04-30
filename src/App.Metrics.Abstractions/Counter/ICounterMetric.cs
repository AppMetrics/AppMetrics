// <copyright file="ICounterMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Counter
{
    /// <summary>
    ///     Provides access to a counter metric implementation
    /// </summary>
    /// <seealso cref="ICounter" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface ICounterMetric : ICounter, IMetricValueProvider<CounterValue>
    {
    }
}