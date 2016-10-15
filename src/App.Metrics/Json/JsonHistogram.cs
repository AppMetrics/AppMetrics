using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public sealed class JsonHistogram : JsonMetric
    {
        public long Count { get; set; }

        public string LastUserValue { get; set; }

        public double LastValue { get; set; }

        public double Max { get; set; }

        public string MaxUserValue { get; set; }

        public double Mean { get; set; }

        public double Median { get; set; }

        public double Min { get; set; }

        public string MinUserValue { get; set; }

        public double Percentile75 { get; set; }

        public double Percentile95 { get; set; }

        public double Percentile98 { get; set; }

        public double Percentile99 { get; set; }

        public double Percentile999 { get; set; }

        public int SampleSize { get; set; }

        public double StdDev { get; set; }

        public static JsonHistogram FromHistogram(MetricValueSource<HistogramValue> histogram)
        {
            return new JsonHistogram
            {
                Name = histogram.Name,
                Count = histogram.Value.Count,
                LastValue = histogram.Value.LastValue,
                LastUserValue = histogram.Value.LastUserValue,
                Max = histogram.Value.Max,
                MaxUserValue = histogram.Value.MaxUserValue,
                Mean = histogram.Value.Mean,
                Min = histogram.Value.Min,
                MinUserValue = histogram.Value.MinUserValue,
                StdDev = histogram.Value.StdDev,
                Median = histogram.Value.Median,
                Percentile75 = histogram.Value.Percentile75,
                Percentile95 = histogram.Value.Percentile95,
                Percentile98 = histogram.Value.Percentile98,
                Percentile99 = histogram.Value.Percentile99,
                Percentile999 = histogram.Value.Percentile999,
                SampleSize = histogram.Value.SampleSize,
                Unit = histogram.Unit.Name,
                Tags = histogram.Tags
            };
        }

        public JsonObject ToJsonObject()
        {
            return new JsonObject(ToJsonProperties());
        }

        public IEnumerable<JsonProperty> ToJsonProperties()
        {
            yield return new JsonProperty("Name", Name);
            yield return new JsonProperty("Count", Count);
            yield return new JsonProperty("LastValue", LastValue);

            foreach (var userValueProperty in UserValueProperties())
            {
                yield return userValueProperty;
            }

            yield return new JsonProperty("StdDev", StdDev);
            yield return new JsonProperty("Median", Median);
            yield return new JsonProperty("Percentile75", Percentile75);
            yield return new JsonProperty("Percentile95", Percentile95);
            yield return new JsonProperty("Percentile98", Percentile98);
            yield return new JsonProperty("Percentile99", Percentile99);
            yield return new JsonProperty("Percentile999", Percentile999);
            yield return new JsonProperty("SampleSize", SampleSize);
            yield return new JsonProperty("Unit", Unit);

            if (Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", Tags);
            }
        }

        private bool HasUserValues => LastUserValue != null || MinUserValue != null || MaxUserValue != null;

        public HistogramValueSource ToValueSource()
        {
            var histogramValue = new HistogramValue(Count, LastValue, LastUserValue,
                Max, MaxUserValue, Mean, Min, MinUserValue, StdDev, Median,
                Percentile75, Percentile95, Percentile98, Percentile99, Percentile999, SampleSize);

            return new HistogramValueSource(Name, ConstantValue.Provider(histogramValue), Unit, Tags);
        }

        private IEnumerable<JsonProperty> UserValueProperties()
        {
            if (HasUserValues)
            {
                yield return new JsonProperty("LastUserValue", LastUserValue);
            }
            yield return new JsonProperty("Min", Min);

            if (HasUserValues)
            {
                yield return new JsonProperty("MinUserValue", MinUserValue);
            }
            yield return new JsonProperty("Max", Max);

            if (HasUserValues)
            {
                yield return new JsonProperty("MaxUserValue", MaxUserValue);
            }
            yield return new JsonProperty("Mean", Mean);

            if (HasUserValues)
            {
                yield return new JsonProperty("MaxUserValue", MaxUserValue);
            }
        }
    }
}