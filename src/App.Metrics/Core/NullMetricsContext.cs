// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class NullMetricsContext : IMetricsContext, IAdvancedMetricsContext
    {
        internal const string InternalMetricsContextName = "App.Metrics.Internal";
        private readonly ConcurrentDictionary<string, IMetricsContext> _childContexts = new ConcurrentDictionary<string, IMetricsContext>();
        private readonly IMetricsContext _metricsContext;

        public NullMetricsContext(string context, IClock systemClock, SamplingType defaultSamplingType)
        {
            Func<IMetricsRegistry> setupMetricsRegistry = () => new NullMetricsRegistry();
            var metricsBuilder = new DefaultMetricsBuilder(systemClock, defaultSamplingType);
            var healthCheckDataProvider = new NullHealthCheckDataProvider();
            IMetricsDataProvider metricsDataProvider
                = new NullMetricsDataProvider();

            HealthCheckDataProvider = healthCheckDataProvider;
            GroupName = context;
            MetricsDataProvider = metricsDataProvider;
            RegistryDataProvider = setupMetricsRegistry().DataProvider;

            _metricsContext = new MetricsContext(context, systemClock, defaultSamplingType,
                setupMetricsRegistry, metricsBuilder,
                healthCheckDataProvider, metricsDataProvider);
        }

        public IAdvancedMetricsContext Advanced => this;

        public IReadOnlyDictionary<string, IMetricsContext> ChildContexts => _childContexts;

        public IClock Clock => _metricsContext.Advanced.Clock;


        public string GroupName { get; }


        public void Decrement(CounterOptions options)
        {
            _metricsContext.Decrement(options);
        }

        public void Decrement(CounterOptions options, long amount)
        {
            _metricsContext.Decrement(options, amount);
        }

        public void Decrement(CounterOptions options, string item)
        {
            _metricsContext.Decrement(options, item);
        }

        public void Decrement(CounterOptions options, long amount, string item)
        {
            _metricsContext.Decrement(options, amount, item);
        }


        public void Dispose()
        {
            _metricsContext.Dispose();
        }

        public void Gauge(GaugeOptions options, Func<double> valueProvider)
        {
            _metricsContext.Gauge(options, valueProvider);
        }

        public IMetricsContext Group(string groupName)
        {
            return _metricsContext.Group(groupName);
        }

        public void Increment(CounterOptions options)
        {
            _metricsContext.Increment(options);
        }

        public void Increment(CounterOptions options, long amount)
        {
            _metricsContext.Increment(options, amount);
        }

        public void Increment(CounterOptions options, string item)
        {
            _metricsContext.Increment(options, item);
        }

        public void Increment(CounterOptions options, long amount, string item)
        {
            _metricsContext.Increment(options, amount, item);
        }

        public void Mark(MeterOptions options)
        {
            _metricsContext.Mark(options);
        }

        public void Time(TimerOptions options, Action action, string userValue = null)
        {
            _metricsContext.Time(options, action, userValue);
        }

        public void Update(HistogramOptions options, long value, string userValue = null)
        {
            _metricsContext.Update(options, value, userValue);
        }

        #region advanced

        //TODO: Move advanced into separate class

        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public IHealthCheckDataProvider HealthCheckDataProvider { get; }

        public IMetricsDataProvider MetricsDataProvider { get; }

        public IRegistryDataProvider RegistryDataProvider { get; }

        public bool AttachContext(string contextName, IMetricsContext context)
        {
            return _metricsContext.Advanced.AttachContext(contextName, context);
        }

        public void CompletelyDisableMetrics()
        {
            _metricsContext.Advanced.CompletelyDisableMetrics();
        }

        public void ResetMetricsValues()
        {
            _metricsContext.Advanced.ResetMetricsValues();
        }

        public void ShutdownContext(string contextName)
        {
            _metricsContext.Advanced.ShutdownContext(contextName);
        }

        public IMetricsContext Context(string contextName, Func<string, IMetricsContext> contextCreator)
        {
            throw new NotImplementedException();
        }

        public ICounter Counter(string name, Unit unit, MetricTags tags = new MetricTags())
        {
            throw new NotImplementedException();
        }

        public ITimer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _metricsContext.Advanced.Timer(name, unit, samplingType, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _metricsContext.Advanced.Timer(name, unit, rateUnit, durationUnit, tags);
        }

        public ICounter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags = new MetricTags()) where T : ICounterImplementation
        {
            throw new NotImplementedException();
        }

        public ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation
        {
            return _metricsContext.Advanced.Timer(name, unit, builder, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _metricsContext.Advanced.Timer(name, unit, builder, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _metricsContext.Advanced.Timer(name, unit, builder, rateUnit, durationUnit, tags);
        }

        public void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags)
        {
            _metricsContext.Advanced.Gauge(name, valueProvider, unit, tags);
        }

        public void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
            _metricsContext.Advanced.Gauge(name, valueProvider, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags)
        {
            return _metricsContext.Advanced.Histogram(name, unit, samplingType, tags);
        }

        public IHistogram Histogram(string name, Unit unit, MetricTags tags)
        {
            return _metricsContext.Advanced.Histogram(name, unit, tags);
        }

        public IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : IHistogramImplementation
        {
            return _metricsContext.Advanced.Histogram(name, unit, builder, tags);
        }

        public IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags)
        {
            return _metricsContext.Advanced.Histogram(name, unit, builder, tags);
        }

        public IMeter Meter(string name, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            return _metricsContext.Advanced.Meter(name, unit, rateUnit, tags);
        }

        public IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation
        {
            return _metricsContext.Advanced.Meter(name, unit, builder, rateUnit, tags);
        }

        public IMetricsContext Group(string groupName, Func<string, IMetricsContext> groupCreator)
        {
            return _metricsContext.Advanced.Group(groupName, groupCreator);
        }

        #endregion
    }
}