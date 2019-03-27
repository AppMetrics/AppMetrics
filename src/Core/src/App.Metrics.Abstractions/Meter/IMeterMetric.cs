// <copyright file="IMeterMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Meter
{
    /// <summary>
    ///     Provides access to a meter metric implementation, allows custom meters to be implemented.
    /// </summary>
    /// <seealso cref="IMeter" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface IMeterMetric : IMeter, IMetricValueProvider<MeterValue>, IDisposable
    {
    }
}