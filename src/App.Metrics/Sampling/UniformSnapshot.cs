// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Sampling
{
    public sealed class UniformSnapshot : ISnapshot
    {
        private readonly long[] _values;

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

        public long Count { get; }

        public long Max => _values.LastOrDefault();

        public string MaxUserValue { get; }

        public double Mean => Size == 0 ? 0.0 : _values.Average();

        public double Median => GetValue(0.5d);

        public long Min => _values.FirstOrDefault();

        public string MinUserValue { get; }

        public double Percentile75 => GetValue(0.75d);

        public double Percentile95 => GetValue(0.95d);

        public double Percentile98 => GetValue(0.98d);

        public double Percentile99 => GetValue(0.99d);

        public double Percentile999 => GetValue(0.999d);

        public int Size => _values.Length;

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

        public IEnumerable<long> Values => _values;

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