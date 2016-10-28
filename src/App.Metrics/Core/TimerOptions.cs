namespace App.Metrics.Core
{
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
}