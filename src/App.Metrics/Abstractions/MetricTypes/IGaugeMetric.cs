// <copyright file="IGaugeMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Abstractions;
using App.Metrics.Gauge;

namespace App.Metrics.Abstractions.MetricTypes
{
    /// <summary>
    ///     Provides access to a gauge metric implementation e.g. <see cref="FunctionGauge" />, <see cref="HitRatioGauge" />,
    ///     <see cref="DerivedGauge" />, <see cref="HitPercentageGauge" />, <see cref="PercentageGauge" />.
    ///     Allows custom gauges to be implemented
    /// </summary>
    /// <seealso cref="IGaugeMetric" />
    public interface IGaugeMetric : IGauge, IMetricValueProvider<double>
    {
    }
}