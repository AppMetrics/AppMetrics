using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace App.Metrics.Data
// ReSharper restore CheckNamespace
{
    public static class MetricContextValueSourceEnumerableExtensions
    {
        public static IEnumerable<MetricsContextValueSource> FilterBy(this IEnumerable<MetricsContextValueSource> valueSources, IMetricsFilter filter)
        {
            return valueSources.Select(g => g.Filter(filter));
        }

        public static IEnumerable<MetricsContextValueSource> WhereNotEmpty(this IEnumerable<MetricsContextValueSource> valueSources)
        {
            return valueSources.Where(g => g.IsNotEmpty());
        }
    }
}