// <copyright file="Striped64.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;

// Striped64 & LongAdder classes were ported from Java and had this copyright:
// Written by Doug Lea with assistance from members of JCP JSR-166

// Expert Group and released to the public domain, as explained at http://creativecommons.org/publicdomain/zero/1.0/

// Source: http: //gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/jsr166e/Striped64.java?revision=1.8

// This class was ported to .NET by Iulian Margarintescu and will retain the same license as the Java Version

// Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/Striped64.cs
// Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
// ReSharper disable TooWideLocalVariableScope
namespace App.Metrics.Concurrency
{
#pragma warning disable SA1407, SA1108, SA1306, SA1401
    /// <summary>
    ///     A class holding common representation and mechanics for classes supporting dynamic striping on 64bit values.
    /// </summary>
    public abstract class Striped64
    {
        protected AtomicLong Base = new AtomicLong(0);

        protected volatile Cell[] Cells;

        private static readonly ThreadLocal<ThreadHashCode> HashCode = new ThreadLocal<ThreadHashCode>(() => new ThreadHashCode());
        private static readonly int ProcessorCount = Environment.ProcessorCount;

        private int _cellsBusy; // no need for volatile as we only update with Interlocked.CompareExchange

        /// <summary>
        ///     Returns the size in bytes occupied by an Striped64 instance.
        /// </summary>
        /// <param name="instance">instance for whch to calculate the size.</param>
        /// <returns>The size of the instance in bytes.</returns>
        protected static int GetEstimatedFootprintInBytes(Striped64 instance)
        {
            var cells = instance.Cells;
            var cellsLength = cells?.Length ?? 0;
            var nonNullCells = 0;
            if (cells != null)
            {
                foreach (var cell in cells)
                {
                    if (cell != null)
                    {
                        nonNullCells++;
                    }
                }
            }

            return AtomicLong.SizeInBytes + // base
                   sizeof(int) + // cellsBusy
                   IntPtr.Size + // cells reference
                   cellsLength * IntPtr.Size + // size of array of references to cells
                   nonNullCells * Cell.SizeInBytes; // size of non null cells
        }

        protected static int GetProbe() { return HashCode.Value.Code; }

        protected void LongAccumulate(long x, bool wasUncontended)
        {
            var h = GetProbe();

            var collide = false; // True if last slot nonempty
            do
            {
                Cell[] @as;
                Cell a;
                int n;
                long v;
                if ((@as = Cells) != null && (n = @as.Length) > 0)
                {
                    if ((a = @as[(n - 1) & h]) == null)
                    {
                        if (_cellsBusy == 0)
                        {
                            // Try to attach new Cell
                            var r = new Cell(x); // Optimistically create
                            if (_cellsBusy == 0 && CasCellsBusy())
                            {
                                var created = false;
                                try
                                {
                                    // Recheck under lock
                                    Cell[] rs;
                                    int m, j;
                                    if ((rs = Cells) != null &&
                                        (m = rs.Length) > 0 &&
                                        rs[j = (m - 1) & h] == null)
                                    {
                                        rs[j] = r;
                                        created = true;
                                    }
                                }
                                finally
                                {
                                    _cellsBusy = 0;
                                }

                                if (created)
                                {
                                    break;
                                }

                                continue; // Slot is now non-empty
                            }
                        }

                        collide = false;
                    }
                    else if (!wasUncontended) // CAS already known to fail
                    {
                        wasUncontended = true; // Continue after rehash
                    }
                    else if (a.Value.CompareAndSwap(v = a.Value.GetValue(), v + x))
                    {
                        break;
                    }
                    else if (n >= ProcessorCount || Cells != @as)
                    {
                        collide = false; // At max size or stale
                    }
                    else if (!collide)
                    {
                        collide = true;
                    }
                    else if (_cellsBusy == 0 && CasCellsBusy())
                    {
                        try
                        {
                            if (Cells == @as)
                            {
                                // Expand table unless stale
                                var rs = new Cell[n << 1];
                                for (var i = 0; i < n; ++i)
                                {
                                    rs[i] = @as[i];
                                }

                                Cells = rs;
                            }
                        }
                        finally
                        {
                            _cellsBusy = 0;
                        }

                        collide = false;
                        continue; // Retry with expanded table
                    }

                    h = AdvanceProbe(h);
                }
                else if (_cellsBusy == 0 && Cells == @as && CasCellsBusy())
                {
                    var init = false;
                    try
                    {
                        // Initialize table
                        if (Cells == @as)
                        {
                            var rs = new Cell[2];
                            rs[h & 1] = new Cell(x);
                            Cells = rs;
                            init = true;
                        }
                    }
                    finally
                    {
                        _cellsBusy = 0;
                    }

                    if (init)
                    {
                        break;
                    }
                }
                else if (Base.CompareAndSwap(v = Base.GetValue(), v + x))
                {
                    break; // Fall back on using volatileBase
                }
            }
            while (true);
        }

        private static int AdvanceProbe(int probe)
        {
            probe ^= probe << 13; // xorshift
            probe ^= (int)((uint)probe >> 17);
            probe ^= probe << 5;
            HashCode.Value.Code = probe;
            return probe;
        }

        private bool CasCellsBusy() { return Interlocked.CompareExchange(ref _cellsBusy, 1, 0) == 0; }

        protected sealed class Cell
        {
            public static readonly int SizeInBytes = PaddedAtomicLong.SizeInBytes + 16;

            public PaddedAtomicLong Value;

            public Cell(long x) { Value = new PaddedAtomicLong(x); }
        }

        private class ThreadHashCode
        {
            public int Code = ThreadLocalRandom.Next(1, int.MaxValue);
        }
    }
#pragma warning restore SA1407
}