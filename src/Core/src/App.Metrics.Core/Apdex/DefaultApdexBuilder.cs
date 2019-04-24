// <copyright file="DefaultApdexBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
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
            Func<IReservoir> reservoir,
            double apdexTSeconds,
            bool allowWarmup,
            IClock clock)
        {
            if (reservoir == null)
            {
                reservoir = _defaultSamplingReservoirProvider.Instance;
            }

            return new DefaultApdexMetric(new ApdexProvider(reservoir(), apdexTSeconds), clock, allowWarmup);
        }
    }
}