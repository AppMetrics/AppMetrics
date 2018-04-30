// <copyright file="IGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Gauge
{
    public interface IGauge : IResetableMetric
    {
        void SetValue(double value);
    }
}