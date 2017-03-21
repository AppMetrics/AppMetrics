// <copyright file="IBuildMeterMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.MetricTypes;

namespace App.Metrics.Meter.Abstractions
{
    public interface IBuildMeterMetrics
    {
        IMeterMetric Build(IClock clock);
    }
}