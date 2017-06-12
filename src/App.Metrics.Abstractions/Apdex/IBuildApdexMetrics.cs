// <copyright file="IBuildApdexMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Apdex
{
    public interface IBuildApdexMetrics
    {
        IApdexMetric Build(double apdexTSeconds, bool allowWarmup, IClock clock);

        IApdexMetric Build(Func<IReservoir> reservoir, double apdexTSeconds, bool allowWarmup, IClock clock);
    }
}