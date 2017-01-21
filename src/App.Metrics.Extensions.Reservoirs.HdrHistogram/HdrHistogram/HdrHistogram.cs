// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
#pragma warning disable

    /// <summary>
    ///     <h3>A High Dynamic Range (HDR) Histogram</h3>
    ///     <para>
    ///         {@link Histogram} supports the recording and analyzing sampled data value counts across a configurable integer
    ///         value
    ///         range with configurable value precision within the range. Value precision is expressed as the number of
    ///         significant
    ///         digits in the value recording, and provides control over value quantization behavior across the value range and
    ///         the
    ///         subsequent value resolution at any given level.
    ///     </para>
    ///     <para>
    ///         For example, a Histogram could be configured to track the counts of observed integer values between 0 and
    ///         3,600,000,000 while maintaining a value precision of 3 significant digits across that range. Value quantization
    ///         within the range will thus be no larger than 1/1,000th (or 0.1%) of any value. This example Histogram could
    ///         be used to track and analyze the counts of observed response times ranging between 1 microsecond and 1 hour
    ///         in magnitude, while maintaining a value resolution of 1 microsecond up to 1 millisecond, a resolution of
    ///         1 millisecond (or better) up to one second, and a resolution of 1 second (or better) up to 1,000 seconds. At
    ///         its
    ///         maximum tracked value (1 hour), it would still maintain a resolution of 3.6 seconds (or better).
    ///     </para>
    ///     <para>
    ///         Histogram tracks value counts in
    ///         <b>
    ///             <code>long</code>
    ///         </b>
    ///         fields. Smaller field types are available in the
    ///         {@link IntCountsHistogram} and {@link ShortCountsHistogram} implementations of
    ///         {@link org.HdrHistogram.AbstractHistogram}.
    ///     </para>
    ///     <para>
    ///         Auto-resizing: When constructed with no specified value range (or when auto-resize is turned on with {@link
    ///         Histogram#setAutoResize}) a {@link Histogram} will auto-resize its dynamic range to include recorded values as
    ///         they are encountered. Note that recording calls that cause auto-resizing may take longer to execute, as
    ///         resizing
    ///         incurs allocation and copying of internal data structures.
    ///     </para>
    ///     <para>
    ///         See package description for {@link org.HdrHistogram} for details.
    ///     </para>
    /// </summary>
    public class HdrHistogram : AbstractHistogram
    {
        protected long[] Counts;
        protected int NormalizingIndexOffset;
        protected long TotalCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HdrHistogram" /> class.
        /// </summary>
        /// <param name="numberOfSignificantValueDigits">
        ///     Specifies the precision to use. This is the number of significant decimal
        ///     digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between
        ///     0 and 5.
        /// </param>
        /// <remarks>
        ///     Construct an auto-resizing histogram with a lowest discernible value of 1 and an auto-adjusting
        ///     highestTrackableValue. Can auto-resize up to track values up to (long.MaxValue / 2).
        /// </remarks>
        public HdrHistogram(int numberOfSignificantValueDigits)
            : this(1, 2, numberOfSignificantValueDigits, sizeof(long), allocateCountsArray: true, autoResize: true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HdrHistogram" /> class.
        /// </summary>
        /// <param name="highestTrackableValue">
        ///     The highest value to be tracked by the histogram. Must be a positiveinteger that is
        ///     {@literal &gt;=} 2.
        /// </param>
        /// <param name="numberOfSignificantValueDigits">
        ///     Specifies the precision to use. This is the number of significant decimal
        ///     digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between
        ///     0 and 5.
        /// </param>
        /// <remarks>
        ///     Construct a Histogram given the Highest value to be tracked and a number of significant decimal digits. The
        ///     histogram will be constructed to implicitly track (distinguish from 0) values as low as 1.
        /// </remarks>
        public HdrHistogram(long highestTrackableValue, int numberOfSignificantValueDigits)
            : this(1, highestTrackableValue, numberOfSignificantValueDigits)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HdrHistogram" /> class.
        /// </summary>
        /// <param name="lowestDiscernibleValue">
        ///     The lowest value that can be discerned (distinguished from 0) by the histogram.
        ///     Must be a positive integer that is {@literal &gt;=} 1. May be internally rounded down to nearest power of 2.
        /// </param>
        /// <param name="highestTrackableValue">
        ///     The highest value to be tracked by the histogram. Must be a positive integer that
        ///     is {@literal &gt;=} (2 * lowestDiscernibleValue).
        /// </param>
        /// <param name="numberOfSignificantValueDigits">
        ///     Specifies the precision to use. This is the number of significant decimal
        ///     digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between
        ///     0 and 5.
        /// </param>
        /// <remarks>
        ///     Construct a Histogram given the Lowest and Highest values to be tracked and a number of significant
        ///     decimal digits. Providing a lowestDiscernibleValue is useful is situations where the units used
        ///     for the histogram's values are much smaller that the minimal accuracy required. E.g. when tracking
        ///     time values stated in nanosecond units, where the minimal accuracy required is a microsecond, the
        ///     proper value for lowestDiscernibleValue would be 1000.
        /// </remarks>
        public HdrHistogram(long lowestDiscernibleValue, long highestTrackableValue, int numberOfSignificantValueDigits)
            : this(
                lowestDiscernibleValue,
                highestTrackableValue,
                numberOfSignificantValueDigits,
                sizeof(long),
                allocateCountsArray: true,
                autoResize: false)
        {
        }

        /// <summary>
        ///     Construct a histogram with the same range settings as a given source histogram,
        ///     duplicating the source's start/end timestamps (but NOT its contents).
        /// </summary>
        /// <param name="source">The source histogram to duplicate/</param>
        public HdrHistogram(AbstractHistogram source)
            : this(source, true)
        {
        }

        protected HdrHistogram(AbstractHistogram source, bool allocateCountsArray)
            : base(source)
        {
            if (allocateCountsArray)
            {
                Counts = new long[countsArrayLength];
            }
        }

        protected HdrHistogram(
            long lowestDiscernibleValue,
            long highestTrackableValue,
            int numberOfSignificantValueDigits,
            int wordSizeInBytes,
            bool allocateCountsArray,
            bool autoResize)
            : base(lowestDiscernibleValue, highestTrackableValue, numberOfSignificantValueDigits, wordSizeInBytes, autoResize)
        {
            if (allocateCountsArray)
            {
                Counts = new long[countsArrayLength];
            }
        }

        public override AbstractHistogram copy()
        {
            var copy = new HdrHistogram(this);
            copy.add(this);
            return copy;
        }

        public override AbstractHistogram copyCorrectedForCoordinatedOmission(long expectedIntervalBetweenValueSamples)
        {
            var copy = new HdrHistogram(this);
            copy.addWhileCorrectingForCoordinatedOmission(this, expectedIntervalBetweenValueSamples);
            return copy;
        }

        public override long getTotalCount() { return TotalCount; }

        internal override long getCountAtIndex(int index) { return Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)]; }

        protected internal override int _getEstimatedFootprintInBytes() { return 512 + 8 * Counts.Length; }

        protected internal override void clearCounts()
        {
            Array.Clear(Counts, 0, Counts.Length);
            TotalCount = 0;
        }

        protected internal override void resize(long newHighestTrackableValue)
        {
            var oldNormalizedZeroIndex = NormalizeIndex(0, NormalizingIndexOffset, countsArrayLength);

            establishSize(newHighestTrackableValue);

            var countsDelta = countsArrayLength - Counts.Length;

            Array.Resize(ref Counts, countsArrayLength);

            if (oldNormalizedZeroIndex != 0)
            {
                // We need to shift the stuff from the zero index and up to the end of the array:
                var newNormalizedZeroIndex = oldNormalizedZeroIndex + countsDelta;
                var lengthToCopy = countsArrayLength - countsDelta - oldNormalizedZeroIndex;
                Array.Copy(Counts, oldNormalizedZeroIndex, Counts, newNormalizedZeroIndex, lengthToCopy);
            }
        }

        protected override void addToCountAtIndex(int index, long value)
        {
            Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)] += value;
        }

        protected override void addToTotalCount(long value) { TotalCount += value; }

        protected override long getCountAtNormalizedIndex(int index) { return Counts[index]; }

        protected override int getNormalizingIndexOffset() { return NormalizingIndexOffset; }

        protected override void incrementCountAtIndex(int index) { Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)]++; }

        protected override void incrementTotalCount() { TotalCount++; }

        protected override void setCountAtIndex(int index, long value)
        {
            Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)] = value;
        }

        protected override void setCountAtNormalizedIndex(int index, long value) { Counts[index] = value; }

        protected override void setNormalizingIndexOffset(int normalizingIndexOffset) { NormalizingIndexOffset = normalizingIndexOffset; }

        protected override void setTotalCount(long totalCount) { TotalCount = totalCount; }

        protected override void shiftNormalizingIndexByOffset(int offsetToAdd, bool lowestHalfBucketPopulated)
        {
            nonConcurrentNormalizingIndexShift(offsetToAdd, lowestHalfBucketPopulated);
        }
    }
#pragma warning restore
}