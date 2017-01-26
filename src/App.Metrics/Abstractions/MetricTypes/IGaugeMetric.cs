// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
    public interface IGaugeMetric : IMetricValueProvider<double>
    {
    }
}