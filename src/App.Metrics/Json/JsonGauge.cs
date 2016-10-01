using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public class JsonGauge : JsonMetric
    {
        private double value;

        public double? Value
        {
            get { return this.value; }
            set { this.value = value ?? double.NaN; }
        }

        public static JsonGauge FromGauge(MetricValueSource<double> gauge)
        {
            return new JsonGauge
            {
                Name = gauge.Name,
                Value = gauge.Value,
                Unit = gauge.Unit.Name,
                Tags = gauge.Tags
            };
        }

        public JsonObject ToJsonObject()
        {
            return new JsonObject(ToJsonProperties());
        }

        public IEnumerable<JsonProperty> ToJsonProperties()
        {
            yield return new JsonProperty("Name", this.Name);
            yield return new JsonProperty("Value", this.Value.Value);
            yield return new JsonProperty("Unit", this.Unit);

            if (this.Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", this.Tags);
            }
        }

        public GaugeValueSource ToValueSource()
        {
            return new GaugeValueSource(this.Name, ConstantValue.Provider(this.Value.Value), this.Unit, this.Tags);
        }
    }
}