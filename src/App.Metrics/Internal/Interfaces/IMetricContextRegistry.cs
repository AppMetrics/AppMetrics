// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Data.Interfaces;

namespace App.Metrics.Internal.Interfaces
{
    internal interface IMetricContextRegistry
    {
        string Context { get; }

        IMetricRegistryManager DataProvider { get; }

        void ClearAllMetrics();

        ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric;

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric;

        IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric;

        ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric;
    }
}