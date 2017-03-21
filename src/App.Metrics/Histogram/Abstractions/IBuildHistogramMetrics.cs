// <copyright file="IBuildHistogramMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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