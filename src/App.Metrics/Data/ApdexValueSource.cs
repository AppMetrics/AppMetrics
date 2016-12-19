using App.Metrics.Data.Interfaces;

namespace App.Metrics.Data
{
    public sealed class ApdexValueSource : MetricValueSource<ApdexValue>
    {
        public ApdexValueSource(string name, IMetricValueProvider<ApdexValue> value,
            MetricTags tags,
            bool resetOnReporting = false)
            : base(name, value, Unit.Results, tags)
        {
            ResetOnReporting = resetOnReporting;
        }

        public bool ResetOnReporting { get; private set; }
    }
}