namespace App.Metrics.Data
{
    /// <summary>
    ///     Combines the value for a counter with the defined unit for the value.
    /// </summary>
    public sealed class CounterValueSource : MetricValueSource<CounterValue>
    {
        public CounterValueSource(string name, IMetricValueProvider<CounterValue> value, Unit unit, MetricTags tags)
            : base(name, value, unit, tags)
        {
        }
    }
}