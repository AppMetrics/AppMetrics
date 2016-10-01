using System.Collections.Generic;
using App.Metrics.App_Packages.HdrHistogram;
using System.Linq;

namespace App.Metrics.Sampling
{
    internal sealed class HdrSnapshot : Snapshot
    {
        private readonly AbstractHistogram histogram;

        public HdrSnapshot(AbstractHistogram histogram, long minValue, string minUserValue, long maxValue, string maxUserValue)
        {
            this.histogram = histogram;
            this.Min = minValue;
            this.MinUserValue = minUserValue;
            this.Max = maxValue;
            this.MaxUserValue = maxUserValue;
        }

        public IEnumerable<long> Values
        {
            get { return this.histogram.RecordedValues().Select(v => v.getValueIteratedTo()); }
        }

        public double GetValue(double quantile)
        {
            return this.histogram.getValueAtPercentile(quantile * 100);
        }

        public long Min { get; }
        public string MinUserValue { get; }
        public long Max { get; }
        public string MaxUserValue { get; }

        public long Count => this.histogram.getTotalCount();
        public double Mean => this.histogram.getMean();
        public double StdDev => this.histogram.getStdDeviation();

        public double Median => this.histogram.getValueAtPercentile(50);
        public double Percentile75 => this.histogram.getValueAtPercentile(75);
        public double Percentile95 => this.histogram.getValueAtPercentile(95);
        public double Percentile98 => this.histogram.getValueAtPercentile(98);
        public double Percentile99 => this.histogram.getValueAtPercentile(99);
        public double Percentile999 => this.histogram.getValueAtPercentile(99.9);
        
        public int Size => this.histogram.getEstimatedFootprintInBytes();
    }
}