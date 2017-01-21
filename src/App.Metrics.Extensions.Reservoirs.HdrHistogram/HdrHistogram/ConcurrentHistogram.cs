// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Diagnostics;
using App.Metrics.Concurrency;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
#pragma warning disable


    // ReSharper disable InconsistentNaming

    /// <summary>
    ///     <h3>An integer values High Dynamic Range (HDR) Histogram that supports safe concurrent recording operations.</h3>
    ///     A ConcurrentHistogram guarantees lossless recording of values into the histogram even when the
    ///     histogram is updated by multiple threads, and supports auto-resize and shift operations that may
    ///     result from or occur concurrently with other recording operations.
    ///     It is important to note that concurrent recording, auto-sizing, and value shifting are the only thread-safe
    ///     behaviors provided by {@link ConcurrentHistogram}, and that it is not otherwise synchronized. Specifically, {@link
    ///     ConcurrentHistogram} provides no implicit synchronization that would prevent the contents of the histogram
    ///     from changing during queries, iterations, copies, or addition operations on the histogram. Callers wishing to make
    ///     potentially concurrent, multi-threaded updates that would safely work in the presence of queries, copies, or
    ///     additions of histogram objects should either take care to externally synchronize and/or order their access,
    ///     use the {@link SynchronizedHistogram} variant, or (recommended) use {@link Recorder} or
    ///     {@link SingleWriterRecorder} which are intended for this purpose.
    ///     Auto-resizing: When constructed with no specified value range range (or when auto-resize is turned on with {@link
    ///     Histogram#setAutoResize}) a {@link Histogram} will auto-resize its dynamic range to include recorded values as
    ///     they are encountered. Note that recording calls that cause auto-resizing may take longer to execute, as resizing
    ///     incurs allocation and copying of internal data structures.
    ///     See package description for {@link org.HdrHistogram} for details.
    /// </summary>
    internal class ConcurrentHistogram : HdrHistogram
    {
        private readonly WriterReaderPhaser _wrp = new WriterReaderPhaser();
        private new readonly StripedLongAdder TotalCount = new StripedLongAdder();
        private AtomicLongArray _activeCounts;

        private int _activeCountsNormalizingIndexOffset;
        private AtomicLongArray _inactiveCounts;

        private int _inactiveCountsNormalizingIndexOffset;

        /**
         * Construct an auto-resizing ConcurrentHistogram with a lowest discernible value of 1 and an auto-adjusting
         * highestTrackableValue. Can auto-resize up to track values up to (Long.MAX_VALUE / 2).
         *
         * @param numberOfSignificantValueDigits Specifies the precision to use. This is the number of significant
         *                                       decimal digits to which the histogram will maintain value resolution
         *                                       and separation. Must be a non-negative integer between 0 and 5.
         */

        public ConcurrentHistogram(int numberOfSignificantValueDigits)
            : this(1, 2, numberOfSignificantValueDigits) { }

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
            : base(
                lowestDiscernibleValue,
                highestTrackableValue,
                numberOfSignificantValueDigits,
                wordSizeInBytes: sizeof(long),
                allocateCountsArray: false,
                autoResize: true)
        {
            _activeCounts = new AtomicLongArray(countsArrayLength);
            _activeCountsNormalizingIndexOffset = 0;

            _inactiveCounts = new AtomicLongArray(countsArrayLength);
            _inactiveCountsNormalizingIndexOffset = 0;
        }

        /**
         * Construct a histogram with the same range settings as a given source histogram,
         * duplicating the source's start/end timestamps (but NOT it's contents)
         * @param source The source histogram to duplicate
         */

        public ConcurrentHistogram(AbstractHistogram source)
            : base(source, false)
        {
            _activeCounts = new AtomicLongArray(countsArrayLength);
            _activeCountsNormalizingIndexOffset = 0;

            _inactiveCounts = new AtomicLongArray(countsArrayLength);
            _inactiveCountsNormalizingIndexOffset = 0;
        }

        public override AbstractHistogram copy()
        {
            var copy = new ConcurrentHistogram(this);
            copy.add(this);
            return copy;
        }

        public override AbstractHistogram copyCorrectedForCoordinatedOmission(long expectedIntervalBetweenValueSamples)
        {
            var toHistogram = new ConcurrentHistogram(this);
            toHistogram.addWhileCorrectingForCoordinatedOmission(this, expectedIntervalBetweenValueSamples);
            return toHistogram;
        }

        public override long getTotalCount() { return TotalCount.GetValue(); }

        internal override long getCountAtIndex(int index)
        {
            try
            {
                _wrp.ReaderLock();
                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);

                var activeCount = _activeCounts.GetValue(NormalizeIndex(index, _activeCountsNormalizingIndexOffset, _activeCounts.Length));
                var inactiveCount = _inactiveCounts.GetValue(NormalizeIndex(index, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length));
                return activeCount + inactiveCount;
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        protected internal override int _getEstimatedFootprintInBytes() { return 512 + 2 * 8 * _activeCounts.Length; }

        protected internal override void clearCounts()
        {
            try
            {
                _wrp.ReaderLock();
                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);
                for (var i = 0; i < _activeCounts.Length; i++)
                {
                    _activeCounts.SetValue(i, 0);
                    _inactiveCounts.SetValue(i, 0);
                }

                TotalCount.Reset();
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        // ReSharper disable ArrangeModifiersOrder
        internal protected override void resize(long newHighestTrackableValue)

            // ReSharper restore ArrangeModifiersOrder
        {
            try
            {
                _wrp.ReaderLock();

                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);

                var newArrayLength = determineArrayLengthNeeded(newHighestTrackableValue);
                var countsDelta = newArrayLength - countsArrayLength;

                if (countsDelta <= 0)
                {
                    // This resize need was already covered by a concurrent resize op.
                    return;
                }

                var oldNormalizedZeroIndex =
                    NormalizeIndex(0, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length);

                // Resize the current inactiveCounts:
                var oldInactiveCounts = _inactiveCounts;

                _inactiveCounts = new AtomicLongArray(newArrayLength);

                // Copy inactive contents to newly sized inactiveCounts:
                for (var i = 0; i < oldInactiveCounts.Length; i++)
                {
                    _inactiveCounts.SetValue(i, oldInactiveCounts.GetValue(i));
                }

                if (oldNormalizedZeroIndex != 0)
                {
                    // We need to shift the stuff from the zero index and up to the end of the array:
                    var newNormalizedZeroIndex = oldNormalizedZeroIndex + countsDelta;
                    var lengthToCopy = newArrayLength - countsDelta - oldNormalizedZeroIndex;
                    int src, dst;
                    for (src = oldNormalizedZeroIndex, dst = newNormalizedZeroIndex;
                         src < oldNormalizedZeroIndex + lengthToCopy;
                         src++, dst++)
                    {
                        _inactiveCounts.SetValue(dst, oldInactiveCounts.GetValue(src));
                    }
                }

                // switch active and inactive:
                var tmp = _activeCounts;
                var tmpOffset = _activeCountsNormalizingIndexOffset;

                _activeCounts = _inactiveCounts;
                _activeCountsNormalizingIndexOffset = _inactiveCountsNormalizingIndexOffset;

                _inactiveCounts = tmp;
                _inactiveCountsNormalizingIndexOffset = tmpOffset;

                _wrp.FlipPhase();

                // Resize the newly inactiveCounts:
                oldInactiveCounts = _inactiveCounts;
                _inactiveCounts = new AtomicLongArray(newArrayLength);

                // Copy inactive contents to newly sized inactiveCounts:
                for (var i = 0; i < oldInactiveCounts.Length; i++)
                {
                    _inactiveCounts.SetValue(i, oldInactiveCounts.GetValue(i));
                }

                if (oldNormalizedZeroIndex != 0)
                {
                    // We need to shift the stuff from the zero index and up to the end of the array:
                    var newNormalizedZeroIndex = oldNormalizedZeroIndex + countsDelta;
                    var lengthToCopy = newArrayLength - countsDelta - oldNormalizedZeroIndex;
                    int src, dst;
                    for (src = oldNormalizedZeroIndex, dst = newNormalizedZeroIndex;
                         src < oldNormalizedZeroIndex + lengthToCopy;
                         src++, dst++)
                    {
                        _inactiveCounts.SetValue(dst, oldInactiveCounts.GetValue(src));
                    }
                }

                // switch active and inactive again:
                tmp = _activeCounts;
                _activeCounts = _inactiveCounts;
                _inactiveCounts = tmp;

                _wrp.FlipPhase();

                // At this point, both active and inactive have been safely resized,
                // and the switch in each was done without any writers modifying it in flight.

                // We resized things. We can now make the historam establish size accordingly for future recordings:
                establishSize(newHighestTrackableValue);

                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        protected override void addToCountAtIndex(int index, long value)
        {
            var criticalValue = _wrp.WriterCriticalSectionEnter();
            try
            {
                _activeCounts.Add(NormalizeIndex(index, _activeCountsNormalizingIndexOffset, _activeCounts.Length), value);
            }
            finally
            {
                _wrp.WriterCriticalSectionExit(criticalValue);
            }
        }

        protected override void addToTotalCount(long value) { TotalCount.Add(value); }

        protected override long getCountAtNormalizedIndex(int index)
        {
            try
            {
                _wrp.ReaderLock();
                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);
                var activeCount = _activeCounts.GetValue(index);
                var inactiveCount = _inactiveCounts.GetValue(index);
                return activeCount + inactiveCount;
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        protected override int getNormalizingIndexOffset() { return _activeCountsNormalizingIndexOffset; }

        protected override void incrementCountAtIndex(int index)
        {
            var criticalValue = _wrp.WriterCriticalSectionEnter();
            try
            {
                _activeCounts.Increment(NormalizeIndex(index, _activeCountsNormalizingIndexOffset, _activeCounts.Length));
            }
            finally
            {
                _wrp.WriterCriticalSectionExit(criticalValue);
            }
        }

        protected override void incrementTotalCount() { TotalCount.Increment(); }

        protected override void setCountAtIndex(int index, long value)
        {
            try
            {
                _wrp.ReaderLock();
                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);
                _activeCounts.SetValue(NormalizeIndex(index, _activeCountsNormalizingIndexOffset, _activeCounts.Length), value);
                _inactiveCounts.SetValue(NormalizeIndex(index, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length), 0);
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        protected override void setCountAtNormalizedIndex(int index, long value)
        {
            try
            {
                _wrp.ReaderLock();
                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);
                _inactiveCounts.SetValue(index, value);
                _activeCounts.SetValue(index, 0);
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        protected override void setNormalizingIndexOffset(int normalizingIndexOffset) { setNormalizingIndexOffset(normalizingIndexOffset, 0, false); }

        protected override void setTotalCount(long totalCount)
        {
            TotalCount.Reset();
            TotalCount.Add(totalCount);
        }

        protected override void shiftNormalizingIndexByOffset(int offsetToAdd, bool lowestHalfBucketPopulated)
        {
            try
            {
                _wrp.ReaderLock();
                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);
                var newNormalizingIndexOffset = getNormalizingIndexOffset() + offsetToAdd;
                setNormalizingIndexOffset(newNormalizingIndexOffset, offsetToAdd, lowestHalfBucketPopulated);
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        private void setNormalizingIndexOffset(
            int normalizingIndexOffset,
            int shiftedAmount,
            bool lowestHalfBucketPopulated)
        {
            try
            {
                _wrp.ReaderLock();

                Debug.Assert(countsArrayLength == _activeCounts.Length);
                Debug.Assert(countsArrayLength == _inactiveCounts.Length);

                if (normalizingIndexOffset == _activeCountsNormalizingIndexOffset)
                {
                    return; // Nothing to do.
                }

                // Save and clear the inactive 0 value count:
                var zeroIndex = NormalizeIndex(0, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length);
                var inactiveZeroValueCount = _inactiveCounts.GetValue(zeroIndex);
                _inactiveCounts.SetValue(zeroIndex, 0);

                // Change the normalizingIndexOffset on the current inactiveCounts:
                _inactiveCountsNormalizingIndexOffset = normalizingIndexOffset;

                // inactiveCounts.setNormalizingIndexOffset(normalizingIndexOffset);

                // Handle the inactive lowest half bucket:
                if (shiftedAmount > 0 && lowestHalfBucketPopulated)
                {
                    shiftLowestInactiveHalfBucketContentsLeft(shiftedAmount);
                }

                // Restore the inactive 0 value count:
                zeroIndex = NormalizeIndex(0, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length);
                _inactiveCounts.SetValue(zeroIndex, inactiveZeroValueCount);

                // switch active and inactive:
                var tmp = _activeCounts;
                var tmpOffset = _activeCountsNormalizingIndexOffset;

                _activeCounts = _inactiveCounts;
                _activeCountsNormalizingIndexOffset = _inactiveCountsNormalizingIndexOffset;

                _inactiveCounts = tmp;
                _inactiveCountsNormalizingIndexOffset = tmpOffset;

                _wrp.FlipPhase();

                // Save and clear the newly inactive 0 value count:
                zeroIndex = NormalizeIndex(0, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length);
                inactiveZeroValueCount = _inactiveCounts.GetValue(zeroIndex);
                _inactiveCounts.SetValue(zeroIndex, 0);

                // Change the normalizingIndexOffset on the newly inactiveCounts:
                _inactiveCountsNormalizingIndexOffset = normalizingIndexOffset;

                // inactiveCounts.setNormalizingIndexOffset(normalizingIndexOffset);

                // Handle the newly inactive lowest half bucket:
                if (shiftedAmount > 0 && lowestHalfBucketPopulated)
                {
                    shiftLowestInactiveHalfBucketContentsLeft(shiftedAmount);
                }

                // Restore the newly inactive 0 value count:
                zeroIndex = NormalizeIndex(0, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length);
                _inactiveCounts.SetValue(zeroIndex, inactiveZeroValueCount);

                // switch active and inactive again:
                tmp = _activeCounts;
                tmpOffset = _activeCountsNormalizingIndexOffset;

                _activeCounts = _inactiveCounts;
                _activeCountsNormalizingIndexOffset = _inactiveCountsNormalizingIndexOffset;

                _inactiveCounts = tmp;
                _inactiveCountsNormalizingIndexOffset = tmpOffset;

                _wrp.FlipPhase();

                // At this point, both active and inactive have normalizingIndexOffset safely set,
                // and the switch in each was done without any writers using the wrong value in flight.
            }
            finally
            {
                _wrp.ReaderUnlock();
            }
        }

        private void shiftLowestInactiveHalfBucketContentsLeft(int shiftAmount)
        {
            var numberOfBinaryOrdersOfMagnitude = shiftAmount >> subBucketHalfCountMagnitude;

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
            for (var fromIndex = 1; fromIndex < subBucketHalfCount; fromIndex++)
            {
                var toValue = ValueFromIndex(fromIndex) << numberOfBinaryOrdersOfMagnitude;
                var toIndex = CountsArrayIndex(toValue);
                var normalizedToIndex =
                    NormalizeIndex(toIndex, _inactiveCountsNormalizingIndexOffset, _inactiveCounts.Length);
                var countAtFromIndex = _inactiveCounts.GetValue(fromIndex);
                _inactiveCounts.SetValue(normalizedToIndex, countAtFromIndex);
                _inactiveCounts.SetValue(fromIndex, 0);
            }

            // Note that the above loop only creates O(N) work for histograms that have values in
            // the lowest half-bucket (excluding the 0 value). Histograms that never have values
            // there (e.g. all integer value histograms used as internal storage in DoubleHistograms)
            // will never loop, and their shifts will remain O(1).
        }
    }
#pragma warning restore
}

// ReSharper restore InconsistentNaming