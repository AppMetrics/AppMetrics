// Written by Iulian Margarintescu
// 
// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Sampling
{
    public struct WeightedSample
    {
        public readonly string UserValue;
        public readonly long Value;
        public readonly double Weight;

        public WeightedSample(long value, string userValue, double weight)
        {
            this.Value = value;
            this.UserValue = userValue;
            this.Weight = weight;
        }
    }

    public sealed class WeightedSnapshot : Snapshot
    {
        private readonly double[] normWeights;
        private readonly double[] quantiles;
        private readonly long[] values;

        public WeightedSnapshot(long count, IEnumerable<WeightedSample> values)
        {
            this.Count = count;
            var sample = values.ToArray();
            Array.Sort(sample, WeightedSampleComparer.Instance);

            var sumWeight = sample.Sum(s => s.Weight);

            this.values = new long[sample.Length];
            this.normWeights = new double[sample.Length];
            this.quantiles = new double[sample.Length];

            for (var i = 0; i < sample.Length; i++)
            {
                this.values[i] = sample[i].Value;
                this.normWeights[i] = sample[i].Weight / sumWeight;
                if (i > 0)
                {
                    this.quantiles[i] = this.quantiles[i - 1] + this.normWeights[i - 1];
                }
            }

            this.MinUserValue = sample.Select(s => s.UserValue).FirstOrDefault();
            this.MaxUserValue = sample.Select(s => s.UserValue).LastOrDefault();
        }

        public long Count { get; }

        public long Max => this.values.LastOrDefault();

        public string MaxUserValue { get; }

        public double Mean
        {
            get
            {
                if (this.values.Length == 0)
                {
                    return 0.0;
                }

                double sum = 0;
                for (var i = 0; i < this.values.Length; i++)
                {
                    sum += this.values[i] * this.normWeights[i];
                }
                return sum;
            }
        }

        public double Median => GetValue(0.5d);

        public long Min => this.values.FirstOrDefault();

        public string MinUserValue { get; }

        public double Percentile75 => GetValue(0.75d);

        public double Percentile95 => GetValue(0.95d);

        public double Percentile98 => GetValue(0.98d);

        public double Percentile99 => GetValue(0.99d);

        public double Percentile999 => GetValue(0.999d);

        public int Size => this.values.Length;

        public double StdDev
        {
            get
            {
                if (this.Size <= 1)
                {
                    return 0;
                }

                var mean = this.Mean;
                double variance = 0;

                for (var i = 0; i < this.values.Length; i++)
                {
                    var diff = this.values[i] - mean;
                    variance += this.normWeights[i] * diff * diff;
                }

                return Math.Sqrt(variance);
            }
        }

        public IEnumerable<long> Values => this.values;

        public double GetValue(double quantile)
        {
            if (quantile < 0.0 || quantile > 1.0 || double.IsNaN(quantile))
            {
                throw new ArgumentException($"{quantile} is not in [0..1]");
            }

            if (Size == 0)
            {
                return 0;
            }

            var posx = Array.BinarySearch(this.quantiles, quantile);
            if (posx < 0)
            {
                posx = ~posx - 1;
            }

            if (posx < 1)
            {
                return this.values[0];
            }

            return posx >= this.values.Length ? this.values[this.values.Length - 1] : this.values[posx];
        }

        private class WeightedSampleComparer : IComparer<WeightedSample>
        {
            public static readonly IComparer<WeightedSample> Instance = new WeightedSampleComparer();

            public int Compare(WeightedSample x, WeightedSample y)
            {
                return Comparer<long>.Default.Compare(x.Value, y.Value);
            }
        }
    }
}