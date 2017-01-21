// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram
{
    internal sealed class HdrSnapshot : IReservoirSnapshot
    {
        private readonly AbstractHistogram _histogram;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HdrSnapshot" /> class.
        /// </summary>
        /// <param name="histogram">The histogram.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="minUserValue">The minimum user value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="maxUserValue">The maximum user value.</param>
        public HdrSnapshot(AbstractHistogram histogram, long minValue, string minUserValue, long maxValue, string maxUserValue)
        {
            _histogram = histogram;
            Min = !string.IsNullOrWhiteSpace(minUserValue) ? minValue : _histogram.getMinValue();
            MinUserValue = minUserValue;
            Max = !string.IsNullOrWhiteSpace(maxUserValue) ? maxValue : _histogram.getMaxValue();
            MaxUserValue = maxUserValue;
        }

        /// <inheritdoc cref="IReservoirSnapshot" />
        public long Count => _histogram.getTotalCount();

        /// <inheritdoc cref="IReservoirSnapshot" />
        public long Max { get; }

        /// <inheritdoc cref="IReservoirSnapshot" />
        public string MaxUserValue { get; }

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double Mean => _histogram.getMean();

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double Median => _histogram.getValueAtPercentile(50);

        /// <inheritdoc cref="IReservoirSnapshot" />
        public long Min { get; }

        /// <inheritdoc cref="IReservoirSnapshot" />
        public string MinUserValue { get; }

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double Percentile75 => _histogram.getValueAtPercentile(75);

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double Percentile95 => _histogram.getValueAtPercentile(95);

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double Percentile98 => _histogram.getValueAtPercentile(98);

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double Percentile99 => _histogram.getValueAtPercentile(99);

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double Percentile999 => _histogram.getValueAtPercentile(99.9);

        /// <inheritdoc cref="IReservoirSnapshot" />
        public int Size => _histogram.getEstimatedFootprintInBytes();

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double StdDev => _histogram.getStdDeviation();

        /// <inheritdoc cref="IReservoirSnapshot" />
        public IEnumerable<long> Values
        {
            get { return _histogram.RecordedValues().Select(v => v.GetValueIteratedTo()); }
        }

        /// <inheritdoc cref="IReservoirSnapshot" />
        public double GetValue(double quantile) { return _histogram.getValueAtPercentile(quantile * 100); }
    }
}