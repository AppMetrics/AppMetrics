// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Infrastructure;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public interface IMetricsJsonBuilder
    {
        string MetricsMimeType { get; }

        string BuildJson(IMetricsContext metricsContext,
            EnvironmentInfo environmentInfo,
             IMetricsFilter filter);

        string BuildJson(IMetricsContext metricsContext, EnvironmentInfo environmentInfo,
            IMetricsFilter filter,
            bool indented);
    }
}