// <copyright file="IBuildCounterMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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