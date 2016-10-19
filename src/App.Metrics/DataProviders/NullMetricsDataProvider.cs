// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.MetricData;

namespace App.Metrics.DataProviders
{
    public sealed class NullMetricsDataProvider : IMetricsDataProvider
    {
        public MetricsData GetMetricsData(IMetricsContext metricsContext)
        {
            return MetricsData.Empty;
        }
    }
}