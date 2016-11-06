using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public static class HistogramConverterExtensions
    {
        public static HistogramValueSource FromJson(this JsonHistogram source)
        {
            var histogramValue = new HistogramValue(source.Count, source.LastValue, source.LastUserValue,
                source.Max, source.MaxUserValue, source.Mean, source.Min, source.MinUserValue, source.StdDev, source.Median,
                source.Percentile75, source.Percentile95, source.Percentile98, source.Percentile99, source.Percentile999, source.SampleSize);

            return new HistogramValueSource(source.Name, ConstantValue.Provider(histogramValue), source.Unit, source.Tags);
        }

        public static IEnumerable<JsonHistogram> ToJson(this IEnumerable<HistogramValueSource> source)
        {
            return source.Select(ToJson);
        }

        public static JsonHistogram ToJson(this HistogramValueSource source)
        {
            return new JsonHistogram
            {
                Name = source.Name,
                Count = source.Value.Count,
                Unit = source.Unit.Name,
                LastUserValue = source.Value.LastUserValue,
                LastValue = source.Value.LastValue,
                Max = source.Value.Max,
                MaxUserValue = source.Value.MaxUserValue,
                Mean = source.Value.Mean,
                Median = source.Value.Median,
                Min = source.Value.Min,
                MinUserValue = source.Value.MinUserValue,
                Percentile75 = source.Value.Percentile75,
                Percentile95 = source.Value.Percentile95,
                Percentile98 = source.Value.Percentile98,
                Percentile99 = source.Value.Percentile99,
                Percentile999 = source.Value.Percentile999,
                SampleSize = source.Value.SampleSize,
                StdDev = source.Value.StdDev,
                Tags = source.Tags.ToDictionary()
            };
        }

        public static IEnumerable<HistogramValueSource> FromJson(this IEnumerable<JsonHistogram> source)
        {
            return source.Select(x => x.FromJson());
        }
    }
}