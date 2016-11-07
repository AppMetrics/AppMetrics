using App.Metrics.Data;

namespace App.Metrics
{
    public sealed class Gauge : Metric
    {
        private double _value;

        public double? Value
        {
            get { return _value; }
            set { _value = value ?? double.NaN; }
        }

        public static Gauge FromGauge(MetricValueSource<double> gauge)
        {
            return new Gauge
            {
                Name = gauge.Name,
                Value = gauge.Value,
                Unit = gauge.Unit.Name,
                Tags = gauge.Tags.ToDictionary()
            };
        }
    }
}