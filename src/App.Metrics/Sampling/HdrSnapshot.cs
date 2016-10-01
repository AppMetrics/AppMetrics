// Written by Iulian Margarintescu
// 
// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

using System.Collections.Generic;
using System.Linq;
using App.Metrics.App_Packages.HdrHistogram;

namespace App.Metrics.Sampling
{
    internal sealed class HdrSnapshot : Snapshot
    {
        private readonly AbstractHistogram _histogram;

        public HdrSnapshot(AbstractHistogram histogram, long minValue, string minUserValue, long maxValue, string maxUserValue)
        {
            _histogram = histogram;
            Min = minValue;
            MinUserValue = minUserValue;
            Max = maxValue;
            MaxUserValue = maxUserValue;
        }

        public long Count => _histogram.getTotalCount();

        public long Max { get; }

        public string MaxUserValue { get; }

        public double Mean => _histogram.getMean();

        public double Median => _histogram.getValueAtPercentile(50);

        public long Min { get; }

        public string MinUserValue { get; }

        public double Percentile75 => _histogram.getValueAtPercentile(75);

        public double Percentile95 => _histogram.getValueAtPercentile(95);

        public double Percentile98 => _histogram.getValueAtPercentile(98);

        public double Percentile99 => _histogram.getValueAtPercentile(99);

        public double Percentile999 => _histogram.getValueAtPercentile(99.9);

        public int Size => _histogram.getEstimatedFootprintInBytes();

        public double StdDev => _histogram.getStdDeviation();

        public IEnumerable<long> Values
        {
            get { return _histogram.RecordedValues().Select(v => v.GetValueIteratedTo()); }
        }

        public double GetValue(double quantile)
        {
            return _histogram.getValueAtPercentile(quantile * 100);
        }
    }
}