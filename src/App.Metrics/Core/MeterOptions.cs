namespace App.Metrics.Core
{
    public class MeterOptions : MetricValueOptions
    {
        public MeterOptions()
        {
            RateUnit = TimeUnit.Milliseconds;
        }
        public TimeUnit RateUnit { get; set; }
    }
}