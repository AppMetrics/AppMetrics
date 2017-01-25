// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.ReservoirSampling;

namespace App.Metrics.Histogram.Interfaces
{
    public interface IBuildHistogramMetrics
    {
        IHistogramMetric Build(Lazy<IReservoir> reservoir);
    }
}