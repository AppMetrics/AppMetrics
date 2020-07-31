// <copyright file="DefaultHistogramMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using App.Metrics.Concurrency;

namespace App.Metrics.BucketHistogram
{
    public sealed class DefaultBucketHistogramMetric : IBucketHistogramMetric
    {
        private bool _disposed;
        private readonly StripedLongAdder _counter = new StripedLongAdder();
        private readonly StripedLongAdder _sum = new StripedLongAdder();
        private readonly SortedDictionary<double, StripedLongAdder> _buckets = new SortedDictionary<double, StripedLongAdder>(new DoubleReverseCompare());

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultBucketHistogramMetric" /> class.
        /// </summary>
        public DefaultBucketHistogramMetric(IEnumerable<double> buckets)
        {
            if (buckets != null)
            {
                foreach (var bucket in buckets)
                {
                    _buckets.Add(bucket, new StripedLongAdder());
                }
            }

            _buckets.Add(double.PositiveInfinity, new StripedLongAdder());
        }

        public BucketHistogramValue Value => GetValue();

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
        }

        [ExcludeFromCodeCoverage]
        // ReSharper disable MemberCanBePrivate.Global
        public void Dispose(bool disposing)
        // ReSharper restore MemberCanBePrivate.Global
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public BucketHistogramValue GetValue(bool resetMetric = false)
        {
            var value = new BucketHistogramValue(_counter.GetValue(), _sum.GetValue(), _buckets.ToDictionary(x => x.Key, x => Convert.ToDouble(x.Value.GetValue())));

            if (resetMetric)
            {
                Reset();
            }

            return value;
        }

        /// <inheritdoc />
        public void Reset()
        {
            foreach (var bucket in _buckets)
            {
                bucket.Value.Reset();
            }
            _sum.Reset();
            _counter.Reset();
        }

        public void Update(long value, string userValue)
        {
            Update(value);
        }

        /// <inheritdoc />
        public void Update(long value)
        {
            StripedLongAdder bucketCounter = null;
            foreach (var kvp in _buckets)
            {
                if (kvp.Key < value)
                    break;

                bucketCounter = kvp.Value;
            }


            bucketCounter.Increment();
            _sum.Add(value);
            _counter.Increment();
        }

        private class DoubleReverseCompare : IComparer<double>
        {
            public int Compare(double x, double y)
            {
                if (x < y)
                    return 1;

                if (x > y)
                    return -1;

                return 0;
            }
        }
    }
}