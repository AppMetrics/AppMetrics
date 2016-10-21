// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;

namespace App.Metrics.Registries
{
    public interface IMetricsRegistry
    {
        IRegistryDataProvider DataProvider { get; }

        void ClearAllMetrics();

        ICounter Counter<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : ICounterImplementation;

        void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags);

        IHistogram Histogram<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : IHistogramImplementation;

        IMeter Meter<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation;

        void ResetMetricsValues();

        ITimer Timer<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation;

        #region new contract

        ICounter Counter<T>(CounterOptions options, Func<T> builder)
           where T : ICounterImplementation;

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramImplementation;

        IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterImplementation;

        ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerImplementation;

        #endregion
    }
}