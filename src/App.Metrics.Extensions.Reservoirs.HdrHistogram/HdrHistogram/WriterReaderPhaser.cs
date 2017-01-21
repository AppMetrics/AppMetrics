// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Concurrency;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    ///     WriterReaderPhaser instances provide an asymmetric means for synchronizing the execution of
    ///     wait-free "writer" critical sections against a "reader phase flip" that needs to make sure no writer critical
    ///     sections that were active at the beginning of the flip are still active after the flip is done. Multiple writers
    ///     and multiple readers are supported.
    ///     While a WriterReaderPhaser can be useful in multiple scenarios, a specific and common use case is
    ///     that of safely managing "double buffered" data stream access in which writers can proceed without being
    ///     blocked, while readers gain access to stable and unchanging buffer samples
    ///     <blockquote>
    ///         NOTE: WriterReaderPhaser writers are wait-free on architectures that support wait-free atomic
    ///         increment operations. They remain lock-free (but not wait-free) on architectures that do not support
    ///         wait-free atomic increment operations.
    ///     </blockquote>
    ///     WriterReaderPhaser "writers" are wait free, "readers" block for other "readers", and
    ///     "readers" are only blocked by "writers" whose critical was entered before the reader's
    ///     WriterReaderPhaser#flipPhase() attempt.
    ///     When used to protect an actively recording data structure, the assumptions on how readers and writers act are:
    ///     <ol>
    ///         <li>There are two sets of data structures ("active" and "inactive")</li>
    ///         <li>
    ///             Writing is done to the perceived active version (as perceived by the writer), and only
    ///             within critical sections delineated by {@link WriterReaderPhaser#writerCriticalSectionEnter}
    ///             and {@link WriterReaderPhaser#writerCriticalSectionExit}).
    ///         </li>
    ///         <li>
    ///             Only readers switch the perceived roles of the active and inactive data structures.
    ///             They do so only while under readerLock(), and only before calling flipPhase().
    ///         </li>
    ///     </ol>
    ///     When the above assumptions are met, {@link WriterReaderPhaser} guarantees that the inactive data structures are not
    ///     being modified by any writers while being read while under readerLock() protection after a flipPhase()
    ///     operation.
    /// </summary>
    internal class WriterReaderPhaser
    {
        private readonly object readerLockObject = new object();
        private PaddedAtomicLong evenEndEpoch = new PaddedAtomicLong(0);
        private PaddedAtomicLong oddEndEpoch = new PaddedAtomicLong(long.MinValue);
        private PaddedAtomicLong startEpoch = new PaddedAtomicLong(0);

        /// <summary>
        ///     Flip a phase in the {@link WriterReaderPhaser} instance, {@link WriterReaderPhaser#flipPhase()}
        ///     can only be called while holding the readerLock().
        ///     {@link WriterReaderPhaser#flipPhase()} will return only after all writer critical sections (protected by
        ///     {@link WriterReaderPhaser#writerCriticalSectionEnter()} ()} and
        ///     {@link WriterReaderPhaser#writerCriticalSectionExit(long)} ()}) that may have been in flight when the
        ///     {@link WriterReaderPhaser#flipPhase()} call were made had completed.
        ///     No actual writer critical section activity is required for {@link WriterReaderPhaser#flipPhase()} to
        ///     succeed.
        ///     However, {@link WriterReaderPhaser#flipPhase()} is lock-free with respect to calls to
        ///     {@link WriterReaderPhaser#writerCriticalSectionEnter()} and
        ///     {@link WriterReaderPhaser#writerCriticalSectionExit(long)}. It may spin-wait for for active
        ///     writer critical section code to complete.
        /// </summary>
        /// <param name="yieldTimeNsec">The amount of time (in nanoseconds) to sleep in each yield if yield loop is needed.</param>
        public void FlipPhase(long yieldTimeNsec = 0)
        {
            if (!Monitor.IsEntered(readerLockObject))
            {
                throw new ThreadStateException("flipPhase() can only be called while holding the readerLock()");
            }

            var nextPhaseIsEven = startEpoch.GetValue() < 0; // Current phase is odd...

            long initialStartValue;

            // First, clear currently unused [next] phase end epoch (to proper initial value for phase):
            if (nextPhaseIsEven)
            {
                initialStartValue = 0;
                evenEndEpoch.SetValue(initialStartValue);
            }
            else
            {
                initialStartValue = long.MinValue;
                oddEndEpoch.SetValue(initialStartValue);
            }

            // Next, reset start value, indicating new phase, and retain value at flip:
            var startValueAtFlip = startEpoch.GetAndSet(initialStartValue);

            // Now, spin until previous phase end value catches up with start value at flip:
            bool caughtUp;
            do
            {
                if (nextPhaseIsEven)
                {
                    caughtUp = oddEndEpoch.GetValue() == startValueAtFlip;
                }
                else
                {
                    caughtUp = evenEndEpoch.GetValue() == startValueAtFlip;
                }

                if (!caughtUp)
                {
                    if (yieldTimeNsec == 0)
                    {
                        Task.Yield();
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(yieldTimeNsec / 1000000.0));
                    }
                }
            }
            while (!caughtUp);
        }

        /// <summary>
        ///     Enter to a critical section containing a read operation (mutually excludes against other
        ///     {@link WriterReaderPhaser#readerLock} calls).
        ///     {@link WriterReaderPhaser#readerLock} DOES NOT provide synchronization
        ///     against {@link WriterReaderPhaser#writerCriticalSectionEnter()} calls. Use {@link WriterReaderPhaser#flipPhase()}
        ///     to synchronize reads against writers.
        /// </summary>
        public void ReaderLock() { Monitor.Enter(readerLockObject); }

        /// <summary>
        ///     Exit from a critical section containing a read operation (relinquishes mutual exclusion against other
        ///     {@link WriterReaderPhaser#readerLock} calls).
        /// </summary>
        public void ReaderUnlock() { Monitor.Exit(readerLockObject); }

        /// <summary>
        ///     Indicate entry to a critical section containing a write operation.
        ///     This call is wait-free on architectures that support wait free atomic increment operations,
        ///     and is lock-free on architectures that do not.
        ///     {@link WriterReaderPhaser#writerCriticalSectionEnter()} must be matched with a subsequent
        ///     {@link WriterReaderPhaser#writerCriticalSectionExit(long)} in order for CriticalSectionPhaser
        ///     synchronization to function properly.
        /// </summary>
        /// <returns>
        ///     an (opaque) value associated with the critical section entry, which MUST be provided to the matching {@link
        ///     WriterReaderPhaser#writerCriticalSectionExit} call.
        /// </returns>
        public long WriterCriticalSectionEnter() { return startEpoch.GetAndIncrement(); }

        /// <summary>
        ///     Indicate exit from a critical section containing a write operation.
        ///     This call is wait-free on architectures that support wait free atomic increment operations,
        ///     and is lock-free on architectures that do not.
        ///     {@link WriterReaderPhaser#writerCriticalSectionExit(long)} must be matched with a preceding
        ///     {@link WriterReaderPhaser#writerCriticalSectionEnter()} call, and must be provided with the
        ///     matching {@link WriterReaderPhaser#writerCriticalSectionEnter()} call's return value, in
        ///     order for CriticalSectionPhaser synchronization to function properly.
        /// </summary>
        /// <param name="criticalValueAtEnter">
        ///     the (opaque) value returned from the matching {@link
        ///     WriterReaderPhaser#writerCriticalSectionEnter()} call.
        /// </param>
        public void WriterCriticalSectionExit(long criticalValueAtEnter)
        {
            if (criticalValueAtEnter < 0)
            {
                oddEndEpoch.GetAndIncrement();
            }
            else
            {
                evenEndEpoch.GetAndIncrement();
            }
        }
    }
}

// ReSharper restore InconsistentNaming