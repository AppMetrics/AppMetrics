// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Counter
{
    internal class DefaultCounterManager : IMeasureCounterMetrics
    {
        private readonly IBuildCounterMetrics _counterBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultCounterManager" /> class.
        /// </summary>
        /// <param name="counterBuilder">The counter builder.</param>
        /// <param name="registry">The registry storing all metric data.</param>
        public DefaultCounterManager(IBuildCounterMetrics counterBuilder, IMetricsRegistry registry)
        {
            _counterBuilder = counterBuilder;
            _registry = registry;
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(amount);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, string item)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(item);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(item, amount);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement();
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(item);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(item, amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment();
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, string item)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(item);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(item, amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(item);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(item, amount);
        }
    }
}