// <copyright file="DefaultMetricsRegistry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Filters;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Internal.NoOp;
using App.Metrics.Logging;
using App.Metrics.Meter;
using App.Metrics.Registry;
using App.Metrics.Timer;

namespace App.Metrics.Internal
{
    public sealed class DefaultMetricsRegistry : IMetricsRegistry
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsRegistry>();
        private readonly IClock _clock;
        private readonly ConcurrentDictionary<string, IMetricContextRegistry> _contexts = new ConcurrentDictionary<string, IMetricContextRegistry>();
        private readonly string _defaultContextLabel;
        private readonly Func<string, IMetricContextRegistry> _newContextRegistry;
        private readonly Lazy<NullMetricsRegistry> _nullMetricsRegistry = new Lazy<NullMetricsRegistry>();

        public DefaultMetricsRegistry(
            string defaultContextLabel,
            IClock clock,
            Func<string, IMetricContextRegistry> newContextRegistry)
        {
            _clock = clock;
            _newContextRegistry = newContextRegistry;
            _defaultContextLabel = defaultContextLabel;
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

        /// <inheritdoc />
        public IApdex Apdex<T>(ApdexOptions options, MetricTags tags, Func<T> builder)
            where T : IApdexMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Apdex(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Apdex(options, tags, builder);
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
        public ICounter Counter<T>(CounterOptions options, MetricTags tags, Func<T> builder)
            where T : ICounterMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Counter(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Counter(options, tags, builder);
        }

        /// <inheritdoc />
        public void Disable()
        {
            Clear();

            _nullMetricsRegistry.Value.Disable();
        }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedMethodReturnValue.Global
        public MetricValueOptionsBase EnsureContextLabel(MetricValueOptionsBase options)
            // ReSharper restore UnusedMethodReturnValue.Global
            // ReSharper restore MemberCanBePrivate.Global
        {
            if (options.Context.IsMissing())
            {
                options.Context = _defaultContextLabel;
            }

            return options;
        }

        /// <inheritdoc />
        public IGauge Gauge<T>(GaugeOptions options, Func<T> builder)
            where T : IGaugeMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Gauge(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Gauge(options, builder);
        }

        /// <inheritdoc />
        public IGauge Gauge<T>(GaugeOptions options, MetricTags tags, Func<T> builder)
            where T : IGaugeMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Gauge(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Gauge(options, tags, builder);
        }

        public MetricsDataValueSource GetData(IFilterMetrics filter)
        {
            Logger.Trace("Getting metrics snaphot");

            if (_nullMetricsRegistry.IsValueCreated)
            {
                Logger.Trace("Using null metrics registry");
                _nullMetricsRegistry.Value.GetData(filter);
            }

            if (_contexts.Count == 0)
            {
                Logger.Trace("No metrics recorded");
                return MetricsDataValueSource.Empty;
            }

            Logger.Trace("Metrics snapshot retrieved containing {ContextCount} contexts", _contexts.Count);

            var contexts = _contexts.Values.Select(
                g => new MetricsContextValueSource(
                    g.Context,
                    g.DataProvider.Gauges.ToArray(),
                    g.DataProvider.Counters.ToArray(),
                    g.DataProvider.Meters.ToArray(),
                    g.DataProvider.Histograms.ToArray(),
                    g.DataProvider.BucketHistograms.ToArray(),
                    g.DataProvider.Timers.ToArray(),
                    g.DataProvider.BucketTimers.ToArray(),
                    g.DataProvider.ApdexScores.ToArray()));

            var data = new MetricsDataValueSource(_clock.UtcDateTime,  contexts);

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

        /// <inheritdoc />
        public IHistogram Histogram<T>(HistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IHistogramMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Histogram(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Histogram(options, tags, builder);
        }

        public IBucketHistogram BucketHistogram<T>(BucketHistogramOptions options, Func<T> builder)
            where T : IBucketHistogramMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.BucketHistogram(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.BucketHistogram(options, builder);
        }

        /// <inheritdoc />
        public IBucketHistogram BucketHistogram<T>(BucketHistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketHistogramMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.BucketHistogram(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.BucketHistogram(options, tags, builder);
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

        public IMeter Meter<T>(MeterOptions options, MetricTags tags, Func<T> builder)
            where T : IMeterMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                return _nullMetricsRegistry.Value.Meter(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Meter(options, tags, builder);
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

            if (_contexts.TryRemove(context, out IMetricContextRegistry contextRegistry))
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

        /// <inheritdoc />
        public ITimer Timer<T>(TimerOptions options, MetricTags tags, Func<T> builder)
            where T : ITimerMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.Timer(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.Timer(options, tags, builder);
        }

        public ITimer BucketTimer<T>(BucketTimerOptions options, Func<T> builder)
            where T : IBucketTimerMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.BucketTimer(options, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.BucketTimer(options, builder);
        }

        /// <inheritdoc />
        public ITimer BucketTimer<T>(BucketTimerOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketTimerMetric
        {
            if (_nullMetricsRegistry.IsValueCreated)
            {
                _nullMetricsRegistry.Value.BucketTimer(options, tags, builder);
            }

            EnsureContextLabel(options);

            var contextRegistry = _contexts.GetOrAdd(options.Context, _newContextRegistry);

            return contextRegistry.BucketTimer(options, tags, builder);
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