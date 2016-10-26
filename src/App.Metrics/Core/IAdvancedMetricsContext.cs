// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public interface IAdvancedMetricsContext : IHideObjectMembers
    {
        IClock Clock { get; }

        IMetricsDataManager DataManager { get; }

        IHealthCheckManager HealthCheckManager { get; }

        void CompletelyDisableMetrics();

        ICounter Counter(CounterOptions options);

        ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterImplementation;

        void Gauge(GaugeOptions options, Func<double> valueProvider);

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        IHistogram Histogram(HistogramOptions options);

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation;

        IHistogram Histogram(HistogramOptions options, Func<IReservoir> builder);

        IMeter Meter(MeterOptions options);

        IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation;

        void ResetMetricsValues();

        void ShutdownGroup(string groupName);

        ITimer Timer(TimerOptions options);

        ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerImplementation;

        ITimer Timer(TimerOptions options, Func<IHistogramImplementation> builder);

        ITimer Timer(TimerOptions options, Func<IReservoir> builder);
    }
}