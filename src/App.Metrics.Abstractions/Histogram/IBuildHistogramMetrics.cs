// <copyright file="IBuildHistogramMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Histogram
{
    public interface IBuildHistogramMetrics
    {
        IHistogramMetric Build(Func<IReservoir> setupReservoir);
    }
}