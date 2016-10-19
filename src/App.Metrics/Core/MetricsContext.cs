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

        private readonly ConcurrentDictionary<string, IMetricsContext> _childContexts =
            new ConcurrentDictionary<string, IMetricsContext>();

        private readonly IMetricsBuilder _metricsBuilder;
        private readonly SamplingType _defaultSamplingType;
        private readonly Func<IMetricsRegistry> _setupMetricsRegistry;
        private bool _isDisabled;
        private IMetricsRegistry _metricsRegistry;

        public MetricsContext(string context,
            IClock systemClock,
            SamplingType defaultSamplingType,
            Func<IMetricsRegistry> setupMetricsRegistry,
            IMetricsBuilder metricsBuilder,
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

            if (setupMetricsRegistry == null)
            {
                throw new ArgumentNullException(nameof(setupMetricsRegistry));
            }

            if (metricsBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsBuilder));
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
            _setupMetricsRegistry = setupMetricsRegistry;
            _metricsRegistry = _setupMetricsRegistry();
            _metricsBuilder = metricsBuilder;
            MetricsDataProvider = metricsDataProvider;

            Name = context;
            Clock = systemClock;
            HealthCheckDataProvider = healthCheckDataProvider;
        }

        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public string Name { get; }

        public IAdvancedMetricsContext Advanced => this;

        public IRegistryDataProvider RegistryDataProvider => _metricsRegistry.DataProvider;

        public IMetricsDataProvider MetricsDataProvider { get; }

        public IHealthCheckDataProvider HealthCheckDataProvider { get; }

        public IClock Clock { get; }

        public IReadOnlyDictionary<string, IMetricsContext> ChildContexts => _childContexts;

        public bool AttachContext(string contextName, IMetricsContext context)
        {
            if (_isDisabled)
            {
                return true;
            }

            if (string.IsNullOrEmpty(contextName))
            {
                throw new ArgumentException("Context name can't be null or empty for attached contexts");
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

            var oldRegistry = _metricsRegistry;
            _metricsRegistry = new NullMetricsRegistry();
            oldRegistry.ClearAllMetrics();
            using (oldRegistry as IDisposable)
            {
            }

            ForAllChildContexts(c => c.Advanced.CompletelyDisableMetrics());

            ContextShuttingDown?.Invoke(this, EventArgs.Empty);
            ContextDisabled?.Invoke(this, EventArgs.Empty);
        }

        public IMetricsContext Context(string contextName)
        {
            return Context(contextName, c => CreateChildContextInstance(contextName));
        }

        public IMetricsContext Context(string contextName, Func<string, IMetricsContext> contextCreator)
        {
            if (_isDisabled)
            {
                return this;
            }

            return string.IsNullOrEmpty(contextName)
                ? this
                : _childContexts.GetOrAdd(contextName, contextCreator);
        }

        public ICounter Counter(string name, Unit unit, MetricTags tags)
        {
            return Counter(name, unit, () => _metricsBuilder.BuildCounter(name, unit), tags);
        }

        public ICounter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : ICounterImplementation
        {
            return _metricsRegistry.Counter(name, builder, unit, tags);
        }

        public IMetricsContext CreateChildContextInstance(string contextName)
        {
            return new MetricsContext(contextName, Clock, _defaultSamplingType,
                _setupMetricsRegistry, _metricsBuilder, 
                HealthCheckDataProvider, MetricsDataProvider);
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

        public void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags)
        {
            Gauge(name, () => _metricsBuilder.BuildGauge(name, unit, valueProvider), unit, tags);
        }

        public void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
            _metricsRegistry.Gauge(name, valueProvider, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags)
        {
            return Histogram(name, unit, () => _metricsBuilder.BuildHistogram(name, unit, samplingType), tags);
        }

        public IHistogram Histogram(string name, Unit unit, MetricTags tags)
        {
            return Histogram(name, unit, _defaultSamplingType, tags);
        }

        public IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : IHistogramImplementation
        {
            return _metricsRegistry.Histogram(name, builder, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags)
        {
            return Histogram(name, unit, () => _metricsBuilder.BuildHistogram(name, unit, builder()), tags);
        }

        public IMeter Meter(string name, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            return Meter(name, unit, () => _metricsBuilder.BuildMeter(name, unit, rateUnit), rateUnit, tags);
        }

        public IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation
        {
            return _metricsRegistry.Meter(name, builder, unit, rateUnit, tags);
        }

        public void ResetMetricsValues()
        {
            _metricsRegistry.ResetMetricsValues();
            ForAllChildContexts(c => c.Advanced.ResetMetricsValues());
        }

        public void ShutdownContext(string contextName)
        {
            if (string.IsNullOrEmpty(contextName))
            {
                throw new ArgumentException("contextName must not be null or empty", contextName);
            }

            IMetricsContext context;
            if (_childContexts.TryRemove(contextName, out context))
            {
                context.Dispose();
            }
        }

        public ITimer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _metricsRegistry.Timer(name, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, samplingType), unit, rateUnit,
                durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, _defaultSamplingType, rateUnit, durationUnit, tags);
        }

        public ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation
        {
            return _metricsRegistry.Timer(name, builder, unit, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        private void ForAllChildContexts(Action<IMetricsContext> action)
        {
            foreach (var context in _childContexts.Values)
            {
                action(context);
            }
        }
    }
}