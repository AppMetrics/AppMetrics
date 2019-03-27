// <copyright file="HistogramValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Histogram
{
    /// <summary>
    ///     The value reported by a Histogram Metric
    /// </summary>
    public sealed class HistogramValue
    {
        public HistogramValue(double lastValue, string lastUserValue, IReservoirSnapshot snapshot)
            : this(
                snapshot.Count,
                snapshot.Sum,
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
                snapshot.Size) { }

        public HistogramValue(
            long count,
            double sum,
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
            Count = count;
            Sum = sum;
            LastValue = lastValue;
            LastUserValue = lastUserValue;
            Max = max;
            MaxUserValue = maxUserValue;
            Mean = mean;
            Min = min;
            MinUserValue = minUserValue;
            StdDev = stdDev;
            Median = median;
            Percentile75 = percentile75;
            Percentile95 = percentile95;
            Percentile98 = percentile98;
            Percentile99 = percentile99;
            Percentile999 = percentile999;
            SampleSize = sampleSize;
        }

        public long Count { get; }

        public double Sum { get; }

        public string LastUserValue { get; }

        public double LastValue { get; }

        public double Max { get; }

        public string MaxUserValue { get; }

        public double Mean { get; }

        public double Median { get; }

        public double Min { get; }

        public string MinUserValue { get; }

        public double Percentile75 { get; }

        public double Percentile95 { get; }

        public double Percentile98 { get; }

        public double Percentile99 { get; }

        public double Percentile999 { get; }

        public int SampleSize { get; }

        public double StdDev { get; }

        public HistogramValue Scale(double factor)
        {
            if (Math.Abs(factor - 1.0d) < 0.001)
            {
                return this;
            }

            return new HistogramValue(
                Count,
                Sum * factor,
                LastValue * factor,
                LastUserValue,
                Max * factor,
                MaxUserValue,
                Mean * factor,
                Min * factor,
                MinUserValue,
                StdDev * factor,
                Median * factor,
                Percentile75 * factor,
                Percentile95 * factor,
                Percentile98 * factor,
                Percentile99 * factor,
                Percentile999 * factor,
                SampleSize);
        }
    }
}