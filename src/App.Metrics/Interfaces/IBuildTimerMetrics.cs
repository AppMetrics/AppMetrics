// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Sampling.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Interfaces
{
    public interface IBuildTimerMetrics
    {
        ITimerMetric Instance(SamplingType samplingType, int sampleSize, double exponentialDecayFactor, IClock clock);

        ITimerMetric Instance(IHistogramMetric histogram, IClock clock);

        ITimerMetric Instance(IReservoir reservoir, IClock clock);
    }
}