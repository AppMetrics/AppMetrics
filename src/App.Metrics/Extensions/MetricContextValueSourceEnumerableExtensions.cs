// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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