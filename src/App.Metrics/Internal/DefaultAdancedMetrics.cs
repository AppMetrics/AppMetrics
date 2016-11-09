// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Utils;

namespace App.Metrics.Internal
{
    internal sealed class DefaultAdancedMetrics : IAdvancedMetrics
    {
        private IMetricsDataManager _dataManager;
        private IMetricsRegistry _registry;

        public DefaultAdancedMetrics(
            AppMetricsOptions options,
            IMetricsRegistry registry,
            IHealthCheckManager healthCheckManager,
            IMetricsDataManager dataManagerManager)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Clock = options.Clock;
            HealthCheckManager = healthCheckManager;

            _dataManager = dataManagerManager;
            _registry = registry;
        }

        public IClock Clock { get; }

        public IMetricsDataManager DataManager => _dataManager;

        public IHealthCheckManager HealthCheckManager { get; }

        public void Disable()
        {
            if (_registry is NullMetricsRegistry)
            {
                return;
            }

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
    }
}