// <copyright file="IBuildGaugeMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Gauge
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