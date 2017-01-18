// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Internal.Builders
{
    public class DefaultHistogramBuilder : IBuildHistogramMetrics
    {
        /// <inheritdoc />
        public IHistogramMetric Instance(SamplingType samplingType, int sampleSize, double exponentialDecayFactor)
        {
            return new HistogramMetric(samplingType, sampleSize, exponentialDecayFactor);
        }

        /// <inheritdoc />
        public IHistogramMetric Instance(IReservoir reservoir)
        {
            return new HistogramMetric(reservoir);
        }
    }
}