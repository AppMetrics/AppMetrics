// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Abstractions.ReservoirSampling;

namespace App.Metrics.Histogram.Abstractions
{
    public interface IBuildHistogramMetrics
    {
        IHistogramMetric Build(Lazy<IReservoir> reservoir);
    }
}