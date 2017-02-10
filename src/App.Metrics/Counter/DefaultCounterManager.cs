// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Counter
{
    internal sealed class DefaultCounterManager : IMeasureCounterMetrics
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
        public void Decrement(CounterOptions options, string setItem)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(setItem);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, string setItem)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(setItem, amount);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement();
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, MetricSetItem itemSetup)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(itemSetup);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, MetricSetItem itemSetup)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Decrement(itemSetup, amount);
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
        public void Increment(CounterOptions options, string setItem)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(setItem);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount, string setItem)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(setItem, amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, MetricSetItem setItem)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(setItem);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount, MetricSetItem setItem)
        {
            _registry.Counter(options, () => _counterBuilder.Build()).Increment(setItem, amount);
        }
    }
}