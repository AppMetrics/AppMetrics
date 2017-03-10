// <copyright file="IMeterMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Abstractions;
using App.Metrics.Meter;
using App.Metrics.Meter.Abstractions;

namespace App.Metrics.Abstractions.MetricTypes
{
    /// <summary>
    ///     Provides access to a meter metric implementation e.g. <see cref="DefaultMeterMetric" />, allows custom meters to be
    ///     implemented
    /// </summary>
    /// <seealso cref="IMeter" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface IMeterMetric : IMeter, IMetricValueProvider<MeterValue>, IDisposable
    {
    }
}