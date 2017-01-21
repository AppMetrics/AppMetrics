// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using App.Metrics.Concurrency;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
    // ReSharper disable InconsistentNaming
#pragma warning disable

    /// <summary>
    ///     Records integer values, and provides stable interval {@link Histogram} samples from
    ///     live recorded data without interrupting or stalling active recording of values. Each interval
    ///     histogram provided contains all value counts accumulated since the previous interval histogram
    ///     was taken.
    ///     This pattern is commonly used in logging interval histogram information while recording is ongoing.
    ///     {@link Recorder} supports concurrent
    ///     {@link Recorder#RecordValue} or
    ///     {@link Recorder#recordValueWithExpectedInterval} calls.
    ///     Recording calls are wait-free on architectures that support atomic increment operations, and
    ///     are lock-free on architectures that do not.
    /// </summary>
    public class Recorder
    {
        private static readonly long factor = 1000L / Stopwatch.Frequency;

        private static readonly AtomicLong instanceIdSequencer = new AtomicLong(1);
        private static readonly object syncLock = new object();

        // ReSharper disable ImpureMethodCallOnReadonlyValueField
        private readonly long instanceId = instanceIdSequencer.GetAndIncrement();

        // ReSharper restore ImpureMethodCallOnReadonlyValueField
        private readonly WriterReaderPhaser recordingPhaser = new WriterReaderPhaser();

        private volatile HdrHistogram activeHistogram;
        private HdrHistogram inactiveHistogram;

        /// <summary>
        ///     Construct an auto-resizing Recorder with a lowest discernible value of
        ///     1 and an auto-adjusting highestTrackableValue. Can auto-resize up to track values up to (long.MaxValue / 2).
        /// </summary>
        /// <param name="numberOfSignificantValueDigits">
        ///     Specifies the precision to use. This is the number of significant decimal
        ///     digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between
        ///     0 and 5.
        /// </param>
        public Recorder(int numberOfSignificantValueDigits)
        {
            activeHistogram = new InternalConcurrentHistogram(instanceId, numberOfSignificantValueDigits);
            inactiveHistogram = new InternalConcurrentHistogram(instanceId, numberOfSignificantValueDigits);
            activeHistogram.setStartTimeStamp(CurentTimeInMilis());
        }

        public static long CurentTimeInMilis() { return Stopwatch.GetTimestamp() * factor; }

        /// <summary>
        ///     Get a new instance of an interval histogram, which will include a stable, consistent view of all value
        ///     counts accumulated since the last interval histogram was taken.
        ///     Calling {@link Recorder#GetIntervalHistogram()} will reset
        ///     the value counts, and start accumulating value counts for the next interval.
        /// </summary>
        /// <returns>a histogram containing the value counts accumulated since the last interval histogram was taken.</returns>
        public HdrHistogram GetIntervalHistogram() { return GetIntervalHistogram(null); }

        /// <summary>
        ///     Get an interval histogram, which will include a stable, consistent view of all value counts
        ///     accumulated since the last interval histogram was taken.
        ///     {@link Recorder#GetIntervalHistogram(Histogram histogramToRecycle)
        ///     GetIntervalHistogram(histogramToRecycle)}
        ///     accepts a previously returned interval histogram that can be recycled internally to avoid allocation
        ///     and content copying operations, and is therefore significantly more efficient for repeated use than
        ///     {@link Recorder#GetIntervalHistogram()} and
        ///     {@link Recorder#getIntervalHistogramInto getIntervalHistogramInto()}. The provided
        ///     {@code histogramToRecycle} must
        ///     be either be null or an interval histogram returned by a previous call to
        ///     {@link Recorder#GetIntervalHistogram(Histogram histogramToRecycle)
        ///     GetIntervalHistogram(histogramToRecycle)} or
        ///     {@link Recorder#GetIntervalHistogram()}.
        ///     NOTE: The caller is responsible for not recycling the same returned interval histogram more than once. If
        ///     the same interval histogram instance is recycled more than once, behavior is undefined.
        ///     Calling {@link Recorder#GetIntervalHistogram(Histogram histogramToRecycle)
        ///     GetIntervalHistogram(histogramToRecycle)} will reset the value counts, and start accumulating value
        ///     counts for the next interval
        /// </summary>
        /// <param name="histogramToRecycle">
        ///     a previously returned interval histogram that may be recycled to avoid allocation and
        ///     copy operations.
        /// </param>
        /// <returns>a histogram containing the value counts accumulated since the last interval histogram was taken.</returns>
        public HdrHistogram GetIntervalHistogram(HdrHistogram histogramToRecycle)
        {
            lock (syncLock)
            {
                if (histogramToRecycle == null)
                {
                    histogramToRecycle = new InternalConcurrentHistogram(
                        instanceId,
                        inactiveHistogram.getNumberOfSignificantValueDigits());
                }

                // Verify that replacement histogram can validly be used as an inactive histogram replacement:
                ValidateFitAsReplacementHistogram(histogramToRecycle);
                try
                {
                    recordingPhaser.ReaderLock();
                    inactiveHistogram = histogramToRecycle;
                    PerformIntervalSample();
                    return inactiveHistogram;
                }
                finally
                {
                    recordingPhaser.ReaderUnlock();
                }
            }
        }

        /// <summary>
        ///     Record a value.
        /// </summary>
        /// <param name="value">The value to record.</param>
        public void RecordValue(long value)
        {
            // ReSharper disable InconsistentlySynchronizedField
            var criticalValueAtEnter = recordingPhaser.WriterCriticalSectionEnter();
            try
            {
                activeHistogram.RecordValue(value);
            }
            finally
            {
                recordingPhaser.WriterCriticalSectionExit(criticalValueAtEnter);
            }

            // ReSharper restore InconsistentlySynchronizedField
        }

        /// <summary>
        ///     Reset any value counts accumulated thus far.
        /// </summary>
        public void Reset()
        {
            lock (syncLock)
            {
                // the currently inactive histogram is reset each time we flip. So flipping twice resets both:
                PerformIntervalSample();
                PerformIntervalSample();
            }
        }

        private void PerformIntervalSample()
        {
            inactiveHistogram.reset();
            try
            {
                recordingPhaser.ReaderLock();

                // Swap active and inactive histograms:
                var tempHistogram = inactiveHistogram;
                inactiveHistogram = activeHistogram;
                activeHistogram = tempHistogram;

                // Mark end time of previous interval and start time of new one:
                var now = CurentTimeInMilis();
                activeHistogram.setStartTimeStamp(now);
                inactiveHistogram.setEndTimeStamp(now);

                // Make sure we are not in the middle of recording a value on the previously active histogram:

                // Flip phase to make sure no recordings that were in flight pre-flip are still active:
                recordingPhaser.FlipPhase(500000L /* yield in 0.5 msec units if needed */);
            }
            finally
            {
                recordingPhaser.ReaderUnlock();
            }
        }

        private void ValidateFitAsReplacementHistogram(HdrHistogram replacementHistogram)
        {
            var replacementConcurrentHistogram = replacementHistogram as InternalConcurrentHistogram;
            if (replacementConcurrentHistogram != null)
            {
                var activeConcurrentHistogram = activeHistogram as InternalConcurrentHistogram;
                if (activeConcurrentHistogram != null &&
                    replacementConcurrentHistogram.ContainingInstanceId ==
                    activeConcurrentHistogram.ContainingInstanceId)
                {
                    return;
                }
            }

            throw new ArgumentException(
                "replacement histogram must have been obtained via a previous" +
                "GetIntervalHistogram() call from this " + GetType().Name + " instance");
        }

        private class InternalConcurrentHistogram : ConcurrentHistogram
        {
            public readonly long ContainingInstanceId;

            public InternalConcurrentHistogram(long id, int numberOfSignificantValueDigits)
                : base(numberOfSignificantValueDigits) { ContainingInstanceId = id; }
        }
    }
}
#pragma warning restore

// ReSharper restore InconsistentNaming