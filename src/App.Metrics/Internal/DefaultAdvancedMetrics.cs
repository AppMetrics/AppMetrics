// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Utils;

namespace App.Metrics.Internal
{
    internal sealed class DefaultAdvancedMetrics : IAdvancedMetrics
    {
        private IMetricsRegistry _registry;

        public DefaultAdvancedMetrics(
            AppMetricsOptions options,
            IMetricsFilter globalFilter,
            IMetricsRegistry registry,
            IHealthStatusProvider heathStatusProvider)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Clock = options.Clock;
            Health = heathStatusProvider;
            GlobalFilter = globalFilter ?? new DefaultMetricsFilter();

            _registry = registry;
        }

        public IClock Clock { get; }

        public IMetricsDataProvider Data => this;

        public IMetricsFilter GlobalFilter { get; }

        public IHealthStatusProvider Health { get; }

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

        public async Task<MetricsContextValueSource> ReadContextAsync(string context)
        {
            var data = await ReadDataAsync();

            var filter = new DefaultMetricsFilter().WhereContext(context);

            var contextData = data.Filter(filter);

            return contextData.Contexts.Single();
        }

        public async Task<MetricsDataValueSource> ReadDataAsync()
        {
            var data = await _registry.GetDataAsync();
            return data.Filter(GlobalFilter);
        }

        public async Task<MetricsDataValueSource> ReadDataAsync(IMetricsFilter filter)
        {
            var data = await ReadDataAsync();

            return data.Filter(filter);
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
    }
}