using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public class JsonHistogram : JsonMetric
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
            bool hasUserValues = this.LastUserValue != null || this.MinUserValue != null || this.MaxUserValue != null;

            yield return new JsonProperty("Name", this.Name);
            yield return new JsonProperty("Count", this.Count);
            yield return new JsonProperty("LastValue", this.LastValue);

            if (hasUserValues)
            {
                yield return new JsonProperty("LastUserValue", this.LastUserValue);
            }
            yield return new JsonProperty("Min", this.Min);
            if (hasUserValues)
            {
                yield return new JsonProperty("MinUserValue", this.MinUserValue);
            }
            yield return new JsonProperty("Max", this.Max);
            if (hasUserValues)
            {
                yield return new JsonProperty("MaxUserValue", this.MaxUserValue);
            }
            yield return new JsonProperty("Mean", this.Mean);
            if (hasUserValues)
            {
                yield return new JsonProperty("MaxUserValue", this.MaxUserValue);
            }

            yield return new JsonProperty("StdDev", this.StdDev);
            yield return new JsonProperty("Median", this.Median);
            yield return new JsonProperty("Percentile75", this.Percentile75);
            yield return new JsonProperty("Percentile95", this.Percentile95);
            yield return new JsonProperty("Percentile98", this.Percentile98);
            yield return new JsonProperty("Percentile99", this.Percentile99);
            yield return new JsonProperty("Percentile999", this.Percentile999);
            yield return new JsonProperty("SampleSize", this.SampleSize);
            yield return new JsonProperty("Unit", this.Unit);

            if (this.Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", this.Tags);
            }
        }

        public HistogramValueSource ToValueSource()
        {
            var histogramValue = new HistogramValue(this.Count, this.LastValue, this.LastUserValue,
                this.Max, this.MaxUserValue, this.Mean, this.Min, this.MinUserValue, this.StdDev, this.Median,
                this.Percentile75, this.Percentile95, this.Percentile98, this.Percentile99, this.Percentile999, this.SampleSize);

            return new HistogramValueSource(this.Name, ConstantValue.Provider(histogramValue), this.Unit, this.Tags);
        }
    }
}