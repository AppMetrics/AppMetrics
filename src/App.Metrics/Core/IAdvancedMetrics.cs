// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Data;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public interface IAdvancedMetrics : IMetricsDataProvider, IHealthStatusProvider, IHideObjectMembers
    {
        IMetricsFilter GlobalFilter { get; }

        IClock Clock { get; }

        IMetricsDataProvider Data { get; }

        IHealthStatusProvider Health { get; }

        ICounter Counter(CounterOptions options);

        ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric;

        void Disable();

        void Gauge(GaugeOptions options, Func<double> valueProvider);

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        IHistogram Histogram(HistogramOptions options);

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric;

        IMeter Meter(MeterOptions options);

        IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric;

        ITimer Timer(TimerOptions options);

        ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric;

        ITimer Timer(TimerOptions options, Func<IHistogramMetric> builder);
    }
}