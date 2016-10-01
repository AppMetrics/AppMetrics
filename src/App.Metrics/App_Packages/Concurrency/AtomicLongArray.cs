// Written by Iulian Margarintescu
// Ported to .NET Standard Library by Allan Hardy

// This is a collection of .NET concurrency utilities, inspired by the classes
// available in java. This utilities are written by Iulian Margarintescu as described here
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


using System;
using System.Collections.Generic;
using System.Threading;

namespace App.Metrics.App_Packages.Concurrency
{
    /// <summary>
    ///     Array of longs which provides atomic operations on the array elements.
    /// </summary>
#if CONCURRENCY_UTILS_PUBLIC
public
#else
    internal
#endif
        struct AtomicLongArray
    {
        private readonly long[] array;

        public AtomicLongArray(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length must be positive", "length");
            }
            this.array = new long[length];
        }

        public AtomicLongArray(IReadOnlyList<long> source)
        {
            var clone = new long[source.Count];
            for (var i = 0; i < source.Count; i++)
            {
                clone[i] = source[i];
            }
            this.array = clone;
        }

        public int Length
        {
            get { return this.array.Length; }
        }

        /// <summary>
        ///     Returns the latest value of this instance written by any processor.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The latest written value of this instance.</returns>
        public long GetValue(int index)
        {
            return Volatile.Read(ref this.array[index]);
        }

        /// <summary>
        ///     Write a new value to this instance. The value is immediately seen by all processors.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The new value for this instance.</param>
        public void SetValue(int index, long value)
        {
            Volatile.Write(ref this.array[index], value);
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the resulting value.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance + the amount added.</returns>
        public long Add(int index, long value)
        {
            return Interlocked.Add(ref this.array[index], value);
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the value this instance had before the add operation.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance before the amount was added.</returns>
        public long GetAndAdd(int index, long value)
        {
            return Add(index, value) - value;
        }

        /// <summary>
        ///     Increment this instance and return the value the instance had before the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *before* the increment.</returns>
        public long GetAndIncrement(int index)
        {
            return Increment(index) - 1;
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value the instance had before the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to increment with</param>
        /// <returns>The value of the instance *before* the increment.</returns>
        public long GetAndIncrement(int index, long value)
        {
            return Increment(index, value) - value;
        }

        /// <summary>
        ///     Decrement this instance and return the value the instance had before the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public long GetAndDecrement(int index)
        {
            return Decrement(index) + 1;
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value the instance had before the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to decrement with</param>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public long GetAndDecrement(int index, long value)
        {
            return Decrement(index, value) + value;
        }

        /// <summary>
        ///     Increment this instance and return the value after the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *after* the increment.</returns>
        public long Increment(int index)
        {
            return Interlocked.Increment(ref this.array[index]);
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value after the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to increment with</param>
        /// <returns>The value of the instance *after* the increment.</returns>
        public long Increment(int index, long value)
        {
            return Add(index, value);
        }

        /// <summary>
        ///     Decrement this instance and return the value after the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public long Decrement(int index)
        {
            return Interlocked.Decrement(ref this.array[index]);
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value after the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to decrement with</param>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public long Decrement(int index, long value)
        {
            return Add(index, -value);
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to zero as an atomic operation.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The current value of the instance.</returns>
        public long GetAndReset(int index)
        {
            return GetAndSet(index, 0L);
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to <paramref name="newValue" /> as an atomic operation.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="newValue">value that will be set in the array at <paramref name="index" /></param>
        /// <returns>The current value of the instance.</returns>
        public long GetAndSet(int index, long newValue)
        {
            return Interlocked.Exchange(ref this.array[index], newValue);
        }

        /// <summary>
        ///     Replace the value of this instance, if the current value is equal to the <paramref name="expected" /> value.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="expected">Value this instance is expected to be equal with.</param>
        /// <param name="updated">Value to set this instance to, if the current value is equal to the expected value</param>
        /// <returns>True if the update was made, false otherwise.</returns>
        public bool CompareAndSwap(int index, long expected, long updated)
        {
            return Interlocked.CompareExchange(ref this.array[index], updated, expected) == expected;
        }
    }
}