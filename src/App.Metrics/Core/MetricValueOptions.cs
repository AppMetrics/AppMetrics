namespace App.Metrics.Core
{
    public abstract class MetricValueOptions
    {
        protected MetricValueOptions()
        {
            Tags = MetricTags.None;
            MeasurementUnit = Unit.None;
        }

        public string GroupName { get; set; }

        public Unit MeasurementUnit { get; set; }

        public string Name { get; set; }

        public MetricTags Tags { get; set; }
    }
}