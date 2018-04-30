// <copyright file="IBuildHistogramMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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