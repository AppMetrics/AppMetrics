// <copyright file="UniformSnapshot.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.ReservoirSampling.Uniform
{
    /// <summary>
    ///     Represents a statistical snapshot of a sample set when using
    ///     <see href="http://www.cs.umd.edu/~samir/498/vitter.pdf">Vitter's Algorithm R</see>.
    ///     This is the snapshot used a histogram with a <see cref="DefaultAlgorithmRReservoir">uniform reservoir </see>
    /// </summary>
    /// <seealso cref="IReservoirSnapshot" />
    public sealed class UniformSnapshot : IReservoirSnapshot
    {
        private readonly long[] _values;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UniformSnapshot" /> class.
        /// </summary>
        /// <param name="count">The count of all observed values.</param>
        /// <param name="sum">The sum of all observed values.</param>
        /// <param name="values">The values within the sample set.</param>
        /// <param name="valuesAreSorted">if set to <c>true</c> [values are already sorted].</param>
        /// <param name="minUserValue">The minimum user value.</param>
        /// <param name="maxUserValue">The maximum user value.</param>
        public UniformSnapshot(
            long count,
            double sum,
            IEnumerable<long> values,
            bool valuesAreSorted = false,
            string minUserValue = null,
            string maxUserValue = null)
        {
            Count = count;
            Sum = sum;
            _values = values.ToArray();

            if (!valuesAreSorted)
            {
                Array.Sort(_values);
            }

            MinUserValue = minUserValue;
            MaxUserValue = maxUserValue;
        }

        /// <inheritdoc />
        public long Count { get; }

        /// <inheritdoc />
        public long Max => _values.LastOrDefault();

        /// <inheritdoc />
        public string MaxUserValue { get; }

        /// <inheritdoc />
        public double Mean => Size == 0 ? 0.0 : _values.Average();

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

                var avg = _values.Average();
                var sum = _values.Sum(d => Math.Pow(d - avg, 2));

                return Math.Sqrt(sum / (_values.Length - 1));
            }
        }

        public double Sum { get; }

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

            var pos = quantile * (_values.Length + 1);
            var index = (int)pos;

            if (index < 1)
            {
                return _values[0];
            }

            if (index >= _values.Length)
            {
                return _values[_values.Length - 1];
            }

            double lower = _values[index - 1];
            double upper = _values[index];

            // ReSharper disable ArrangeRedundantParentheses
            return lower + ((pos - Math.Floor(pos)) * (upper - lower));
            // ReSharper restore ArrangeRedundantParentheses
        }
    }
}