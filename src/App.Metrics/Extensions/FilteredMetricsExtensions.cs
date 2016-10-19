// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.DataProviders;
using App.Metrics.MetricData;

namespace App.Metrics.Extensions
{
    public static class FilteredMetricsExtensions
    {
        public static IMetricsDataProvider WithFilter(this IMetricsDataProvider provider, IMetricsFilter filter)
        {
            if (filter == null)
            {
                return provider;
            }
            return new FilteredMetricsDataProvider(provider, filter);
        }
    }
}