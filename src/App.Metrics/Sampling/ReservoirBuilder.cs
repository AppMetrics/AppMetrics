// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Sampling
{
    public static class ReservoirBuilder
    {
        public static IReservoir Build(SamplingType samplingType, int sampleSize, double alpha)
        {
            while (true)
            {
                switch (samplingType)
                {
                    case SamplingType.HighDynamicRange:
                        return new HdrHistogramReservoir();
                    case SamplingType.ExponentiallyDecaying:
                        return new ExponentiallyDecayingReservoir(sampleSize, alpha);
                    case SamplingType.LongTerm:
                        return new UniformReservoir(sampleSize);
                    case SamplingType.SlidingWindow:
                        return new SlidingWindowReservoir(sampleSize);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(samplingType), samplingType, "Sampling type not implemented " + samplingType);
                }
            }
        }
    }
}