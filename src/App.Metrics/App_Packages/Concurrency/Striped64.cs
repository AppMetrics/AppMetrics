// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported to .NET Standard Library by Allan Hardy

// This is a collection of .NET concurrency utilities, inspired by the classes
// available in java. This utilities are Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET as described here
// https://github.com/etishor/ConcurrencyUtilities
// 
//
// Striped64 & LongAdder classes were ported from Java and had this copyright:
// 
// Written by Doug Lea with assistance from members of JCP JSR-166
// Expert Group and released to the public domain, as explained at
// http://creativecommons.org/publicdomain/zero/1.0/
// 
// Source: http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/jsr166e/Striped64.java?revision=1.8

//
// By default all added classes are internal to your assembly. 
// To make them public define you have to define the conditional compilation symbol CONCURRENCY_UTILS_PUBLIC in your project properties.
//

#pragma warning disable 1591

// ReSharper disable All

/*
 * Striped64 & LongAdder classes were ported from Java and had this copyright:
 * 
 * Written by Doug Lea with assistance from members of JCP JSR-166
 * Expert Group and released to the public domain, as explained at
 * http://creativecommons.org/publicdomain/zero/1.0/
 * 
 * Source: http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/jsr166e/Striped64.java?revision=1.8
 * 
 * This class has been ported to .NET by Iulian Margarintescu and will retain the same license as the Java Version
 * 
 */

using System;
using System.Threading;

// ReSharper disable TooWideLocalVariableScope

namespace App.Metrics.App_Packages.Concurrency
{
    /// <summary>
    ///     A class holding common representation and mechanics for classes supporting dynamic striping on 64bit values.
    /// </summary>
#if CONCURRENCY_UTILS_PUBLIC
public
#else
    internal
#endif
        abstract class Striped64
    {
        private static readonly int processorCount = Environment.ProcessorCount;

        protected class Cell
        {
            public PaddedAtomicLong Value;

            public Cell(long x)
            {
                Value = new PaddedAtomicLong(x);
            }
        }

        protected volatile Cell[] Cells;
        protected AtomicLong Base = new AtomicLong(0);

        private int _cellsBusy; // no need for volatile as we only update with Interlocked.CompareExchange

        private bool CasCellsBusy()
        {
            return Interlocked.CompareExchange(ref _cellsBusy, 1, 0) == 0;
        }

        protected void LongAccumulate(long x, bool wasUncontended)
        {
            var h = GetProbe();

            var collide = false; // True if last slot nonempty
            for (;;)
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
                                    break;
                                continue; // Slot is now non-empty
                            }
                        }
                        collide = false;
                    }
                    else if (!wasUncontended) // CAS already known to fail
                        wasUncontended = true; // Continue after rehash
                    else if (a.Value.CompareAndSwap(v = a.Value.GetValue(), v + x))
                        break;
                    else if (n >= processorCount || Cells != @as)
                        collide = false; // At max size or stale
                    else if (!collide)
                        collide = true;
                    else if (_cellsBusy == 0 && CasCellsBusy())
                    {
                        try
                        {
                            if (Cells == @as)
                            {
                                // Expand table unless stale
                                var rs = new Cell[n << 1];
                                for (var i = 0; i < n; ++i)
                                    rs[i] = @as[i];
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
                        break;
                }
                else if (Base.CompareAndSwap(v = Base.GetValue(), v + x))
                    break; // Fall back on using volatileBase
            }
        }

        protected static int GetProbe()
        {
            return HashCode.Value.Code;
        }

        private static int AdvanceProbe(int probe)
        {
            probe ^= probe << 13; // xorshift
            probe ^= (int)((uint)probe >> 17);
            probe ^= probe << 5;
            HashCode.Value.Code = probe;
            return probe;
        }

        private static readonly ThreadLocal<ThreadHashCode> HashCode = new ThreadLocal<ThreadHashCode>(() => new ThreadHashCode());

        private class ThreadHashCode
        {
            public int Code = ThreadLocalRandom.Next(1, int.MaxValue);
        }
    }
}