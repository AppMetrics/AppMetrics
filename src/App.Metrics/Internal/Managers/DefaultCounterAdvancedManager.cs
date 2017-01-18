// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Managers
{
    public class DefaultCounterAdvancedManager : IMeasureCounterMetricsAdvanced
    {
        private readonly IBuildCounterMetrics _counterBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCounterAdvancedManager" /> class.
        /// </summary>
        /// <param name="counterBuilder">The counter builder.</param>
        /// <param name="registry">The registry.</param>
        public DefaultCounterAdvancedManager(IBuildCounterMetrics counterBuilder, IMetricsRegistry registry)
        {
            _registry = registry;
            _counterBuilder = counterBuilder;
        }

        /// <inheritdoc />
        public ICounter Instance<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric { return _registry.Counter(options, builder); }

        /// <inheritdoc />
        public ICounter Instance(CounterOptions options) { return Instance(options, () => _counterBuilder.Build()); }
    }
}