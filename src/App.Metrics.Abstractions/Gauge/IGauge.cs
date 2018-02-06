// <copyright file="IGauge.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Gauge
{
    public interface IGauge : IResetableMetric
    {
        void SetValue(double value);
    }
}