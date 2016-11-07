using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics.Extensions
{
    public static class GaugeExtensions
    {
        public static GaugeValueSource ToMetricValueSource(this Gauge source)
        {
            return source.Value.HasValue
                ? new GaugeValueSource(source.Name, ConstantValue.Provider(source.Value.Value), source.Unit, source.Tags)
                : new GaugeValueSource(source.Name, null, source.Unit, source.Tags);
        }

        public static IEnumerable<Gauge> ToMetric(this IEnumerable<GaugeValueSource> source)
        {
            return source.Select(ToMetric);
        }

        public static Gauge ToMetric(this GaugeValueSource source)
        {
            return new Gauge
            {
                Name = source.Name,
                Value = source.Value,
                Unit = source.Unit.Name,
                Tags = source.Tags.ToDictionary()
            };
        }

        public static IEnumerable<GaugeValueSource> ToMetricValueSource(this IEnumerable<Gauge> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}