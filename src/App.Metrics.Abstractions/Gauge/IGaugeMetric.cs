// <copyright file="IGaugeMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Gauge
{
    /// <summary>
    ///     Provides access to a gauge metric implementation e.g. App.Metrics.FunctionGauge, App.Metrics.HitRatioGauge,
    ///     App.Metrics.DerivedGauge, App.Metrics.HitPercentageGauge, App.Metrics.PercentageGauge
    ///     Allows custom gauges to be implemented
    /// </summary>
    /// <seealso cref="IGaugeMetric" />
    public interface IGaugeMetric : IGauge, IMetricValueProvider<double>
    {
    }
}