// <copyright file="IBuildApdexMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.ReservoirSampling;

namespace App.Metrics.Apdex.Abstractions
{
    public interface IBuildApdexMetrics
    {
        IApdexMetric Build(double apdexTSeconds, bool allowWarmup, IClock clock);

        IApdexMetric Build(Lazy<IReservoir> reservoir, double apdexTSeconds, bool allowWarmup, IClock clock);
    }
}