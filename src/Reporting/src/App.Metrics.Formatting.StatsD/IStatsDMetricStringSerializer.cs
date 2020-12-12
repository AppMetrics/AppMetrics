// <copyright file="IStatsDMetricStringSerializer.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Formatting.StatsD.Internal;

namespace App.Metrics.Formatting.StatsD
{
    public interface IStatsDMetricStringSerializer
    {
        string Serialize(StatsDPoint point, MetricsStatsDOptions options);
    }
}
