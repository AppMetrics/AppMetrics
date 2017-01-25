// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
#pragma warning disable

    /**
 * <h3>An integer values High Dynamic Range (HDR) Histogram that is synchronized as a whole</h3>
 * <p>
 * A {@link SynchronizedHistogram} is a variant of {@link Histogram} that is
 * synchronized as a whole, such that queries, copying, and addition operations are atomic with relation to
 * modification on the {@link SynchronizedHistogram}, and such that external accessors (e.g. iterations on the
 * histogram data) that synchronize on the {@link SynchronizedHistogram} instance can safely assume that no
 * modifications to the histogram data occur within their synchronized block.
 * <p>
 * It is important to note that synchronization can result in blocking recoding calls. If non-blocking recoding
 * operations are required, consider using {@link ConcurrentHistogram}, {@link AtomicHistogram}, or (recommended)
 * {@link Recorder} or {@link org.HdrHistogram.SingleWriterRecorder} which were intended for concurrent operations.
 * <p>
 * See package description for {@link org.HdrHistogram} and {@link org.HdrHistogram.Histogram} for more details.
 */
    
    public class SynchronizedHistogram : HdrHistogram
    {
        private static readonly object SyncLock = new object();

        /**
     * Construct an auto-resizing SynchronizedHistogram with a lowest discernible value of 1 and an auto-adjusting
     * highestTrackableValue. Can auto-resize up to track values up to (Long.MAX_VALUE / 2).
     *
     * @param numberOfSignificantValueDigits Specifies the precision to use. This is the number of significant
     *                                       decimal digits to which the histogram will maintain value resolution
     *                                       and separation. Must be a non-negative integer between 0 and 5.
     */

        public SynchronizedHistogram(int numberOfSignificantValueDigits)
            : base(numberOfSignificantValueDigits) { }

        /**
     * Construct a SynchronizedHistogram given the Highest value to be tracked and a number of significant decimal digits. The
     * histogram will be constructed to implicitly track (distinguish from 0) values as low as 1.
     *
     * @param highestTrackableValue The highest value to be tracked by the histogram. Must be a positive
     *                              integer that is {@literal >=} 2.
     * @param numberOfSignificantValueDigits Specifies the precision to use. This is the number of significant
     *                                       decimal digits to which the histogram will maintain value resolution
     *                                       and separation. Must be a non-negative integer between 0 and 5.
     */

        public SynchronizedHistogram(long highestTrackableValue, int numberOfSignificantValueDigits)
            : base(highestTrackableValue, numberOfSignificantValueDigits) { }

        /**
     * Construct a SynchronizedHistogram given the Lowest and Highest values to be tracked and a number of significant
     * decimal digits. Providing a lowestDiscernibleValue is useful is situations where the units used
     * for the histogram's values are much smaller that the minimal accuracy required. E.g. when tracking
     * time values stated in nanosecond units, where the minimal accuracy required is a microsecond, the
     * proper value for lowestDiscernibleValue would be 1000.
     *
     * @param lowestDiscernibleValue The lowest value that can be tracked (distinguished from 0) by the histogram.
     *                               Must be a positive integer that is {@literal >=} 1. May be internally rounded
     *                               down to nearest power of 2.
     * @param highestTrackableValue The highest value to be tracked by the histogram. Must be a positive
     *                              integer that is {@literal >=} (2 * lowestDiscernibleValue).
     * @param numberOfSignificantValueDigits Specifies the precision to use. This is the number of significant
     *                                       decimal digits to which the histogram will maintain value resolution
     *                                       and separation. Must be a non-negative integer between 0 and 5.
     */

        public SynchronizedHistogram(long lowestDiscernibleValue, long highestTrackableValue, int numberOfSignificantValueDigits)
            : base(lowestDiscernibleValue, highestTrackableValue, numberOfSignificantValueDigits) { }

        /**
     * Construct a histogram with the same range settings as a given source histogram,
     * duplicating the source's start/end timestamps (but NOT it's contents)
     * @param source The source histogram to duplicate
     */

        public SynchronizedHistogram(AbstractHistogram source)
            : base(source) { }

        public override AbstractHistogram copy()
        {
            lock (SyncLock)
            {
                SynchronizedHistogram copy;

                lock (this)
                {
                    copy = new SynchronizedHistogram(this);
                }

                copy.add(this);

                return copy;
            }
        }

        public override AbstractHistogram copyCorrectedForCoordinatedOmission(long expectedIntervalBetweenValueSamples)
        {
            lock (SyncLock)
            {
                var toHistogram = new SynchronizedHistogram(this);
                toHistogram.addWhileCorrectingForCoordinatedOmission(this, expectedIntervalBetweenValueSamples);
                return toHistogram;
            }
        }

        public override long getTotalCount()
        {
            lock (SyncLock)
            {
                return TotalCount;
            }
        }

        // ReSharper disable InconsistentNaming
        public new void add(AbstractHistogram otherHistogram)

            // ReSharper restore InconsistentNaming
        {
            // Synchronize add(). Avoid deadlocks by synchronizing in order of construction identity count.
            if (Identity < otherHistogram.Identity)
            {
                lock (this)
                {
                    lock (otherHistogram)
                    {
                        base.add(otherHistogram);
                    }
                }
            }
            else
            {
                lock (otherHistogram)
                {
                    lock (this)
                    {
                        base.add(otherHistogram);
                    }
                }
            }
        }

        internal override long getCountAtIndex(int index)
        {
            lock (SyncLock)
            {
                return Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)];
            }
        }

        protected internal override int _getEstimatedFootprintInBytes()
        {
            lock (SyncLock)
            {
                return 512 + 8 * Counts.Length;
            }
        }

        protected internal override void clearCounts()
        {
            lock (SyncLock)
            {
                Array.Clear(Counts, 0, Counts.Length);
                TotalCount = 0;
            }
        }

        protected internal override void resize(long newHighestTrackableValue)
        {
            lock (SyncLock)
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
        }

        protected internal override void shiftValuesLeft(int numberOfBinaryOrdersOfMagnitude)
        {
            lock (SyncLock)
            {
                base.shiftValuesLeft(numberOfBinaryOrdersOfMagnitude);
            }
        }

        protected internal override void shiftValuesRight(int numberOfBinaryOrdersOfMagnitude)
        {
            lock (SyncLock)
            {
                base.shiftValuesRight(numberOfBinaryOrdersOfMagnitude);
            }
        }

        protected override void addToCountAtIndex(int index, long value)
        {
            lock (SyncLock)
            {
                Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)] += value;
            }
        }

        protected override void addToTotalCount(long value)
        {
            lock (SyncLock)
            {
                TotalCount += value;
            }
        }

        protected override long getCountAtNormalizedIndex(int index)
        {
            lock (SyncLock)
            {
                return Counts[index];
            }
        }

        protected override int getNormalizingIndexOffset()
        {
            lock (SyncLock)
            {
                return NormalizingIndexOffset;
            }
        }

        protected override void incrementCountAtIndex(int index)
        {
            lock (SyncLock)
            {
                Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)]++;
            }
        }

        protected override void incrementTotalCount()
        {
            lock (SyncLock)
            {
                TotalCount++;
            }
        }

        protected override void setCountAtIndex(int index, long value)
        {
            lock (SyncLock)
            {
                Counts[NormalizeIndex(index, NormalizingIndexOffset, countsArrayLength)] = value;
            }
        }

        protected override void setCountAtNormalizedIndex(int index, long value)
        {
            lock (SyncLock)
            {
                Counts[index] = value;
            }
        }

        protected override void setNormalizingIndexOffset(int normalizingIndexOffset)
        {
            lock (SyncLock)
            {
                NormalizingIndexOffset = normalizingIndexOffset;
            }
        }

        protected override void setTotalCount(long totalCount)
        {
            lock (SyncLock)
            {
                TotalCount = totalCount;
            }
        }

        protected override void shiftNormalizingIndexByOffset(int offsetToAdd, bool lowestHalfBucketPopulated)
        {
            lock (SyncLock)
            {
                nonConcurrentNormalizingIndexShift(offsetToAdd, lowestHalfBucketPopulated);
            }
        }

        protected override void updatedMaxValue(long maxValue)
        {
            lock (SyncLock)
            {
                if (maxValue > getMaxValue())
                {
                    base.updatedMaxValue(maxValue);
                }
            }
        }

        protected override void updateMinNonZeroValue(long minNonZeroValue)
        {
            lock (SyncLock)
            {
                if (minNonZeroValue < getMinNonZeroValue())
                {
                    base.updateMinNonZeroValue(minNonZeroValue);
                }
            }
        }
    }
#pragma warning restore
}