// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Sampling;
using App.Metrics.Utils;
using Microsoft.Extensions.Options;

namespace App.Metrics.Internal
{
    internal sealed class DefaultAdancedMetricsContext : IAdvancedMetricsContext
    {
        private IMetricsDataManager _dataManager;
        private bool _isDisabled;
        private IMetricsRegistry _registry;

        public DefaultAdancedMetricsContext(IMetricsContext context,
            IOptions<AppMetricsOptions> options,
            IMetricsRegistry registry,
            IHealthCheckManager healthCheckManager,
            IMetricsDataManager dataManagerManager)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _registry = registry;
            HealthCheckManager = healthCheckManager;
            _dataManager = dataManagerManager;
            Clock = options.Value.Clock;
            _isDisabled = options.Value.DisableMetrics;

            if (_isDisabled)
            {
                ClearAndDisable();
            }
        }

        public IClock Clock { get; }

        public IMetricsDataManager DataManager => _dataManager;

        public IHealthCheckManager HealthCheckManager { get; }

        public void ClearAndDisable()
        {
            if (_isDisabled)
            {
                return;
            }

            _isDisabled = true;

            _registry.Clear();
            Interlocked.Exchange(ref _registry, new NullMetricsRegistry());
            Interlocked.Exchange(ref _dataManager, new DefaultMetricsDataManager(_registry));
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric
        {
            return _registry.Counter(options, builder);
        }

        public ICounter Counter(CounterOptions options)
        {
            return Counter(options, () => this.BuildCounter(options));
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
            return Histogram(options, () => this.BuildHistogram(options));
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric
        {
            //NOTE: Options Resevoir will be ignored the builder should specify
            //TODO: AH - ^ bit confusing
            return _registry.Histogram(options, builder);
        }

        public IHistogram Histogram(HistogramOptions options, Func<IReservoir> builder)
        {
            //NOTE: Options Resevoir will be ignored since we're defining it with the builder
            //TODO: AH - ^ bit confusing
            return Histogram(options, () => this.BuildHistogram(options, builder()));
        }

        public IMeter Meter(MeterOptions options)
        {
            return Meter(options, () => this.BuildMeter(options));
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric
        {
            return _registry.Meter(options, builder);
        }

        public ITimer Timer(TimerOptions options)
        {
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

        public ITimer Timer(TimerOptions options, Func<IReservoir> builder)
        {
            return Timer(options, () => this.BuildTimer(options, builder()));
        }
    }
}