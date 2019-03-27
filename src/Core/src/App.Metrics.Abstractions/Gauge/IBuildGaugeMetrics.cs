// <copyright file="IBuildGaugeMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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