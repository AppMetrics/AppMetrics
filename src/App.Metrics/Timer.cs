namespace App.Metrics
{
    public sealed class Timer : Metric
    {
        public long ActiveSessions { get; set; }

        public long Count { get; set; }

        public string DurationUnit { get; set; }

        public HistogramData Histogram { get; set; }

        public RateData Rate { get; set; }

        public string RateUnit { get; set; }

        public long TotalTime { get; set; }

        public class HistogramData
        {
            public string LastUserValue { get; set; }

            public double LastValue { get; set; }

            public double Max { get; set; }

            public string MaxUserValue { get; set; }

            public double Mean { get; set; }

            public double Median { get; set; }

            public double Min { get; set; }

            public string MinUserValue { get; set; }

            public double Percentile75 { get; set; }

            public double Percentile95 { get; set; }

            public double Percentile98 { get; set; }

            public double Percentile99 { get; set; }

            public double Percentile999 { get; set; }

            public int SampleSize { get; set; }

            public double StdDev { get; set; }
        }

        public class RateData
        {
            public double FifteenMinuteRate { get; set; }

            public double FiveMinuteRate { get; set; }

            public double MeanRate { get; set; }

            public double OneMinuteRate { get; set; }
        }
    }
}