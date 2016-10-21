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
    public sealed class MetricsContext : IMetricsContext, IAdvancedMetricsContext
    {
        internal const string InternalMetricsContextName = "App.Metrics.Internal";

        private readonly IMetricsBuilder _builder;

        private readonly ConcurrentDictionary<string, IMetricsContext> _childContexts =
            new ConcurrentDictionary<string, IMetricsContext>();

        private readonly SamplingType _defaultSamplingType;
        private readonly Func<IMetricsRegistry> _newRegistry;
        private bool _isDisabled;
        private IMetricsRegistry _registry;

        public MetricsContext(string context,
            IClock systemClock,
            SamplingType defaultSamplingType,
            Func<IMetricsRegistry> newRegistry,
            IMetricsBuilder builder,
            IHealthCheckDataProvider healthCheckDataProvider,
            IMetricsDataProvider metricsDataProvider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(context));
            }

            if (systemClock == null)
            {
                throw new ArgumentNullException(nameof(systemClock));
            }

            if (newRegistry == null)
            {
                throw new ArgumentNullException(nameof(newRegistry));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (healthCheckDataProvider == null)
            {
                throw new ArgumentNullException(nameof(healthCheckDataProvider));
            }

            if (metricsDataProvider == null)
            {
                throw new ArgumentNullException(nameof(metricsDataProvider));
            }

            _defaultSamplingType = defaultSamplingType;
            _newRegistry = newRegistry;
            _registry = _newRegistry();
            _builder = builder;
            MetricsDataProvider = metricsDataProvider;

            GroupName = context;
            Clock = systemClock;
            HealthCheckDataProvider = healthCheckDataProvider;
        }

        public IAdvancedMetricsContext Advanced => this;

        public IReadOnlyDictionary<string, IMetricsContext> ChildContexts => _childContexts;


        public IClock Clock { get; }

        public string GroupName { get; }

        public void Decrement(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Decrement(amount);
        }

        public void Decrement(CounterOptions options, string item)
        {
            _registry.Counter(options, () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Decrement(item);
        }

        public void Decrement(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options,
                    () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Decrement(item, amount);
        }

        public void Decrement(CounterOptions options)
        {
            _registry.Counter(options,
                    () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Decrement();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (!_isDisabled)
            {
                ContextShuttingDown?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Gauge(GaugeOptions options, Func<double> valueProvider)
        {
            _registry.Gauge(options,
                () => _builder.BuildGauge(options.Name, options.MeasurementUnit, valueProvider));
        }


        public IMetricsContext Group(string groupName)
        {
            return Group(groupName, c => CreateChildContextInstance(groupName));
        }

        public IMetricsContext Group(string groupName, Func<string, IMetricsContext> groupCreator)
        {
            if (_isDisabled)
            {
                return this;
            }

            return string.IsNullOrEmpty(groupName)
                ? this
                : _childContexts.GetOrAdd(groupName, groupCreator);
        }

        public void Increment(CounterOptions options)
        {
            _registry.Counter(options,
                    () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Increment();
        }

        public void Increment(CounterOptions options, long amount)
        {
            _registry.Counter(options,
                    () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Increment(amount);
        }

        public void Increment(CounterOptions options, string item)
        {
            _registry.Counter(options,
                    () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Increment(item);
        }

        public void Increment(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options,
                    () => _builder.BuildCounter(options.Name, options.MeasurementUnit))
                .Increment(item, amount);
        }

        public void Mark(MeterOptions options)
        {
            _registry.Meter(options,
                    () => _builder.BuildMeter(options.Name, options.MeasurementUnit, options.RateUnit))
                .Mark();
        }

        public void Time(TimerOptions options, Action action, string userValue = null)
        {
            using (_registry.Timer(options,
                    () => _builder.BuildTimer(options.Name, options.MeasurementUnit, options.RateUnit, options.DurationUnit, options.SamplingType))
                .NewContext(userValue))
            {
                action();
            }
        }

        public void Update(HistogramOptions options, long value, string userValue = null)
        {
            _registry.Histogram(options,
                    () => _builder.BuildHistogram(options.Name, options.MeasurementUnit, options.SamplingType))
                .Update(value, userValue);
        }


        private IMetricsContext CreateChildContextInstance(string contextName)
        {
            return new MetricsContext(contextName, Clock, _defaultSamplingType,
                _newRegistry, _builder,
                HealthCheckDataProvider, MetricsDataProvider);
        }

        private void ForAllChildContexts(Action<IMetricsContext> action)
        {
            foreach (var context in _childContexts.Values)
            {
                action(context);
            }
        }

        #region advanced

        public ITimer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _registry.Timer(name, () => _builder.BuildTimer(name, unit, rateUnit, durationUnit, samplingType), unit, rateUnit,
                durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, _defaultSamplingType, rateUnit, durationUnit, tags);
        }

        public ICounter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags = new MetricTags()) where T : ICounterImplementation
        {
            return _registry.Counter(name, builder, unit, tags);
        }

        public ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation
        {
            return _registry.Timer(name, builder, unit, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _builder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _builder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public ICounter Counter(string name, Unit unit, MetricTags tags = new MetricTags())
        {
            return Counter(name, unit, () => _builder.BuildCounter(name, unit), tags);
        }

        public void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags)
        {
            Gauge(name, () => _builder.BuildGauge(name, unit, valueProvider), unit, tags);
        }

        public void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
            _registry.Gauge(name, valueProvider, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags)
        {
            return Histogram(name, unit, () => _builder.BuildHistogram(name, unit, samplingType), tags);
        }

        public IHistogram Histogram(string name, Unit unit, MetricTags tags)
        {
            return Histogram(name, unit, _defaultSamplingType, tags);
        }

        public IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : IHistogramImplementation
        {
            return _registry.Histogram(name, builder, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags)
        {
            return Histogram(name, unit, () => _builder.BuildHistogram(name, unit, builder()), tags);
        }

        public IMeter Meter(string name, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            return Meter(name, unit, () => _builder.BuildMeter(name, unit, rateUnit), rateUnit, tags);
        }

        public IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation
        {
            return _registry.Meter(name, builder, unit, rateUnit, tags);
        }


        public IRegistryDataProvider RegistryDataProvider => _registry.DataProvider;

        public IMetricsDataProvider MetricsDataProvider { get; }

        public IHealthCheckDataProvider HealthCheckDataProvider { get; }

        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public bool AttachContext(string contextName, IMetricsContext context)
        {
            if (_isDisabled)
            {
                return true;
            }

            if (string.IsNullOrEmpty(contextName))
            {
                throw new ArgumentException("Group name can't be null or empty for attached contexts");
            }
            var attached = _childContexts.GetOrAdd(contextName, context);
            return ReferenceEquals(attached, context);
        }

        public void CompletelyDisableMetrics()
        {
            if (_isDisabled)
            {
                return;
            }

            _isDisabled = true;

            var oldRegistry = _registry;
            _registry = new NullMetricsRegistry();
            oldRegistry.ClearAllMetrics();
            using (oldRegistry as IDisposable)
            {
            }

            ForAllChildContexts(c => c.Advanced.CompletelyDisableMetrics());

            ContextShuttingDown?.Invoke(this, EventArgs.Empty);
            ContextDisabled?.Invoke(this, EventArgs.Empty);
        }

        //TODO: Move advanced into separate class

        public void ResetMetricsValues()
        {
            _registry.ResetMetricsValues();
            ForAllChildContexts(c => c.Advanced.ResetMetricsValues());
        }

        public void ShutdownContext(string contextName)
        {
            if (string.IsNullOrEmpty(contextName))
            {
                throw new ArgumentException("groupName must not be null or empty", contextName);
            }

            IMetricsContext context;
            if (_childContexts.TryRemove(contextName, out context))
            {
                context.Dispose();
            }
        }

        #endregion
    }
}