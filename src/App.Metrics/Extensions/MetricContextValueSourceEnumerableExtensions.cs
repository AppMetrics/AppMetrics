// <copyright file="MetricContextValueSourceEnumerableExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Core;

// ReSharper disable CheckNamespace
namespace App.Metrics.Data
{
    // ReSharper restore CheckNamespace
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