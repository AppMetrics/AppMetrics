// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Providers
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
            if (options.WithReservoir != null)
            {
                return Instance(options, () => _apdexBuidler.Build(options.WithReservoir(), options.ApdexTSeconds, options.AllowWarmup, _clock));
            }

            return _registry.Apdex(
                options,
                () =>
                    _apdexBuidler.Build(
                        options.SamplingType,
                        options.SampleSize,
                        options.ExponentialDecayFactor,
                        options.ApdexTSeconds,
                        options.AllowWarmup,
                        _clock));
        }

        /// <inheritdoc />
        public IApdex Instance<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric
        {
            return _registry.Apdex(options, builder);
        }
    }
}