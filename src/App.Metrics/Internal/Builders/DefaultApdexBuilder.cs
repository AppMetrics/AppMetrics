// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Clock;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Builders
{
    public class DefaultApdexBuilder : IBuildApdexMetrics
    {
        private readonly DefaultSamplingReservoirProvider _defaultSamplingReservoirProvider;

        public DefaultApdexBuilder(DefaultSamplingReservoirProvider defaultSamplingReservoirProvider)
        {
            _defaultSamplingReservoirProvider = defaultSamplingReservoirProvider;
        }

        /// <inheritdoc />
        public IApdexMetric Build(
            double apdexTSeconds,
            bool allowWarmup,
            IClock clock)
        {
            var reservoir = _defaultSamplingReservoirProvider.Instance();

            return new ApdexMetric(new ApdexProvider(reservoir, apdexTSeconds), clock, allowWarmup);
        }

        /// <inheritdoc />
        public IApdexMetric Build(
            Lazy<IReservoir> reservoir,
            double apdexTSeconds,
            bool allowWarmup,
            IClock clock)
        {
            if (reservoir == null)
            {
                reservoir = _defaultSamplingReservoirProvider.Instance();
            }

            return new ApdexMetric(new ApdexProvider(reservoir, apdexTSeconds), clock, allowWarmup);
        }
    }
}