// Written by Gil Tene of Azul Systems, and released to the public domain,
// as explained at http://creativecommons.org/publicdomain/zero/1.0/
// 
// Ported to .NET by Iulian Margarintescu under the same license and terms as the java version
// Java Version repo: https://github.com/HdrHistogram/HdrHistogram
// Latest ported version is available in the Java submodule in the root of the repo

using System.Diagnostics;
using App.Metrics.App_Packages.Concurrency;

namespace App.Metrics.App_Packages.HdrHistogram
{
    /// <summary>
    /// <h3>An integer values High Dynamic Range (HDR) Histogram that supports safe concurrent recording operations.</h3>
    /// A ConcurrentHistogram guarantees lossless recording of values into the histogram even when the
    /// histogram is updated by multiple threads, and supports auto-resize and shift operations that may
    /// result from or occur concurrently with other recording operations.

    /// It is important to note that concurrent recording, auto-sizing, and value shifting are the only thread-safe
    /// behaviors provided by {@link ConcurrentHistogram}, and that it is not otherwise synchronized. Specifically, {@link
    /// ConcurrentHistogram} provides no implicit synchronization that would prevent the contents of the histogram
    /// from changing during queries, iterations, copies, or addition operations on the histogram. Callers wishing to make
    /// potentially concurrent, multi-threaded updates that would safely work in the presence of queries, copies, or
    /// additions of histogram objects should either take care to externally synchronize and/or order their access,
    /// use the {@link SynchronizedHistogram} variant, or (recommended) use {@link Recorder} or
    /// {@link SingleWriterRecorder} which are intended for this purpose.
    /// 
    /// Auto-resizing: When constructed with no specified value range range (or when auto-resize is turned on with {@link
    /// Histogram#setAutoResize}) a {@link Histogram} will auto-resize its dynamic range to include recorded values as
    /// they are encountered. Note that recording calls that cause auto-resizing may take longer to execute, as resizing
    /// incurs allocation and copying of internal data structures.
    /// 
    /// See package description for {@link org.HdrHistogram} for details.
    /// </summary>
    internal class ConcurrentHistogram : Histogram
    {
        private new readonly StripedLongAdder totalCount = new StripedLongAdder();

        private int activeCountsNormalizingIndexOffset;
        private AtomicLongArray activeCounts;

        private int inactiveCountsNormalizingIndexOffset;
        private AtomicLongArray inactiveCounts;

        private readonly WriterReaderPhaser wrp = new WriterReaderPhaser();

        internal override long getCountAtIndex(int index)
        {
            try
            {
                wrp.ReaderLock();
                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);

                long activeCount = activeCounts.GetValue(NormalizeIndex(index, activeCountsNormalizingIndexOffset, activeCounts.Length));
                long inactiveCount = inactiveCounts.GetValue(NormalizeIndex(index, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length));
                return activeCount + inactiveCount;
            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        protected override long getCountAtNormalizedIndex(int index)
        {
            try
            {
                wrp.ReaderLock();
                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);
                long activeCount = activeCounts.GetValue(index);
                long inactiveCount = inactiveCounts.GetValue(index);
                return activeCount + inactiveCount;
            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        protected override void incrementCountAtIndex(int index)
        {
            long criticalValue = wrp.WriterCriticalSectionEnter();
            try
            {
                activeCounts.Increment(NormalizeIndex(index, activeCountsNormalizingIndexOffset, activeCounts.Length));
            }
            finally
            {
                wrp.WriterCriticalSectionExit(criticalValue);
            }
        }

        protected override void addToCountAtIndex(int index, long value)
        {
            long criticalValue = wrp.WriterCriticalSectionEnter();
            try
            {
                activeCounts.Add(NormalizeIndex(index, activeCountsNormalizingIndexOffset, activeCounts.Length), value);
            }
            finally
            {
                wrp.WriterCriticalSectionExit(criticalValue);
            }
        }

        protected override void setCountAtIndex(int index, long value)
        {
            try
            {
                wrp.ReaderLock();
                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);
                activeCounts.SetValue(NormalizeIndex(index, activeCountsNormalizingIndexOffset, activeCounts.Length), value);
                inactiveCounts.SetValue(NormalizeIndex(index, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length), 0);
            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        protected override void setCountAtNormalizedIndex(int index, long value)
        {
            try
            {
                wrp.ReaderLock();
                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);
                inactiveCounts.SetValue(index, value);
                activeCounts.SetValue(index, 0);
            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        protected override int getNormalizingIndexOffset()
        {
            return activeCountsNormalizingIndexOffset;
        }

        protected override void setNormalizingIndexOffset(int normalizingIndexOffset)
        {
            setNormalizingIndexOffset(normalizingIndexOffset, 0, false);
        }

        private void setNormalizingIndexOffset(
            int normalizingIndexOffset,
            int shiftedAmount,
            bool lowestHalfBucketPopulated)
        {
            try
            {
                wrp.ReaderLock();

                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);

                if (normalizingIndexOffset == activeCountsNormalizingIndexOffset)
                {
                    return; // Nothing to do.
                }

                // Save and clear the inactive 0 value count:
                int zeroIndex = NormalizeIndex(0, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length);
                long inactiveZeroValueCount = inactiveCounts.GetValue(zeroIndex);
                inactiveCounts.SetValue(zeroIndex, 0);

                // Change the normalizingIndexOffset on the current inactiveCounts:
                inactiveCountsNormalizingIndexOffset = normalizingIndexOffset;
                //inactiveCounts.setNormalizingIndexOffset(normalizingIndexOffset);

                // Handle the inactive lowest half bucket:
                if ((shiftedAmount > 0) && lowestHalfBucketPopulated)
                {
                    shiftLowestInactiveHalfBucketContentsLeft(shiftedAmount);
                }

                // Restore the inactive 0 value count:
                zeroIndex = NormalizeIndex(0, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length);
                inactiveCounts.SetValue(zeroIndex, inactiveZeroValueCount);

                // switch active and inactive:
                var tmp = activeCounts;
                var tmpOffset = activeCountsNormalizingIndexOffset;

                activeCounts = inactiveCounts;
                activeCountsNormalizingIndexOffset = inactiveCountsNormalizingIndexOffset;

                inactiveCounts = tmp;
                inactiveCountsNormalizingIndexOffset = tmpOffset;

                wrp.FlipPhase();

                // Save and clear the newly inactive 0 value count:
                zeroIndex = NormalizeIndex(0, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length);
                inactiveZeroValueCount = inactiveCounts.GetValue(zeroIndex);
                inactiveCounts.SetValue(zeroIndex, 0);

                // Change the normalizingIndexOffset on the newly inactiveCounts:
                inactiveCountsNormalizingIndexOffset = normalizingIndexOffset;
                //inactiveCounts.setNormalizingIndexOffset(normalizingIndexOffset);

                // Handle the newly inactive lowest half bucket:
                if ((shiftedAmount > 0) && lowestHalfBucketPopulated)
                {
                    shiftLowestInactiveHalfBucketContentsLeft(shiftedAmount);
                }

                // Restore the newly inactive 0 value count:
                zeroIndex = NormalizeIndex(0, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length);
                inactiveCounts.SetValue(zeroIndex, inactiveZeroValueCount);

                // switch active and inactive again:
                tmp = activeCounts;
                tmpOffset = activeCountsNormalizingIndexOffset;

                activeCounts = inactiveCounts;
                activeCountsNormalizingIndexOffset = inactiveCountsNormalizingIndexOffset;

                inactiveCounts = tmp;
                inactiveCountsNormalizingIndexOffset = tmpOffset;

                wrp.FlipPhase();

                // At this point, both active and inactive have normalizingIndexOffset safely set,
                // and the switch in each was done without any writers using the wrong value in flight.
            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        private void shiftLowestInactiveHalfBucketContentsLeft(int shiftAmount)
        {
            int numberOfBinaryOrdersOfMagnitude = shiftAmount >> subBucketHalfCountMagnitude;

            // The lowest inactive half-bucket (not including the 0 value) is special: unlike all other half
            // buckets, the lowest half bucket values cannot be scaled by simply changing the
            // normalizing offset. Instead, they must be individually re-recorded at the new
            // scale, and cleared from the current one.
            //
            // We know that all half buckets "below" the current lowest one are full of 0s, because
            // we would have overflowed otherwise. So we need to shift the values in the current
            // lowest half bucket into that range (including the current lowest half bucket itself).
            // Iterating up from the lowermost non-zero "from slot" and copying values to the newly
            // scaled "to slot" (and then zeroing the "from slot"), will work in a single pass,
            // because the scale "to slot" index will always be a lower index than its or any
            // preceding non-scaled "from slot" index:
            //
            // (Note that we specifically avoid slot 0, as it is directly handled in the outer case)

            for (int fromIndex = 1; fromIndex < subBucketHalfCount; fromIndex++)
            {
                long toValue = ValueFromIndex(fromIndex) << numberOfBinaryOrdersOfMagnitude;
                int toIndex = CountsArrayIndex(toValue);
                int normalizedToIndex =
                    NormalizeIndex(toIndex, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length);
                long countAtFromIndex = inactiveCounts.GetValue(fromIndex);
                inactiveCounts.SetValue(normalizedToIndex, countAtFromIndex);
                inactiveCounts.SetValue(fromIndex, 0);
            }

            // Note that the above loop only creates O(N) work for histograms that have values in
            // the lowest half-bucket (excluding the 0 value). Histograms that never have values
            // there (e.g. all integer value histograms used as internal storage in DoubleHistograms)
            // will never loop, and their shifts will remain O(1).
        }

        protected override void shiftNormalizingIndexByOffset(int offsetToAdd, bool lowestHalfBucketPopulated)
        {
            try
            {
                wrp.ReaderLock();
                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);
                int newNormalizingIndexOffset = getNormalizingIndexOffset() + offsetToAdd;
                setNormalizingIndexOffset(newNormalizingIndexOffset, offsetToAdd, lowestHalfBucketPopulated);
            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        internal protected override void resize(long newHighestTrackableValue)
        {
            try
            {
                wrp.ReaderLock();

                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);

                int newArrayLength = determineArrayLengthNeeded(newHighestTrackableValue);
                int countsDelta = newArrayLength - countsArrayLength;

                if (countsDelta <= 0)
                {
                    // This resize need was already covered by a concurrent resize op.
                    return;
                }

                int oldNormalizedZeroIndex =
                    NormalizeIndex(0, inactiveCountsNormalizingIndexOffset, inactiveCounts.Length);

                // Resize the current inactiveCounts:
                AtomicLongArray oldInactiveCounts = inactiveCounts;

                inactiveCounts = new AtomicLongArray(newArrayLength);

                // Copy inactive contents to newly sized inactiveCounts:
                for (int i = 0; i < oldInactiveCounts.Length; i++)
                {
                    inactiveCounts.SetValue(i, oldInactiveCounts.GetValue(i));
                }
                if (oldNormalizedZeroIndex != 0)
                {
                    // We need to shift the stuff from the zero index and up to the end of the array:
                    int newNormalizedZeroIndex = oldNormalizedZeroIndex + countsDelta;
                    int lengthToCopy = (newArrayLength - countsDelta) - oldNormalizedZeroIndex;
                    int src, dst;
                    for (src = oldNormalizedZeroIndex, dst = newNormalizedZeroIndex;
                        src < oldNormalizedZeroIndex + lengthToCopy;
                        src++, dst++)
                    {
                        inactiveCounts.SetValue(dst, oldInactiveCounts.GetValue(src));
                    }
                }

                // switch active and inactive:
                var tmp = activeCounts;
                var tmpOffset = activeCountsNormalizingIndexOffset;

                activeCounts = inactiveCounts;
                activeCountsNormalizingIndexOffset = inactiveCountsNormalizingIndexOffset;

                inactiveCounts = tmp;
                inactiveCountsNormalizingIndexOffset = tmpOffset;

                wrp.FlipPhase();

                // Resize the newly inactiveCounts:
                oldInactiveCounts = inactiveCounts;
                inactiveCounts = new AtomicLongArray(newArrayLength);

                // Copy inactive contents to newly sized inactiveCounts:
                for (int i = 0; i < oldInactiveCounts.Length; i++)
                {
                    inactiveCounts.SetValue(i, oldInactiveCounts.GetValue(i));
                }
                if (oldNormalizedZeroIndex != 0)
                {
                    // We need to shift the stuff from the zero index and up to the end of the array:
                    int newNormalizedZeroIndex = oldNormalizedZeroIndex + countsDelta;
                    int lengthToCopy = (newArrayLength - countsDelta) - oldNormalizedZeroIndex;
                    int src, dst;
                    for (src = oldNormalizedZeroIndex, dst = newNormalizedZeroIndex;
                        src < oldNormalizedZeroIndex + lengthToCopy;
                        src++, dst++)
                    {
                        inactiveCounts.SetValue(dst, oldInactiveCounts.GetValue(src));
                    }
                }

                // switch active and inactive again:
                tmp = activeCounts;
                activeCounts = inactiveCounts;
                inactiveCounts = tmp;

                wrp.FlipPhase();

                // At this point, both active and inactive have been safely resized,
                // and the switch in each was done without any writers modifying it in flight.

                // We resized things. We can now make the historam establish size accordingly for future recordings:
                establishSize(newHighestTrackableValue);

                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);

            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        protected internal override void clearCounts()
        {
            try
            {
                wrp.ReaderLock();
                Debug.Assert(countsArrayLength == activeCounts.Length);
                Debug.Assert(countsArrayLength == inactiveCounts.Length);
                for (int i = 0; i < activeCounts.Length; i++)
                {
                    activeCounts.SetValue(i, 0);
                    inactiveCounts.SetValue(i, 0);
                }
                totalCount.Reset();
            }
            finally
            {
                wrp.ReaderUnlock();
            }
        }

        public override AbstractHistogram copy()
        {
            ConcurrentHistogram copy = new ConcurrentHistogram(this);
            copy.add(this);
            return copy;
        }

        public override AbstractHistogram copyCorrectedForCoordinatedOmission(long expectedIntervalBetweenValueSamples)
        {
            ConcurrentHistogram toHistogram = new ConcurrentHistogram(this);
            toHistogram.addWhileCorrectingForCoordinatedOmission(this, expectedIntervalBetweenValueSamples);
            return toHistogram;
        }

        public override long getTotalCount()
        {
            return totalCount.GetValue();
        }

        protected override void setTotalCount(long totalCount)
        {
            this.totalCount.Reset();
            this.totalCount.Add(totalCount);
        }

        protected override void incrementTotalCount()
        {
            totalCount.Increment();
        }

        protected override void addToTotalCount(long value)
        {
            totalCount.Add(value);
        }

        protected internal override int _getEstimatedFootprintInBytes()
        {
            return (512 + (2 * 8 * activeCounts.Length));
        }

        /**
         * Construct an auto-resizing ConcurrentHistogram with a lowest discernible value of 1 and an auto-adjusting
         * highestTrackableValue. Can auto-resize up to track values up to (Long.MAX_VALUE / 2).
         *
         * @param numberOfSignificantValueDigits Specifies the precision to use. This is the number of significant
         *                                       decimal digits to which the histogram will maintain value resolution
         *                                       and separation. Must be a non-negative integer between 0 and 5.
         */

        public ConcurrentHistogram(int numberOfSignificantValueDigits)
            : this(1, 2, numberOfSignificantValueDigits)
        {
        }

        /**
         * Construct a ConcurrentHistogram given the Lowest and Highest values to be tracked and a number of significant
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
        public ConcurrentHistogram(long lowestDiscernibleValue, long highestTrackableValue, int numberOfSignificantValueDigits)
            : base(lowestDiscernibleValue, highestTrackableValue, numberOfSignificantValueDigits, wordSizeInBytes: sizeof(long), allocateCountsArray: false, autoResize: true)
        {
            activeCounts = new AtomicLongArray(countsArrayLength);
            activeCountsNormalizingIndexOffset = 0;

            inactiveCounts = new AtomicLongArray(countsArrayLength);
            inactiveCountsNormalizingIndexOffset = 0;
        }

        /**
         * Construct a histogram with the same range settings as a given source histogram,
         * duplicating the source's start/end timestamps (but NOT it's contents)
         * @param source The source histogram to duplicate
         */
        public ConcurrentHistogram(AbstractHistogram source)
            : base(source, false)
        {
            activeCounts = new AtomicLongArray(countsArrayLength);
            activeCountsNormalizingIndexOffset = 0;

            inactiveCounts = new AtomicLongArray(countsArrayLength);
            inactiveCountsNormalizingIndexOffset = 0;
        }
    }
}
