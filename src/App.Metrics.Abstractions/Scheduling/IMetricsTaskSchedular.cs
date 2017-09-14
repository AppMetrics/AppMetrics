// <copyright file="IMetricsTaskSchedular.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Scheduling
{
    public interface IMetricsTaskSchedular : IDisposable
    {
        void Start(TimeSpan interval);
    }
}