// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Configuration;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Core
{
    /// <summary>
    ///     Provides access to record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    /// <seealso cref="IMetrics" />
    internal sealed class DefaultMetrics : IMetrics
    {
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetrics" /> class.
        /// </summary>
        /// <param name="options">The global metrics options configure on startup.</param>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="advanced">The implementation providing access to more advanced metric options</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public DefaultMetrics(
            AppMetricsOptions options,
            IMetricsRegistry registry,
            IAdvancedMetrics advanced)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            if (advanced == null) throw new ArgumentNullException(nameof(advanced));

            _registry = registry;
            Advanced = advanced;
        }

        /// <inheritdoc />
        public IAdvancedMetrics Advanced { get; }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(amount);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(item);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(item, amount);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement();
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(item);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(item, amount);
        }

        /// <inheritdoc />
        public void Gauge(GaugeOptions options, Func<double> valueProvider)
        {
            _registry.Gauge(options, () => Advanced.BuildGauge(options, valueProvider));
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment();
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(item);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(item);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(item, amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(item, amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark();
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, string item)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(item);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(item);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(item, amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, string item)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(item, amount);
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, Action action)
        {
            using (_registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, Action action, string userValue)
        {
            using (_registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext(userValue))
            {
                action();
            }
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options)
        {
            return _registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext();
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options, string userValue)
        {
            return _registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext(userValue);
        }

        /// <inheritdoc />
        public void Track(ApdexOptions options, Action action)
        {
            using (_registry.Apdex(options, () => Advanced.BuildApdex(options)).NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public ApdexContext Track(ApdexOptions options)
        {
            return _registry.Apdex(options, () => Advanced.BuildApdex(options)).NewContext();
        }

        /// <inheritdoc />
        public void Update(HistogramOptions options, long value)
        {
            _registry.Histogram(options, () => Advanced.BuildHistogram(options)).Update(value);
        }

        /// <inheritdoc />
        public void Update(HistogramOptions options, long value, string userValue)
        {
            _registry.Histogram(options, () => Advanced.BuildHistogram(options)).Update(value, userValue);
        }
    }
}