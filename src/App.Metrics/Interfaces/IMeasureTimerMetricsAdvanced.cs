// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;

namespace App.Metrics.Interfaces
{
    public interface IMeasureTimerMetricsAdvanced
    {
        ITimer With(TimerOptions options);

        ITimer With<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric;

        ITimer WithHistogram<T>(TimerOptions options, Func<T> histogramMetricBuilder)
            where T : IHistogramMetric;
    }
}