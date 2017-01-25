// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Interfaces;
using App.Metrics.Data.Interfaces;
using App.Metrics.Histogram.Interfaces;
using App.Metrics.Meter.Interfaces;
using App.Metrics.Timer.Interfaces;

namespace App.Metrics.Registry.Interfaces
{
    public interface IMetricContextRegistry
    {
        string Context { get; }

        IMetricRegistryManager DataProvider { get; }

        IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric;

        void ClearAllMetrics();

        ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric;

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric;

        IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric;

        ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric;
    }
}