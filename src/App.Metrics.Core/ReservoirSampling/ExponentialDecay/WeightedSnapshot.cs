// <copyright file="WeightedSnapshot.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.ReservoirSampling.ExponentialDecay
{
    public sealed class WeightedSnapshot : IReservoirSnapshot
    {
        private readonly double[] _normWeights;
        private readonly double[] _quantiles;
        private readonly long[] _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedSnapshot" /> class.
        /// </summary>
        /// <param name="count">The count of all observed values.</param>
        /// <param name="sum">The sum of all observed values.</param>
        /// <param name="values">The values within the sample set.</param>
        public WeightedSnapshot(long count, double sum, IEnumerable<WeightedSample> values)
        {
            Count = count;
            Sum = sum;
            var sample = values.ToArray();
            Array.Sort(sample, WeightedSampleComparer.Instance);

            var sumWeight = sample.Sum(s => s.Weight);

            _values = new long[sample.Length];
            _normWeights = new double[sample.Length];
            _quantiles = new double[sample.Length];

            for (var i = 0; i < sample.Length; i++)
            {
                _values[i] = sample[i].Value;
                _normWeights[i] = Math.Abs(sumWeight) < 0.0001 ? 0.0d : sample[i].Weight / sumWeight;

                if (i > 0)
                {
                    _quantiles[i] = _quantiles[i - 1] + _normWeights[i - 1];
                }
            }

            MinUserValue = sample.Select(s => s.UserValue).FirstOrDefault();
            MaxUserValue = sample.Select(s => s.UserValue).LastOrDefault();
        }

        /// <inheritdoc />
        public long Count { get; }

        public double Sum { get; }

        /// <inheritdoc />
        public long Max => _values.LastOrDefault();

        /// <inheritdoc />
        public string MaxUserValue { get; }

        /// <inheritdoc />
        public double Mean
        {
            get
            {
                if (_values.Length == 0)
                {
                    return 0.0;
                }

                double sum = 0;
                for (var i = 0; i < _values.Length; i++)
                {
                    sum += _values[i] * _normWeights[i];
                }

                return sum;
            }
        }

        /// <inheritdoc />
        public double Median => GetValue(0.5d);

        /// <inheritdoc />
        public long Min => _values.FirstOrDefault();

        /// <inheritdoc />
        public string MinUserValue { get; }

        /// <inheritdoc />
        public double Percentile75 => GetValue(0.75d);

        /// <inheritdoc />
        public double Percentile95 => GetValue(0.95d);

        /// <inheritdoc />
        public double Percentile98 => GetValue(0.98d);

        /// <inheritdoc />
        public double Percentile99 => GetValue(0.99d);

        /// <inheritdoc />
        public double Percentile999 => GetValue(0.999d);

        /// <inheritdoc />
        public int Size => _values.Length;

        /// <inheritdoc />
        public double StdDev
        {
            get
            {
                if (Size <= 1)
                {
                    return 0;
                }

                var mean = Mean;
                double variance = 0;

                for (var i = 0; i < _values.Length; i++)
                {
                    var diff = _values[i] - mean;
                    variance += _normWeights[i] * diff * diff;
                }

                return Math.Sqrt(variance);
            }
        }

        /// <inheritdoc />
        public IEnumerable<long> Values => _values;

        /// <inheritdoc />
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

            var posx = Array.BinarySearch(_quantiles, quantile);
            if (posx < 0)
            {
                posx = ~posx - 1;
            }

            if (posx < 1)
            {
                return _values[0];
            }

            return posx >= _values.Length ? _values[_values.Length - 1] : _values[posx];
        }

        private sealed class WeightedSampleComparer : IComparer<WeightedSample>
        {
            public static readonly IComparer<WeightedSample> Instance = new WeightedSampleComparer();

            public int Compare(WeightedSample x, WeightedSample y) { return Comparer<long>.Default.Compare(x.Value, y.Value); }
        }
    }
}