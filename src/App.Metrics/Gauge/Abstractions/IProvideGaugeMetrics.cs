// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;
using App.Metrics.Tagging;

namespace App.Metrics.Gauge.Abstractions
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