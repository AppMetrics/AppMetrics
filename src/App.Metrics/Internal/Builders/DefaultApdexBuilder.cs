// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Sampling.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Builders
{
    public class DefaultApdexBuilder : IBuildApdexMetrics
    {
        /// <inheritdoc />
        public IApdexMetric Instance(
            SamplingType samplingType,
            int sampleSize,
            double exponentialDecayFactor,
            double apdexTSeconds,
            bool allowWarmup,
            IClock clock)
        {
            return new ApdexMetric(
                samplingType,
                sampleSize,
                exponentialDecayFactor,
                clock,
                apdexTSeconds,
                allowWarmup);
        }

        /// <inheritdoc />
        public IApdexMetric Instance(
            IReservoir reservoir,
            double apdexTSeconds,
            bool allowWarmup,
            IClock clock) { return new ApdexMetric(new ApdexProvider(reservoir, apdexTSeconds), clock, allowWarmup); }
    }
}