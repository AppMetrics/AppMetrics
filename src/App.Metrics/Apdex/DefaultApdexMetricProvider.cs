// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Registry.Abstractions;

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
        public IApdex Instance<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric
        {
            return _registry.Apdex(options, builder);
        }
    }
}