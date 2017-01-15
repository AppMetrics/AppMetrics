// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultCounterManager : IMeasureCounterMetrics
    {
        private readonly IAdvancedMetrics _advanced;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultCounterManager" /> class.
        /// </summary>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="advanced">The advanced metrics manager.</param>
        public DefaultCounterManager(IAdvancedMetrics advanced, IMetricsRegistry registry)
        {
            _advanced = advanced;
            _registry = registry;
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Decrement(amount);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, string item)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Decrement(item);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Decrement(item, amount);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Decrement();
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Decrement(item);
        }

        /// <inheritdoc />
        public void Decrement(CounterOptions options, long amount, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Decrement(item, amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Increment();
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Increment(amount);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, string item)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Increment(item);
        }

        /// <inheritdoc />
        public void Increment(CounterOptions options, long amount, string item)
        {
            _registry.Counter(options, () => _advanced.BuildCounter(options)).Increment(item, amount);
        }
    }
}