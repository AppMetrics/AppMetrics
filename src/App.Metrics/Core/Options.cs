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

    public class CounterOptions : MetricValueOptions
    {
    }

    public class MeterOptions : MetricValueOptions
    {
        public MeterOptions()
        {
            RateUnit = TimeUnit.Milliseconds;
        }
        public TimeUnit RateUnit { get; set; }
    }

    public class GaugeOptions : MetricValueOptions
    {
    }

    public abstract class MetricValueWithSamplingOption : MetricValueOptions
    {
        public SamplingType SamplingType { get; set; }
    }

    public class TimerOptions : MetricValueWithSamplingOption
    {
        public TimerOptions()
        {
            DurationUnit = TimeUnit.Milliseconds;
            RateUnit = TimeUnit.Milliseconds;
            SamplingType = SamplingType.ExponentiallyDecaying;
        }
        public TimeUnit DurationUnit { get; set; }

        public TimeUnit RateUnit { get; set; }

    }

    public class HistogramOptions : MetricValueWithSamplingOption
    {
        public HistogramOptions()
        {            
            SamplingType = SamplingType.Default;    
        }
    }
}