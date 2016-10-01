using App.Metrics.Sampling;

namespace App.Metrics.MetricData
{
    /// <summary>
    ///     The value reported by a Histogram Metric
    /// </summary>
    public sealed class HistogramValue
    {
        public readonly long Count;
        public readonly string LastUserValue;

        public readonly double LastValue;
        public readonly double Max;
        public readonly string MaxUserValue;
        public readonly double Mean;
        public readonly double Median;
        public readonly double Min;
        public readonly string MinUserValue;
        public readonly double Percentile75;
        public readonly double Percentile95;
        public readonly double Percentile98;
        public readonly double Percentile99;
        public readonly double Percentile999;
        public readonly int SampleSize;
        public readonly double StdDev;

        public HistogramValue(double lastValue, string lastUserValue, Snapshot snapshot)
            : this(snapshot.Count,
                lastValue,
                lastUserValue,
                snapshot.Max,
                snapshot.MaxUserValue,
                snapshot.Mean,
                snapshot.Min,
                snapshot.MinUserValue,
                snapshot.StdDev,
                snapshot.Median,
                snapshot.Percentile75,
                snapshot.Percentile95,
                snapshot.Percentile98,
                snapshot.Percentile99,
                snapshot.Percentile999,
                snapshot.Size)
        {
        }

        public HistogramValue(long count,
            double lastValue,
            string lastUserValue,
            double max,
            string maxUserValue,
            double mean,
            double min,
            string minUserValue,
            double stdDev,
            double median,
            double percentile75,
            double percentile95,
            double percentile98,
            double percentile99,
            double percentile999,
            int sampleSize)
        {
            this.Count = count;
            this.LastValue = lastValue;
            this.LastUserValue = lastUserValue;
            this.Max = max;
            this.MaxUserValue = maxUserValue;
            this.Mean = mean;
            this.Min = min;
            this.MinUserValue = minUserValue;
            this.StdDev = stdDev;
            this.Median = median;
            this.Percentile75 = percentile75;
            this.Percentile95 = percentile95;
            this.Percentile98 = percentile98;
            this.Percentile99 = percentile99;
            this.Percentile999 = percentile999;
            this.SampleSize = sampleSize;
        }

        public HistogramValue Scale(double factor)
        {
            if (factor == 1.0d)
            {
                return this;
            }

            return new HistogramValue(this.Count,
                this.LastValue * factor,
                this.LastUserValue,
                this.Max * factor,
                this.MaxUserValue,
                this.Mean * factor,
                this.Min * factor,
                this.MinUserValue,
                this.StdDev * factor,
                this.Median * factor,
                this.Percentile75 * factor,
                this.Percentile95 * factor,
                this.Percentile98 * factor,
                this.Percentile99 * factor,
                this.Percentile999 * factor,
                this.SampleSize);
        }
    }

    /// <summary>
    ///     Combines the value of the histogram with the defined unit for the value.
    /// </summary>
    public sealed class HistogramValueSource : MetricValueSource<HistogramValue>
    {
        public HistogramValueSource(string name, MetricValueProvider<HistogramValue> valueProvider, Unit unit, MetricTags tags)
            : base(name, valueProvider, unit, tags)
        {
        }
    }
}