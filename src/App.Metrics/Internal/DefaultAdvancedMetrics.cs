// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Data;
using App.Metrics.Data.Interfaces;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal
{
    internal sealed class DefaultAdvancedMetrics : IAdvancedMetrics
    {
        private readonly IHealthCheckFactory _healthCheckFactory;
        private readonly ILogger<DefaultAdvancedMetrics> _logger;
        private IMetricsRegistry _registry;

        public DefaultAdvancedMetrics(
            ILogger<DefaultAdvancedMetrics> logger,
            AppMetricsOptions options,
            IClock clock,
            IMetricsFilter globalFilter,
            IMetricsRegistry registry,
            IHealthCheckFactory healthCheckFactory)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            GlobalFilter = globalFilter ?? new DefaultMetricsFilter();
            GlobalTags = options.GlobalTags;
            Clock = clock;

            _logger = logger;
            _registry = registry;
            _healthCheckFactory = healthCheckFactory;
        }

        public IClock Clock { get; }

        public IMetricsDataProvider Data => this;

        public IMetricsFilter GlobalFilter { get; }

        public GlobalMetricTags GlobalTags { get; }

        public IHealthStatusProvider Health => this;

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric
        {
            return _registry.Counter(options, builder);
        }

        public ICounter Counter(CounterOptions options)
        {
            return Counter(options, () => this.BuildCounter(options));
        }

        public void Disable()
        {
            if (_registry is NullMetricsRegistry)
            {
                return;
            }

            Interlocked.Exchange(ref _registry, new NullMetricsRegistry());
        }

        public void Gauge(GaugeOptions options, Func<double> valueProvider)
        {
            Gauge(options, () => this.BuildGauge(options, valueProvider));
        }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
        {
            _registry.Gauge(options, valueProvider);
        }

        public IHistogram Histogram(HistogramOptions options)
        {
            if (options.WithReservoir != null)
            {
                return Histogram(options, () => this.BuildHistogram(options, options.WithReservoir()));
            }

            return Histogram(options, () => this.BuildHistogram(options));
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric
        {
            return _registry.Histogram(options, builder);
        }

        public IMeter Meter(MeterOptions options)
        {
            return Meter(options, () => this.BuildMeter(options));
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric
        {
            return _registry.Meter(options, builder);
        }

        public MetricsContextValueSource ReadContext(string context)
        {
            var data = ReadData();

            var filter = new DefaultMetricsFilter().WhereContext(context);

            var contextData = data.Filter(filter);

            return contextData.Contexts.Single();
        }

        public MetricsDataValueSource ReadData()
        {
            return _registry.GetData(GlobalFilter);
        }

        public MetricsDataValueSource ReadData(IMetricsFilter overrideGlobalFilter)
        {
            return _registry.GetData(overrideGlobalFilter);
        }

        public async Task<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(_healthCheckFactory.Checks.Values.OrderBy(v => v.Name)
                .Select(v => v.ExecuteAsync(cancellationToken)));

            var healthStatus = new HealthStatus(results.Where(h => !h.Check.Status.IsIgnored()));

            _logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }

        public void Reset()
        {
            _registry.Clear();
        }

        public void ShutdownContext(string context)
        {
            _registry.RemoveContext(context);
        }

        public ITimer Timer(TimerOptions options)
        {
            if (options.WithReservoir != null)
            {
                return Timer(options, () => this.BuildTimer(options, options.WithReservoir()));
            }

            return _registry.Timer(options, () => this.BuildTimer(options));
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric
        {
            return _registry.Timer(options, builder);
        }

        public ITimer Timer(TimerOptions options, Func<IHistogramMetric> builder)
        {
            return Timer(options, () => this.BuildTimer(options, builder()));
        }

        public IApdex Track(ApdexOptions options)
        {
            if (options.WithReservoir != null)
            {
                return Track(options, () => this.BuildApdex(options, options.WithReservoir()));
            }

            return _registry.Apdex(options, () => this.BuildApdex(options));
        }

        public IApdex Track<T>(ApdexOptions options, Func<T> builder) where T : IApdexMetric
        {
            return _registry.Apdex(options, builder);
        }
    }
}