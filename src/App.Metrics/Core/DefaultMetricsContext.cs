// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Internal;

namespace App.Metrics.Core
{
    internal sealed class DefaultMetricsContext : IMetricsContext
    {
        private readonly IMetricsRegistry _registry;

        public DefaultMetricsContext(
            AppMetricsOptions options,
            IMetricsRegistry registry,
            IAdvancedMetricsContext advancedContext)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            if (advancedContext == null) throw new ArgumentNullException(nameof(advancedContext));

            _registry = options.DisableMetrics ? new NullMetricsRegistry() : registry;

            ContextName = options.GlobalContextName;

            Advanced = advancedContext;
        }

        public IAdvancedMetricsContext Advanced { get; }

        public string ContextName { get; }

        public void Decrement(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(amount);
        }

        public void Decrement(CounterOptions options, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(item);
        }

        public void Decrement(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement(item, amount);
        }

        public void Decrement(CounterOptions options)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Decrement();
        }

        public void Gauge(GaugeOptions options, Func<double> valueProvider)
        {
            _registry.Gauge(options, () => Advanced.BuildGauge(options, valueProvider));
        }

        public void Increment(CounterOptions options)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment();
        }

        public void Increment(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(amount);
        }

        public void Increment(CounterOptions options, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(item);
        }

        public void Increment(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => Advanced.BuildCounter(options)).Increment(item, amount);
        }

        public void Mark(MeterOptions options)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark();
        }

        public void Mark(MeterOptions options, long amount)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(amount);
        }

        public void Mark(MeterOptions options, string item)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(item);
        }

        public void Mark(MeterOptions options, long amount, string item)
        {
            _registry.Meter(options, () => Advanced.BuildMeter(options)).Mark(item, amount);
        }

        public void Time(TimerOptions options, Action action)
        {
            using (_registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext())
            {
                action();
            }
        }

        public void Time(TimerOptions options, Action action, string userValue)
        {
            using (_registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext(userValue))
            {
                action();
            }
        }

        public TimerContext Time(TimerOptions options)
        {
            return _registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext();
        }

        public TimerContext Time(TimerOptions options, string userValue)
        {
            return _registry.Timer(options, () => Advanced.BuildTimer(options)).NewContext(userValue);
        }

        public void Update(HistogramOptions options, long value)
        {
            _registry.Histogram(options, () => Advanced.BuildHistogram(options)).Update(value);
        }

        public void Update(HistogramOptions options, long value, string userValue)
        {
            _registry.Histogram(options, () => Advanced.BuildHistogram(options)).Update(value, userValue);
        }
    }
}