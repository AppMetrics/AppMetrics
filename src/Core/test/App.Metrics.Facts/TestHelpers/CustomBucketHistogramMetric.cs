// <copyright file="CustomHistogramMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.BucketHistogram;

namespace App.Metrics.Facts.TestHelpers
{
    public class CustomBucketHistogramMetric : IBucketHistogramMetric
    {
        private bool _disposed;
        private readonly List<long> _values = new List<long>();

        public long Count => _values.Count;

        public int Size => _values.Count;

        public IEnumerable<long> Values => _values;


        public BucketHistogramValue Value => new BucketHistogramValue(Count, 0, new Dictionary<double, double>());

        public void Dispose()
        {
            if (!_disposed)
            {
            }

            _disposed = true;
        }

        public BucketHistogramValue GetValue(bool resetMetric = false) { return Value; }

        public void Reset() { _values.Clear(); }

        public void Update(long value, string userValue) { _values.Add(value); }

        public void Update(long value) { _values.Add(value); }
    }
}