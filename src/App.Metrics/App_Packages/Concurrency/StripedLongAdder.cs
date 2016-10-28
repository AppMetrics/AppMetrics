// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


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

// ReSharper disable TooWideLocalVariableScope
// ReSharper disable InvertIf
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery

namespace App.Metrics.App_Packages.Concurrency
{
    /// <summary>
    ///     One or more variables that together maintain an initially zero sum.
    ///     When updates are contended cross threads, the set of variables may grow dynamically to reduce contention.
    ///     Method GetValue() returns the current total combined across the variables maintaining the sum.
    ///     This class is usually preferable to AtomicLong when multiple threads update a common sum that is used for purposes
    ///     such
    ///     as collecting statistics, not for fine-grained synchronization control.
    ///     Under low update contention, the two classes have similar characteristics.
    ///     But under high contention, expected throughput of this class is significantly higher, at the expense of higher
    ///     space consumption.
    /// </summary>
#if CONCURRENCY_UTILS_PUBLIC
public
#else
    internal
#endif
        sealed class StripedLongAdder : Striped64
    {
        /// <summary>
        ///     Creates a new instance of the adder with initial value of zero.
        /// </summary>
        public StripedLongAdder()
        {
        }

        /// <summary>
        ///     Creates a new instance of the adder with initial <paramref name="value" />.
        /// </summary>
        public StripedLongAdder(long value)
        {
            Add(value);
        }

        /// <summary>
        ///     Returns the current value of this adder. This method sums all the buckets and returns the result.
        /// </summary>
        /// <returns>The current value recored by this adder.</returns>
        public long GetValue()
        {
            var @as = this.Cells;
            Cell a;
            var sum = Base.GetValue();
            if (@as != null)
            {
                for (var i = 0; i < @as.Length; ++i)
                {
                    if ((a = @as[i]) != null)
                        sum += a.Value.GetValue();
                }
            }
            return sum;
        }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public long NonVolatileGetValue()
        {
            var @as = this.Cells;
            Cell a;
            var sum = Base.NonVolatileGetValue();
            if (@as != null)
            {
                for (var i = 0; i < @as.Length; ++i)
                {
                    if ((a = @as[i]) != null)
                        sum += a.Value.NonVolatileGetValue();
                }
            }
            return sum;
        }

        /// <summary>
        ///     Returns the current value of this adder and resets the value to zero.
        ///     This method sums all the buckets, resets their value and returns the result.
        /// </summary>
        /// <remarks>
        ///     This method is thread-safe. If updates happen during this method, they are either included in the final sum, or
        ///     reflected in the value after the reset.
        /// </remarks>
        /// <returns>The current value recored by this adder.</returns>
        public long GetAndReset()
        {
            var @as = this.Cells;
            Cell a;
            var sum = Base.GetAndReset();
            if (@as != null)
            {
                for (var i = 0; i < @as.Length; ++i)
                {
                    if ((a = @as[i]) != null)
                    {
                        sum += a.Value.GetAndReset();
                    }
                }
            }
            return sum;
        }

        /// <summary>
        ///     Resets the current value to zero.
        /// </summary>
        public void Reset()
        {
            var @as = this.Cells;
            Cell a;
            Base.SetValue(0L);
            if (@as != null)
            {
                for (var i = 0; i < @as.Length; ++i)
                {
                    if ((a = @as[i]) != null)
                    {
                        a.Value.SetValue(0L);
                    }
                }
            }
        }

        /// <summary>
        ///     Increment the value of this instance.
        /// </summary>
        public void Increment()
        {
            Add(1L);
        }

        /// <summary>
        ///     Increment the value of this instance with <paramref name="value" />.
        /// </summary>
        public void Increment(long value)
        {
            Add(value);
        }

        /// <summary>
        ///     Decrement the value of this instance.
        /// </summary>
        public void Decrement()
        {
            Add(-1L);
        }

        /// <summary>
        ///     Decrement the value of this instance with <paramref name="value" />.
        /// </summary>
        public void Decrement(long value)
        {
            Add(-value);
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(long value)
        {
            Cell[] @as;
            long b, v;
            int m;
            Cell a;
            if ((@as = this.Cells) != null || !Base.CompareAndSwap(b = Base.GetValue(), b + value))
            {
                var uncontended = true;
                if (@as == null || (m = @as.Length - 1) < 0 || (a = @as[GetProbe() & m]) == null ||
                    !(uncontended = a.Value.CompareAndSwap(v = a.Value.GetValue(), v + value)))
                {
                    LongAccumulate(value, uncontended);
                }
            }
        }
    }
}