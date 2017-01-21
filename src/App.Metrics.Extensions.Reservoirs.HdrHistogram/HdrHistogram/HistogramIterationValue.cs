// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
    // Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
    // Ported/Refactored to .NET Standard Library by Allan Hardy
#pragma warning disable

    /**
                     * Represents a value point iterated through in a Histogram, with associated stats.
                     * <ul>
                     * <li><b><code>valueIteratedTo</code></b> :<br> The actual value level that was iterated to by the iterator</li>
                     * <li><b><code>prevValueIteratedTo</code></b> :<br> The actual value level that was iterated from by the iterator</li>
                     * <li><b><code>countAtValueIteratedTo</code></b> :<br> The count of recorded values in the histogram that
                     * exactly match this [lowestEquivalentValue(valueIteratedTo)...highestEquivalentValue(valueIteratedTo)] value
                     * range.</li>
                     * <li><b><code>countAddedInThisIterationStep</code></b> :<br> The count of recorded values in the histogram that
                     * were added to the totalCountToThisValue (below) as a result on this iteration step. Since multiple iteration
                     * steps may occur with overlapping equivalent value ranges, the count may be lower than the count found at
                     * the value (e.g. multiple linear steps or percentile levels can occur within a single equivalent value range)</li>
                     * <li><b><code>totalCountToThisValue</code></b> :<br> The total count of all recorded values in the histogram at
                     * values equal or smaller than valueIteratedTo.</li>
                     * <li><b><code>totalValueToThisValue</code></b> :<br> The sum of all recorded values in the histogram at values
                     * equal or smaller than valueIteratedTo.</li>
                     * <li><b><code>percentile</code></b> :<br> The percentile of recorded values in the histogram at values equal
                     * or smaller than valueIteratedTo.</li>
                     * <li><b><code>percentileLevelIteratedTo</code></b> :<br> The percentile level that the iterator returning this
                     * HistogramIterationValue had iterated to. Generally, percentileLevelIteratedTo will be equal to or smaller than
                     * percentile, but the same value point can contain multiple iteration levels for some iterators. E.g. a
                     * PercentileIterator can stop multiple times in the exact same value point (if the count at that value covers a
                     * range of multiple percentiles in the requested percentile iteration points).</li>
                     * </ul>
                     */

    public class HistogramIterationValue
    {
        private long _countAddedInThisIterationStep;
        private long _countAtValueIteratedTo;
        private double _integerToDoubleValueConversionRatio;
        private double _percentile;
        private double _percentileLevelIteratedTo;
        private long _totalCountToThisValue;
        private long _totalValueToThisValue;
        private long _valueIteratedFrom;
        private long _valueIteratedTo;

        public long GetCountAddedInThisIterationStep() { return _countAddedInThisIterationStep; }

        public long GetCountAtValueIteratedTo() { return _countAtValueIteratedTo; }

        public double GetIntegerToDoubleValueConversionRatio() { return _integerToDoubleValueConversionRatio; }

        public double GetPercentile() { return _percentile; }

        public double GetPercentileLevelIteratedTo() { return _percentileLevelIteratedTo; }

        public long GetTotalCountToThisValue() { return _totalCountToThisValue; }

        public long GetTotalValueToThisValue() { return _totalValueToThisValue; }

        public long GetValueIteratedFrom() { return _valueIteratedFrom; }

        public long GetValueIteratedTo() { return _valueIteratedTo; }

        internal void Reset()
        {
            _valueIteratedTo = 0;
            _valueIteratedFrom = 0;
            _countAtValueIteratedTo = 0;
            _countAddedInThisIterationStep = 0;
            _totalCountToThisValue = 0;
            _totalValueToThisValue = 0;
            _percentile = 0.0;
            _percentileLevelIteratedTo = 0.0;
        }

        // Set is all-or-nothing to avoid the potential for accidental omission of some values...
        internal void Set(
            long valueIteratedTo,
            long valueIteratedFrom,
            long countAtValueIteratedTo,
            long countInThisIterationStep,
            long totalCountToThisValue,
            long totalValueToThisValue,
            double percentile,
            double percentileLevelIteratedTo,
            double integerToDoubleValueConversionRatio)
        {
            _valueIteratedTo = valueIteratedTo;
            _valueIteratedFrom = valueIteratedFrom;
            _countAtValueIteratedTo = countAtValueIteratedTo;
            _countAddedInThisIterationStep = countInThisIterationStep;
            _totalCountToThisValue = totalCountToThisValue;
            _totalValueToThisValue = totalValueToThisValue;
            _percentile = percentile;
            _percentileLevelIteratedTo = percentileLevelIteratedTo;
            _integerToDoubleValueConversionRatio = integerToDoubleValueConversionRatio;
        }
    }
#pragma warning restore
}