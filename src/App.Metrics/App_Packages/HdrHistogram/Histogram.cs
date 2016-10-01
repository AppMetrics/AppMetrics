// Written by Gil Tene of Azul Systems, and released to the public domain,
// as explained at http://creativecommons.org/publicdomain/zero/1.0/
// 
// Ported to .NET by Iulian Margarintescu under the same license and terms as the java version
// Java Version repo: https://github.com/HdrHistogram/HdrHistogram
// Latest ported version is available in the Java submodule in the root of the repo

using System;

namespace App.Metrics.App_Packages.HdrHistogram
{
    /// <summary>
    /// <h3>A High Dynamic Range (HDR) Histogram</h3>
    /// <para>
    /// {@link Histogram} supports the recording and analyzing sampled data value counts across a configurable integer value
    /// range with configurable value precision within the range. Value precision is expressed as the number of significant
    /// digits in the value recording, and provides control over value quantization behavior across the value range and the
    /// subsequent value resolution at any given level.
    /// </para>
    /// <para>
    /// For example, a Histogram could be configured to track the counts of observed integer values between 0 and
    /// 3,600,000,000 while maintaining a value precision of 3 significant digits across that range. Value quantization
    /// within the range will thus be no larger than 1/1,000th (or 0.1%) of any value. This example Histogram could
    /// be used to track and analyze the counts of observed response times ranging between 1 microsecond and 1 hour
    /// in magnitude, while maintaining a value resolution of 1 microsecond up to 1 millisecond, a resolution of
    /// 1 millisecond (or better) up to one second, and a resolution of 1 second (or better) up to 1,000 seconds. At its
    /// maximum tracked value (1 hour), it would still maintain a resolution of 3.6 seconds (or better).
    /// </para>
    /// <para>
    /// Histogram tracks value counts in <b><code>long</code></b> fields. Smaller field types are available in the
    /// {@link IntCountsHistogram} and {@link ShortCountsHistogram} implementations of
    /// {@link org.HdrHistogram.AbstractHistogram}.
    /// </para>
    /// <para>
    /// Auto-resizing: When constructed with no specified value range (or when auto-resize is turned on with {@link
    /// Histogram#setAutoResize}) a {@link Histogram} will auto-resize its dynamic range to include recorded values as
    /// they are encountered. Note that recording calls that cause auto-resizing may take longer to execute, as resizing
    /// incurs allocation and copying of internal data structures.
    /// </para>
    /// <para>
    /// See package description for {@link org.HdrHistogram} for details.
    /// </para>
    /// </summary>
    internal class Histogram : AbstractHistogram
    {
        protected long totalCount;
        protected long[] counts;
        protected int normalizingIndexOffset;

        /// <summary>
        /// Construct an auto-resizing histogram with a lowest discernible value of 1 and an auto-adjusting
        /// highestTrackableValue. Can auto-resize up to track values up to (long.MaxValue / 2).
        /// </summary>
        /// <param name="numberOfSignificantValueDigits">Specifies the precision to use. This is the number of significant decimal digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between 0 and 5.</param>
        public Histogram(int numberOfSignificantValueDigits)
            : this(1, 2, numberOfSignificantValueDigits, sizeof(long), allocateCountsArray: true, autoResize: true)
        { }

        /// <summary>
        /// Construct a Histogram given the Highest value to be tracked and a number of significant decimal digits. The
        /// histogram will be constructed to implicitly track (distinguish from 0) values as low as 1.
        /// </summary>
        /// <param name="highestTrackableValue">The highest value to be tracked by the histogram. Must be a positiveinteger that is {@literal >=} 2.</param>
        /// <param name="numberOfSignificantValueDigits">Specifies the precision to use. This is the number of significant decimal digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between 0 and 5.</param>
        public Histogram(long highestTrackableValue, int numberOfSignificantValueDigits)
            : this(1, highestTrackableValue, numberOfSignificantValueDigits)
        { }

        /// <summary>
        /// Construct a Histogram given the Lowest and Highest values to be tracked and a number of significant
        /// decimal digits. Providing a lowestDiscernibleValue is useful is situations where the units used
        /// for the histogram's values are much smaller that the minimal accuracy required. E.g. when tracking
        /// time values stated in nanosecond units, where the minimal accuracy required is a microsecond, the
        /// proper value for lowestDiscernibleValue would be 1000.
        /// </summary>
        /// <param name="lowestDiscernibleValue">The lowest value that can be discerned (distinguished from 0) by the histogram. Must be a positive integer that is {@literal >=} 1. May be internally rounded down to nearest power of 2.</param>
        /// <param name="highestTrackableValue">The highest value to be tracked by the histogram. Must be a positive integer that is {@literal >=} (2 * lowestDiscernibleValue).</param>
        /// <param name="numberOfSignificantValueDigits">Specifies the precision to use. This is the number of significant decimal digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between 0 and 5.</param>
        public Histogram(long lowestDiscernibleValue, long highestTrackableValue, int numberOfSignificantValueDigits)
            : this(lowestDiscernibleValue, highestTrackableValue, numberOfSignificantValueDigits, sizeof(long), allocateCountsArray: true, autoResize: false)
        { }

        /// <summary> 
        /// Construct a histogram with the same range settings as a given source histogram,
        /// duplicating the source's start/end timestamps (but NOT its contents).
        /// </summary>
        /// <param name="source">The source histogram to duplicate/</param>
        public Histogram(AbstractHistogram source)
            : this(source, true)
        {
        }

        protected Histogram(AbstractHistogram source, bool allocateCountsArray)
            : base(source)
        {
            if (allocateCountsArray)
            {
                counts = new long[countsArrayLength];
            }
        }

        protected Histogram(long lowestDiscernibleValue, long highestTrackableValue, int numberOfSignificantValueDigits, int wordSizeInBytes, bool allocateCountsArray, bool autoResize)
            : base(lowestDiscernibleValue, highestTrackableValue, numberOfSignificantValueDigits, wordSizeInBytes, autoResize)
        {
            if (allocateCountsArray)
            {
                counts = new long[countsArrayLength];
            }
        }

        internal override long getCountAtIndex(int index)
        {
            return counts[NormalizeIndex(index, normalizingIndexOffset, countsArrayLength)];
        }

        protected override long getCountAtNormalizedIndex(int index)
        {
            return counts[index];
        }

        protected override void incrementCountAtIndex(int index)
        {
            counts[NormalizeIndex(index, normalizingIndexOffset, countsArrayLength)]++;
        }

        protected override void addToCountAtIndex(int index, long value)
        {
            counts[NormalizeIndex(index, normalizingIndexOffset, countsArrayLength)] += value;
        }

        protected override void setCountAtIndex(int index, long value)
        {
            counts[NormalizeIndex(index, normalizingIndexOffset, countsArrayLength)] = value;
        }

        protected override void setCountAtNormalizedIndex(int index, long value)
        {
            counts[index] = value;
        }

        protected override int getNormalizingIndexOffset()
        {
            return normalizingIndexOffset;
        }

        protected override void setNormalizingIndexOffset(int normalizingIndexOffset)
        {
            this.normalizingIndexOffset = normalizingIndexOffset;
        }

        protected override void shiftNormalizingIndexByOffset(int offsetToAdd, bool lowestHalfBucketPopulated)
        {
            nonConcurrentNormalizingIndexShift(offsetToAdd, lowestHalfBucketPopulated);
        }

        protected internal override void clearCounts()
        {
            Array.Clear(counts, 0, counts.Length);
            totalCount = 0;
        }

        public override AbstractHistogram copy()
        {
            Histogram copy = new Histogram(this);
            copy.add(this);
            return copy;
        }

        public override AbstractHistogram copyCorrectedForCoordinatedOmission(long expectedIntervalBetweenValueSamples)
        {
            Histogram copy = new Histogram(this);
            copy.addWhileCorrectingForCoordinatedOmission(this, expectedIntervalBetweenValueSamples);
            return copy;
        }

        public override long getTotalCount()
        {
            return totalCount;
        }

        protected override void setTotalCount(long totalCount)
        {
            this.totalCount = totalCount;
        }

        protected override void incrementTotalCount()
        {
            totalCount++;
        }

        protected override void addToTotalCount(long value)
        {
            totalCount += value;
        }

        protected internal override int _getEstimatedFootprintInBytes()
        {
            return (512 + (8 * counts.Length));
        }

        protected internal override void resize(long newHighestTrackableValue)
        {
            int oldNormalizedZeroIndex = NormalizeIndex(0, normalizingIndexOffset, countsArrayLength);

            establishSize(newHighestTrackableValue);

            int countsDelta = countsArrayLength - counts.Length;

            Array.Resize(ref counts, countsArrayLength);

            if (oldNormalizedZeroIndex != 0)
            {
                // We need to shift the stuff from the zero index and up to the end of the array:
                int newNormalizedZeroIndex = oldNormalizedZeroIndex + countsDelta;
                int lengthToCopy = (countsArrayLength - countsDelta) - oldNormalizedZeroIndex;
                Array.Copy(counts, oldNormalizedZeroIndex, counts, newNormalizedZeroIndex, lengthToCopy);
            }
        }
    }
}
