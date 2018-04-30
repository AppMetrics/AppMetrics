// <copyright file="DefaultApdexMetricProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Registry;

namespace App.Metrics.Apdex
{
    public class DefaultApdexMetricProvider : IProvideApdexMetrics
    {
        private readonly IBuildApdexMetrics _apdexBuidler;
        private readonly IClock _clock;
        private readonly IMetricsRegistry _registry;

        public DefaultApdexMetricProvider(
            IBuildApdexMetrics apdexBuidler,
            IMetricsRegistry registry,
            IClock clock)
        {
            _registry = registry;
            _clock = clock;
            _apdexBuidler = apdexBuidler;
        }

        /// <inheritdoc />
        public IApdex Instance(ApdexOptions options)
        {
            return Instance(options, () => _apdexBuidler.Build(options.Reservoir, options.ApdexTSeconds, options.AllowWarmup, _clock));
        }

        /// <inheritdoc />
        public IApdex Instance(ApdexOptions options, MetricTags tags)
        {
            return Instance(options, tags, () => _apdexBuidler.Build(options.Reservoir, options.ApdexTSeconds, options.AllowWarmup, _clock));
        }

        /// <inheritdoc />
        public IApdex Instance<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric
        {
            return _registry.Apdex(options, builder);
        }

        /// <inheritdoc />
        public IApdex Instance<T>(ApdexOptions options, MetricTags tags, Func<T> builder)
            where T : IApdexMetric
        {
            return _registry.Apdex(options, tags, builder);
        }
    }
}