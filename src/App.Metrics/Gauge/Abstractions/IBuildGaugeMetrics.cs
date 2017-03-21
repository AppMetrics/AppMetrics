// <copyright file="IBuildGaugeMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Abstractions;

namespace App.Metrics.Gauge.Abstractions
{
    public interface IBuildGaugeMetrics
    {
        IGaugeMetric Build(Func<double> valueProvider);

        IGaugeMetric Build(Func<IMetricValueProvider<double>> valueProvider);

        IGaugeMetric Build();

        IGaugeMetric Build<T>(Func<T> builder)
            where T : IGaugeMetric;
    }
}