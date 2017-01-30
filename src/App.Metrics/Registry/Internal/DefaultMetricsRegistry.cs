// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Infrastructure;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Timer.Abstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Registry.Internal
{
    internal sealed class DefaultMetricsRegistry : IMetricsRegistry
    {
        private readonly IClock _clock;
        private readonly ConcurrentDictionary<string, IMetricContextRegistry> _contexts = new ConcurrentDictionary<string, IMetricContextRegistry>();
        private readonly string _defaultContextLabel;
        private readonly EnvironmentInfoProvider _environmentInfoProvider;
        private readonly ILogger _logger;
        private readonly Func<string, IMetricContextRegistry> _newContextRegistry;
        private readonly Lazy<NullMetricsRegistry> _nullMetricsRegistry = new Lazy<NullMetricsRegistry>();

        public DefaultMetricsRegistry(
            ILoggerFactory loggerFactory,
            AppMetricsOptions options,
            IClock clock,
            EnvironmentInfoProvider environmentInfoProvider,
            Func<string, IMetricContextRegistry> newContextRegistry)
        {
            _logger = loggerFactory.CreateLogger<DefaultMetricContextRegistry>();
            _environmentInfoProvider = environmentInfoProvider;
            _clock = clock;
            _newContextRegistry = newContextRegistry;
            _defaultContextLabel = options.DefaultContextLabel;
            _contexts.TryAdd(_defaultContextLabel, newContextRegistry(_defaultContextLabel));
        }

        public IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Apdex(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Apdex(options, builder);
        }

        public void Clear()
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.Clear();
            }

            ForAllContexts(
                c =>
                {
                    c.ClearAllMetrics();
                    _contexts.TryRemove(c.Context, out c);
                });
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Counter(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Counter(options, builder);
        }

        /// <inheritdoc />
        public void Disable()
        {
            Clear();

            _nullMetricsRegistry.Value.Disable();
        }

        public MetricValueOptionsBase EnsureContextLabel(MetricValueOptionsBase options)
        {
            if (options.Context.IsMissing())
            {
                options.Context = _defaultContextLabel;
            }

            return options;
        }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.Gauge(options, valueProvider);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            contextRegistry.Gauge(options, valueProvider);
        }

        public MetricsDataValueSource GetData(IFilterMetrics filter)
        {
            _logger.RetrievedMetricsData();

            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.GetData(filter);
            }

            if (_contexts.Count == 0)
            {
                return MetricsDataValueSource.Empty;
            }

            var environment = _environmentInfoProvider.Build();

            var contexts = _contexts.Values.Select(
                g => new MetricsContextValueSource(
                    g.Context,
                    g.DataProvider.Gauges.ToArray(),
                    g.DataProvider.Counters.ToArray(),
                    g.DataProvider.Meters.ToArray(),
                    g.DataProvider.Histograms.ToArray(),
                    g.DataProvider.Timers.ToArray(),
                    g.DataProvider.ApdexScores.ToArray()));

            var data = new MetricsDataValueSource(_clock.UtcDateTime, environment, contexts);

            _logger.GettingMetricsData();

            return data.Filter(filter);
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Histogram(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Histogram(options, builder);
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Meter(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Meter(options, builder);
        }

        public void RemoveContext(string context)
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.RemoveContext(context);
            }

            if (context.IsMissing())
            {
                throw new ArgumentException("Registry Context cannot be null or empty", nameof(context));
            }

            IMetricContextRegistry contextRegistry;

            if (_contexts.TryRemove(context, out contextRegistry))
            {
                contextRegistry.ClearAllMetrics();
            }
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.Timer(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Timer(options, builder);
        }

        private void ForAllContexts(Action<IMetricContextRegistry> action)
        {
            foreach (var contextRegistry in _contexts.Values)
            {
                action(contextRegistry);
            }
        }
    }
}