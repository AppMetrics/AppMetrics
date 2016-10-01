using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Sampling
{
    public sealed class UniformSnapshot : Snapshot
    {
        private readonly long[] values;

        public UniformSnapshot(long count, IEnumerable<long> values, bool valuesAreSorted = false, string minUserValue = null, string maxUserValue = null)
        {
            this.Count = count;
            this.values = values.ToArray();
            if (!valuesAreSorted)
            {
                Array.Sort(this.values);
            }
            this.MinUserValue = minUserValue;
            this.MaxUserValue = maxUserValue;
        }

        public long Count { get; }

        public int Size => this.values.Length;

        public long Max => this.values.LastOrDefault();
        public long Min => this.values.FirstOrDefault();

        public string MaxUserValue { get; }
        public string MinUserValue { get; }

        public double Mean => Size == 0 ? 0.0 : this.values.Average();

        public double StdDev
        {
            get
            {
                if (this.Size <= 1)
                {
                    return 0;
                }

                var avg = this.values.Average();
                var sum = this.values.Sum(d => Math.Pow(d - avg, 2));

                return Math.Sqrt((sum) / (this.values.Length - 1));
            }
        }

        public double Median => GetValue(0.5d);
        public double Percentile75 => GetValue(0.75d);
        public double Percentile95 => GetValue(0.95d);
        public double Percentile98 => GetValue(0.98d);
        public double Percentile99 => GetValue(0.99d);
        public double Percentile999 => GetValue(0.999d);

        public IEnumerable<long> Values => this.values;

        public double GetValue(double quantile)
        {
            if (quantile < 0.0 || quantile > 1.0 || double.IsNaN(quantile))
            {
                throw new ArgumentException($"{quantile} is not in [0..1]");
            }

            if (this.Size == 0)
            {
                return 0;
            }

            var pos = quantile * (this.values.Length + 1);
            var index = (int)pos;

            if (index < 1)
            {
                return this.values[0];
            }

            if (index >= this.values.Length)
            {
                return this.values[this.values.Length - 1];
            }

            double lower = this.values[index - 1];
            double upper = this.values[index];

            return lower + (pos - Math.Floor(pos)) * (upper - lower);
        }
    }
}
