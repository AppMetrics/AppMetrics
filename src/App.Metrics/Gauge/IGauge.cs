// <copyright file="IGauge.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Abstractions;

namespace App.Metrics.Gauge
{
    public interface IGauge : IResetableMetric
    {
        void SetValue(double value);
    }
}