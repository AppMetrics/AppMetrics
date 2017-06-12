// <copyright file="IBuildMeterMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Meter
{
    public interface IBuildMeterMetrics
    {
        IMeterMetric Build(IClock clock);
    }
}