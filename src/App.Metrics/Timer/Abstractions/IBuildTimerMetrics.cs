// <copyright file="IBuildTimerMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Abstractions.ReservoirSampling;

namespace App.Metrics.Timer.Abstractions
{
    public interface IBuildTimerMetrics
    {
        ITimerMetric Build(IHistogramMetric histogram, IClock clock);

        ITimerMetric Build(Lazy<IReservoir> reservoir, IClock clock);

        ITimerMetric Build(IHistogramMetric histogram, IMeterMetric meter, IClock clock);

        ITimerMetric Build(Lazy<IReservoir> reservoir, IMeterMetric meter, IClock clock);
    }
}