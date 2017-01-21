// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions;
using App.Metrics.Core.Interfaces;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Interfaces
{
    public interface IBuildTimerMetrics
    {
        ITimerMetric Build(IHistogramMetric histogram, IClock clock);

        ITimerMetric Build(Lazy<IReservoir> reservoir, IClock clock);
    }
}