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
        ICounterImplementation BuildCounter(string name, Unit unit);

        IMetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider);

        IHistogramImplementation BuildHistogram(string name, Unit unit, SamplingType samplingType);

        IHistogramImplementation BuildHistogram(string name, Unit unit, IReservoir reservoir);

        IMeterImplementation BuildMeter(string name, Unit unit, TimeUnit rateUnit);

        ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType);

        ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IHistogramImplementation histogram);

        ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IReservoir reservoir);
    }
}