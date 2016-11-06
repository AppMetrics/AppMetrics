using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public static class GaugeConverterExtensions
    {
        public static GaugeValueSource FromJson(this JsonGauge source)
        {
            return source.Value.HasValue
                ? new GaugeValueSource(source.Name, ConstantValue.Provider(source.Value.Value), source.Unit, source.Tags)
                : new GaugeValueSource(source.Name, null, source.Unit, source.Tags);
        }

        public static IEnumerable<JsonGauge> ToJson(this IEnumerable<GaugeValueSource> source)
        {
            return source.Select(ToJson);
        }

        public static JsonGauge ToJson(this GaugeValueSource source)
        {
            return new JsonGauge
            {
                Name = source.Name,
                Value = source.Value,
                Unit = source.Unit.Name,
                Tags = source.Tags.ToDictionary()
            };
        }

        public static IEnumerable<GaugeValueSource> FromJson(this IEnumerable<JsonGauge> source)
        {
            return source.Select(x => x.FromJson());
        }
    }
}