// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics
{
    public interface IAdvancedMetricsContext : IHideObjectMembers
    {
        event EventHandler ContextDisabled;

        event EventHandler ContextShuttingDown;

        IReadOnlyDictionary<string, IMetricsContext> ChildContexts { get; }

        IClock Clock { get; }

        IHealthCheckDataProvider HealthCheckDataProvider { get; }

        IRegistryDataProvider RegistryDataProvider { get; }
        IMetricsDataProvider MetricsDataProvider { get; }

        bool AttachContext(string contextName, IMetricsContext context);

        void CompletelyDisableMetrics();

        void ShutdownContext(string contextName);

        IMetricsContext Group(string groupName, Func<string, IMetricsContext> groupCreator);

        IMetricsContext Group(string groupName);


        ICounter Counter(CounterOptions options);

        void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags = default(MetricTags));

        IHistogram Histogram(string name,
            Unit unit,
            SamplingType samplingType,
            MetricTags tags = default(MetricTags));

        IHistogram Histogram(string name,
            Unit unit,
            MetricTags tags = default(MetricTags));

        IMeter Meter(MeterOptions options);

        ITimer Timer(string name,
            Unit unit,
            SamplingType samplingType,
            TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds,
            MetricTags tags = default(MetricTags));

        ITimer Timer(string name,
            Unit unit,
            TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds,
            MetricTags tags = default(MetricTags));

        ICounter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags = default(MetricTags))
            where T : ICounterImplementation;

        void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags = default(MetricTags));

        IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags = default(MetricTags))
            where T : IHistogramImplementation;

        IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags = default(MetricTags));

        IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
            where T : IMeterImplementation;
        void ResetMetricsValues();

        ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit = TimeUnit.Seconds, TimeUnit durationUnit = TimeUnit.Milliseconds,
            MetricTags tags = default(MetricTags))
            where T : ITimerImplementation;

        ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags));

        
        ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags));
    }
}