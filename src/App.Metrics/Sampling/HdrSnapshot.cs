// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Sampling.HdrHistogram;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Sampling
{
    internal sealed class HdrSnapshot : ISnapshot
    {
        private readonly AbstractHistogram _histogram;

        /// <summary>
        /// Initializes a new instance of the <see cref="HdrSnapshot"/> class.
        /// </summary>
        /// <param name="histogram">The histogram.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="minUserValue">The minimum user value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="maxUserValue">The maximum user value.</param>
        public HdrSnapshot(AbstractHistogram histogram, long minValue, string minUserValue, long maxValue, string maxUserValue)
        {
            _histogram = histogram;
            Min = minValue;
            MinUserValue = minUserValue;
            Max = maxValue;
            MaxUserValue = maxUserValue;
        }

        /// <inheritdoc cref="ISnapshot"/>
        public long Count => _histogram.getTotalCount();

        /// <inheritdoc cref="ISnapshot"/>
        public long Max { get; }

        /// <inheritdoc cref="ISnapshot"/>
        public string MaxUserValue { get; }

        /// <inheritdoc cref="ISnapshot"/>
        public double Mean => _histogram.getMean();

        /// <inheritdoc cref="ISnapshot"/>
        public double Median => _histogram.getValueAtPercentile(50);

        /// <inheritdoc cref="ISnapshot"/>
        public long Min { get; }

        /// <inheritdoc cref="ISnapshot"/>
        public string MinUserValue { get; }

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile75 => _histogram.getValueAtPercentile(75);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile95 => _histogram.getValueAtPercentile(95);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile98 => _histogram.getValueAtPercentile(98);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile99 => _histogram.getValueAtPercentile(99);

        /// <inheritdoc cref="ISnapshot"/>
        public double Percentile999 => _histogram.getValueAtPercentile(99.9);

        /// <inheritdoc cref="ISnapshot"/>
        public int Size => _histogram.getEstimatedFootprintInBytes();

        /// <inheritdoc cref="ISnapshot"/>
        public double StdDev => _histogram.getStdDeviation();

        /// <inheritdoc cref="ISnapshot"/>
        public IEnumerable<long> Values
        {
            get { return _histogram.RecordedValues().Select(v => v.GetValueIteratedTo()); }
        }

        /// <inheritdoc cref="ISnapshot"/>
        public double GetValue(double quantile)
        {
            return _histogram.getValueAtPercentile(quantile * 100);
        }
    }
}