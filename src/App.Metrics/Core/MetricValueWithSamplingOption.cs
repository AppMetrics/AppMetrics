namespace App.Metrics.Core
{
    public abstract class MetricValueWithSamplingOption : MetricValueOptions
    {
        public SamplingType SamplingType { get; set; }
    }
}