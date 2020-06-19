// <copyright file="StripedLongAdder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Concurrency.Internal;

// Striped64 & LongAdder classes were ported from Java and had this copyright:
// Written by Doug Lea with assistance from members of JCP JSR-166 Expert Group and released to the public domain, as explained at http://creativecommons.org/publicdomain/zero/1.0/

// Source: http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/jsr166e/Striped64.java?revision=1.8

// This class was been ported to .NET by Iulian Margarintescu and will retain the same license as the Java Version
// Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/StripedLongAdder.cs

// Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the
// source and packages are no longer maintained

// ReSharper disable TooWideLocalVariableScope
// ReSharper disable InvertIf
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     One or more variables that together maintain an initially zero sum.
    ///     When updates are contended cross threads, the set of variables may grow dynamically to reduce contention.
    ///     Method GetValue() returns the current total combined across the variables maintaining the sum.
    ///     This class is usually preferable to AtomicLong when multiple threads update a common sum that is used for purposes
    ///     such  as collecting statistics, not for fine-grained synchronization control.
    ///     Under low update contention, the two classes have similar characteristics.
    ///     But under high contention, expected throughput of this class is significantly higher, at the expense of higher
    ///     space consumption.
    /// </summary>
    public sealed class StripedLongAdder : Striped64, IValueAdder<long>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StripedLongAdder" /> class.
        /// </summary>
        public StripedLongAdder() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StripedLongAdder" /> class.
        /// </summary>
        /// <param name="value">The initial value of the instance.</param>
        /// <remarks>
        ///     Creates a new instance of the adder with initial <paramref name="value" />.
        /// </remarks>
        public StripedLongAdder(long value) { Add(value); }

        /// <summary>
        ///     Returns the size in bytes occupied by an Striped64 instance.
        /// </summary>
        /// <param name="instance">instance for whch to calculate the size.</param>
        /// <returns>The size of the instance in bytes.</returns>
        public static int GetEstimatedFootprintInBytes(StripedLongAdder instance) { return Striped64.GetEstimatedFootprintInBytes(instance); }

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
            if ((@as = Cells) != null || !Base.CompareAndSwap(b = Base.GetValue(), b + value))
            {
                var uncontended = true;
                if (@as == null || (m = @as.Length - 1) < 0 || (a = @as[GetProbe() & m]) == null ||
                    !(uncontended = a.Value.CompareAndSwap(v = a.Value.GetValue(), v + value)))
                {
                    LongAccumulate(value, uncontended);
                }
            }
        }

        /// <summary>
        ///     Decrement the value of this instance.
        /// </summary>
        public void Decrement() { Add(-1L); }

        /// <summary>
        ///     Decrement the value of this instance with <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Decrement(long value) { Add(-value); }

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
            var @as = Cells;
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
        ///     Returns the current value of this adder. This method sums all the buckets and returns the result.
        /// </summary>
        /// <returns>The current value recored by this adder.</returns>
        public long GetValue()
        {
            var @as = Cells;
            Cell a;
            var sum = Base.GetValue();
            if (@as != null)
            {
                for (var i = 0; i < @as.Length; ++i)
                {
                    if ((a = @as[i]) != null)
                    {
                        sum += a.Value.GetValue();
                    }
                }
            }

            return sum;
        }

        /// <summary>
        ///     Increment the value of this instance.
        /// </summary>
        public void Increment() { Add(1L); }

        /// <summary>
        ///     Increment the value of this instance with <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Increment(long value) { Add(value); }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public long NonVolatileGetValue()
        {
            var @as = Cells;
            Cell a;
            var sum = Base.NonVolatileGetValue();
            if (@as != null)
            {
                for (var i = 0; i < @as.Length; ++i)
                {
                    if ((a = @as[i]) != null)
                    {
                        sum += a.Value.NonVolatileGetValue();
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
            var @as = Cells;
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
    }
}