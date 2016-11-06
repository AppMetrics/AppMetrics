using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public sealed class JsonGauge : JsonMetric
    {
        private double _value;

        public double? Value
        {
            get { return _value; }
            set { _value = value ?? double.NaN; }
        }

        public static JsonGauge FromGauge(MetricValueSource<double> gauge)
        {
            return new JsonGauge
            {
                Name = gauge.Name,
                Value = gauge.Value,
                Unit = gauge.Unit.Name,
                Tags = gauge.Tags.ToDictionary()
            };
        }
    }
}