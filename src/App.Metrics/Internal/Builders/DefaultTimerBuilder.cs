// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Sampling.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Builders
{
    public class DefaultTimerBuilder : IBuildTimerMetrics
    {
        /// <inheritdoc />
        public ITimerMetric Instance(SamplingType samplingType, int sampleSize, double exponentialDecayFactor, IClock clock)
        {
            return new TimerMetric(samplingType, sampleSize, exponentialDecayFactor, clock);
        }

        /// <inheritdoc />
        public ITimerMetric Instance(IHistogramMetric histogram, IClock clock)
        {
            return new TimerMetric(histogram, clock);
        }

        /// <inheritdoc />
        public ITimerMetric Instance(IReservoir reservoir, IClock clock)
        {
            return new TimerMetric(reservoir, clock);
        }
    }
}