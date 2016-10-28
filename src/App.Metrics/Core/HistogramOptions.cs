namespace App.Metrics.Core
{
    public class HistogramOptions : MetricValueWithSamplingOption
    {
        public HistogramOptions()
        {            
            SamplingType = SamplingType.Default;    
        }
    }
}