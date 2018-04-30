// <copyright file="IBuildMeterMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Meter
{
    public interface IBuildMeterMetrics
    {
        IMeterMetric Build(IClock clock);
    }
}