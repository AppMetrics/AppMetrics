// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Sampling
{
    /// <summary>
    ///     Represents a statistical snapshot of a sample set when using
    ///     <see href="http://www.cs.umd.edu/~samir/498/vitter.pdf">Vitter's Algorithm R</see>.
    ///     This is the snapshot used a histogram with a <see cref="UniformReservoir">uniform reservoir </see>
    /// </summary>
    /// <seealso cref="App.Metrics.Sampling.Interfaces.ISnapshot" />
    public sealed class UniformSnapshot : ISnapshot
    {
        private readonly long[] _values;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UniformSnapshot" /> class.
        /// </summary>
        /// <param name="count">The count of the sample set.</param>
        /// <param name="values">The values within the sample set.</param>
        /// <param name="valuesAreSorted">if set to <c>true</c> [values are already sorted].</param>
        /// <param name="minUserValue">The minimum user value.</param>
        /// <param name="maxUserValue">The maximum user value.</param>
        public UniformSnapshot(long count, IEnumerable<long> values, bool valuesAreSorted = false, string minUserValue = null,
            string maxUserValue = null)
        {
            Count = count;
            _values = values.ToArray();

            if (!valuesAreSorted)
            {
                Array.Sort(_values);
            }

            MinUserValue = minUserValue;
            MaxUserValue = maxUserValue;
        }

        /// <inheritdoc cref="ISnapshot"/>
        public long Count { get; }

        /// <inheritdoc cref="ISnapshot"/>
        public long Max => _values.LastOrDefault();

        /// <inheritdoc cref="ISnapshot"/>
        public string MaxUserValue { get; }

        /// <inheritdoc cref="ISnapshot"/>
        public double Mean => Size == 0 ? 0.0 : _values.Average();

        /// <inheritdoc cref="ISnapshot"/>
        public double Median => GetValue(0.5d);

        /// <inheritdoc cref="ISnapshot"/>
        public long Min => _values.FirstOrDefault();

        /// <inheritdoc cref="ISnapshot"/>
        public string MinUserValue { get; }

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile75 => GetValue(0.75d);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile95 => GetValue(0.95d);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile98 => GetValue(0.98d);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile99 => GetValue(0.99d);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile999 => GetValue(0.999d);

        /// <inheritdoc cref="ISnapshot"/>
        public int Size => _values.Length;

        /// <inheritdoc cref="ISnapshot"/>
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

                return Math.Sqrt((sum) / (_values.Length - 1));
            }
        }

        /// <inheritdoc cref="ISnapshot"/>
        public IEnumerable<long> Values => _values;

        /// <inheritdoc cref="ISnapshot"/>
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

            return lower + (pos - Math.Floor(pos)) * (upper - lower);
        }
    }
}