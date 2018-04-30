// <copyright file="IProvideGaugeMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Gauge
{
    public interface IProvideGaugeMetrics
    {
        IGauge Instance(GaugeOptions options);

        IGauge Instance(GaugeOptions options, MetricTags tags);

        IGauge Instance<T>(GaugeOptions options, Func<T> builder)
            where T : IGaugeMetric;

        IGauge Instance<T>(GaugeOptions options, MetricTags tags, Func<T> builder)
            where T : IGaugeMetric;
    }
}