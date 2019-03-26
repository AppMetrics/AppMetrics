// <copyright file="DefaultCounterMetricProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Registry;

namespace App.Metrics.Counter
{
    public class DefaultCounterMetricProvider : IProvideCounterMetrics
    {
        private readonly IBuildCounterMetrics _counterBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultCounterMetricProvider" /> class.
        /// </summary>
        /// <param name="counterBuilder">The counter builder.</param>
        /// <param name="registry">The registry.</param>
        public DefaultCounterMetricProvider(IBuildCounterMetrics counterBuilder, IMetricsRegistry registry)
        {
            _registry = registry;
            _counterBuilder = counterBuilder;
        }

        /// <inheritdoc />
        public ICounter Instance<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric
        {
            return _registry.Counter(options, builder);
        }

        /// <inheritdoc />
        public ICounter Instance(CounterOptions options, MetricTags tags)
        {
            return Instance(options, tags, () => _counterBuilder.Build());
        }

        /// <inheritdoc />
        public ICounter Instance<T>(CounterOptions options, MetricTags tags, Func<T> builder)
            where T : ICounterMetric
        {
            return _registry.Counter(options, tags, builder);
        }

        /// <inheritdoc />
        public ICounter Instance(CounterOptions options)
        {
            return Instance(options, () => _counterBuilder.Build());
        }
    }
}