// <copyright file="IBuildTimerMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Timer
{
    public interface IBuildTimerMetrics
    {
        ITimerMetric Build(IHistogramMetric histogram, IClock clock);

        ITimerMetric Build(Func<IReservoir> reservoir, IClock clock);

        ITimerMetric Build(IHistogramMetric histogram, IMeterMetric meter, IClock clock);

        ITimerMetric Build(Func<IReservoir> reservoir, IMeterMetric meter, IClock clock);
    }
}