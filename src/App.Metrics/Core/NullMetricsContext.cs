// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    //public sealed class NullMetricsContext : IMetricsContext, IAdvancedMetricsContext
    //{
    //    internal const string InternalMetricsContextName = "App.Metrics.Internal";
    //    private readonly IMetricsContext _metricsContext;

    //    public NullMetricsContext(string context, IClock systemClock, SamplingType defaultSamplingType)
    //    {
    //        Func<IMetricGroupRegistry> setupMetricsRegistry = () => new NullMetricGroupRegistry();
    //        var metricsBuilder = new DefaultMetricsBuilder(systemClock, defaultSamplingType);
    //        var healthCheckDataProvider = new NullHealthCheckManager();
    //        IMetricsDataManager metricsDataManager
    //            = new NullMetricsDataManager();

    //        HealthCheckManager = healthCheckDataProvider;
    //        GroupName = context;
    //        MetricsDataManager = metricsDataManager;
    //        MetricRegistryManager = setupMetricsRegistry().DataProvider;

    //        _metricsContext = new DefaultMetricsContext(context, systemClock, defaultSamplingType,
    //            setupMetricsRegistry, metricsBuilder,
    //            healthCheckDataProvider, metricsDataManager);
    //    }


    //    public event EventHandler ContextDisabled;

    //    public event EventHandler ContextShuttingDown;

    //    public IAdvancedMetricsContext Advanced => this;

    //    public IClock Clock => _metricsContext.Advanced.Clock;


    //    public string GroupName { get; }

    //    public ConcurrentDictionary<string, IMetricsContext> Groups { get; } = new ConcurrentDictionary<string, IMetricsContext>();

    //    public IHealthCheckManager HealthCheckManager { get; }

    //    public IMetricRegistryManager MetricRegistryManager { get; }

    //    public IMetricsDataManager MetricsDataManager { get; }

    //    public bool AttachGroup(string groupName, IMetricsContext context)
    //    {
    //        return _metricsContext.Advanced.AttachGroup(groupName, context);
    //    }

    //    public void CompletelyDisableMetrics()
    //    {
    //        _metricsContext.Advanced.CompletelyDisableMetrics();
    //    }

    //    public ICounter Counter(CounterOptions options)
    //    {
    //        return _metricsContext.Advanced.Counter(options);
    //    }

    //    public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterImplementation
    //    {
    //        return _metricsContext.Advanced.Counter(options, builder);
    //    }


    //    public void Decrement(CounterOptions options)
    //    {
    //        _metricsContext.Decrement(options);
    //    }

    //    public void Decrement(CounterOptions options, long amount)
    //    {
    //        _metricsContext.Decrement(options, amount);
    //    }

    //    public void Decrement(CounterOptions options, string item)
    //    {
    //        _metricsContext.Decrement(options, item);
    //    }

    //    public void Decrement(CounterOptions options, long amount, string item)
    //    {
    //        _metricsContext.Decrement(options, amount, item);
    //    }


    //    public void Dispose()
    //    {
    //        _metricsContext.Dispose();
    //    }

    //    public void Gauge(GaugeOptions options, Func<double> valueProvider)
    //    {
    //        _metricsContext.Gauge(options, valueProvider);
    //    }

    //    public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
    //    {
    //        _metricsContext.Advanced.Gauge(options, valueProvider);
    //    }

    //    public IMetricsContext Group(string groupName)
    //    {
    //        return _metricsContext.Advanced.Group(groupName);
    //    }

    //    public IMetricsContext Group(string groupName, Func<string, IMetricsContext> groupCreator)
    //    {
    //        return _metricsContext.Advanced.Group(groupName, groupCreator);
    //    }

    //    public IHistogram Histogram(HistogramOptions options)
    //    {
    //        return _metricsContext.Advanced.Histogram(options);
    //    }

    //    public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation
    //    {
    //        return _metricsContext.Advanced.Histogram(options, builder);
    //    }

    //    public IHistogram Histogram(HistogramOptions options, Func<IReservoir> builder)
    //    {
    //        return _metricsContext.Advanced.Histogram(options, builder);
    //    }

    //    public void Increment(CounterOptions options)
    //    {
    //        _metricsContext.Increment(options);
    //    }

    //    public void Increment(CounterOptions options, long amount)
    //    {
    //        _metricsContext.Increment(options, amount);
    //    }

    //    public void Increment(CounterOptions options, string item)
    //    {
    //        _metricsContext.Increment(options, item);
    //    }

    //    public void Increment(CounterOptions options, long amount, string item)
    //    {
    //        _metricsContext.Increment(options, amount, item);
    //    }

    //    public void Mark(MeterOptions options)
    //    {
    //        _metricsContext.Mark(options);
    //    }

    //    public void Mark(MeterOptions options, long amount)
    //    {
    //        _metricsContext.Mark(options, amount);
    //    }

    //    public void Mark(MeterOptions options, string item)
    //    {
    //        _metricsContext.Mark(options, item);
    //    }

    //    public void Mark(MeterOptions options, long amount, string item)
    //    {
    //        _metricsContext.Mark(options, amount, item);
    //    }

    //    public IMeter Meter(MeterOptions options)
    //    {
    //        return _metricsContext.Advanced.Meter(options);
    //    }

    //    public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation
    //    {
    //        return _metricsContext.Advanced.Meter(options, builder);
    //    }

    //    public void ResetMetricsValues()
    //    {
    //        _metricsContext.Advanced.ResetMetricsValues();
    //    }

    //    public void ShutdownGroup(string groupName)
    //    {
    //        _metricsContext.Advanced.ShutdownGroup(groupName);
    //    }

    //    public void Time(TimerOptions options, Action action, string userValue)
    //    {
    //        _metricsContext.Time(options, action, userValue);
    //    }

    //    public void Time(TimerOptions options, Action action)
    //    {
    //        _metricsContext.Time(options, action);
    //    }

    //    public ITimer Timer(TimerOptions options)
    //    {
    //        return _metricsContext.Advanced.Timer(options);
    //    }

    //    public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerImplementation
    //    {
    //        return _metricsContext.Advanced.Timer(options);
    //    }

    //    public ITimer Timer(TimerOptions options, Func<IHistogramImplementation> builder)
    //    {
    //        return _metricsContext.Advanced.Timer(options, builder);
    //    }

    //    public ITimer Timer(TimerOptions options, Func<IReservoir> builder)
    //    {
    //        return _metricsContext.Advanced.Timer(options, builder);
    //    }

    //    public void Update(HistogramOptions options, long value, string userValue)
    //    {
    //        _metricsContext.Update(options, value, userValue);
    //    }

    //    public void Update(HistogramOptions options, long value)
    //    {
    //        _metricsContext.Update(options, value);
    //    }
    //}
}