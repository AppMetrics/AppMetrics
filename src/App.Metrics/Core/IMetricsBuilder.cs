// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using App.Metrics.MetricData;
using App.Metrics.Sampling;

namespace App.Metrics.Core
{
    public interface IMetricsBuilder : IDisposable
    {
        ICounterMetric BuildCounter(string name, Unit unit);

        IMetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider);

        IHistogramMetric BuildHistogram(string name, Unit unit, SamplingType samplingType);

        IHistogramMetric BuildHistogram(string name, Unit unit, IReservoir reservoir);

        IMeterMetric BuildMeter(string name, Unit unit, TimeUnit rateUnit);

        ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType);

        ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IHistogramMetric histogram);

        ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IReservoir reservoir);
    }
}