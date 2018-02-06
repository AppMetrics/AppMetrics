// <copyright file="MetricContextValueSourceEnumerableExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Filters;

namespace App.Metrics
{
    public static class MetricContextValueSourceEnumerableExtensions
    {
        public static IEnumerable<MetricsContextValueSource> FilterBy(this IEnumerable<MetricsContextValueSource> valueSources, IFilterMetrics filter)
        {
            return valueSources.Select(g => g.Filter(filter));
        }

        public static IEnumerable<MetricsContextValueSource> WhereNotEmpty(this IEnumerable<MetricsContextValueSource> valueSources)
        {
            return valueSources.Where(g => g.IsNotEmpty());
        }
    }
}