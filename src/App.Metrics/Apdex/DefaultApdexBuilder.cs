// <copyright file="DefaultApdexBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Apdex
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

            return new DefaultApdexMetric(new ApdexProvider(reservoir, apdexTSeconds), clock, allowWarmup);
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

            return new DefaultApdexMetric(new ApdexProvider(reservoir, apdexTSeconds), clock, allowWarmup);
        }
    }
}