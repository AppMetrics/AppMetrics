// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace App.Metrics.Json
{
    public interface IMetricsJsonBuilder
    {
        string MetricsMimeType { get; }

        Task<string> BuildJsonAsync(IMetricsContext metricsContext, IMetricsFilter filter);

        Task<string> BuildJsonAsync(IMetricsContext metricsContext, IMetricsFilter filter, bool indented);
    }
}