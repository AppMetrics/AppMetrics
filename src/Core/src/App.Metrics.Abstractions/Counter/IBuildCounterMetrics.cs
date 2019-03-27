// <copyright file="IBuildCounterMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Counter
{
    public interface IBuildCounterMetrics
    {
        ICounterMetric Build();

        ICounterMetric Build<T>(Func<T> builder)
            where T : ICounterMetric;
    }
}