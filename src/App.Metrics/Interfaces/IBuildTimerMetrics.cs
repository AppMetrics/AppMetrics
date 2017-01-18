// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Sampling.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Interfaces
{
    public interface IBuildTimerMetrics
    {
        ITimerMetric Build(SamplingType samplingType, int sampleSize, double exponentialDecayFactor, IClock clock);

        ITimerMetric Build(IHistogramMetric histogram, IClock clock);

        ITimerMetric Build(IReservoir reservoir, IClock clock);
    }
}