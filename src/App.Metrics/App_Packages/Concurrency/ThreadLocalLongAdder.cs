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


using System.Threading;

namespace App.Metrics.App_Packages.Concurrency
{
    /// <summary>
    ///     This class is similar in functionality with the StripedLongAdder, but uses the ThreadLocal class to
    ///     keep a value for each thread. The GetValue method sums all the values and returns the result.
    ///     This class is a bit baster (in micro-benchmarks) than the StripedLongAdder for incrementing a value on multiple
    ///     threads,
    ///     but it creates a ValueHolder instance for each thread that increments the value, not just for when contention is
    ///     present.
    ///     Considering this, the StripedLongAdder might be a better solution in some cases (multiple threads, relatively low
    ///     contention).
    /// </summary>
#if CONCURRENCY_UTILS_PUBLIC
public
#else
    internal
#endif
        sealed class ThreadLocalLongAdder
    {
        private sealed class ValueHolder
        {
            public long Value;

            public long GetAndReset()
            {
                return Interlocked.Exchange(ref this.Value, 0L);
            }
        }

        /// <summary>
        ///     We store a ValueHolder instance for each thread that requires one.
        /// </summary>
        private readonly ThreadLocal<ValueHolder> local = new ThreadLocal<ValueHolder>(() => new ValueHolder(), true);

        /// <summary>
        ///     Creates a new instance of the adder with initial value of zero.
        /// </summary>
        public ThreadLocalLongAdder()
        {
        }

        /// <summary>
        ///     Creates a new instance of the adder with initial <paramref name="value" />.
        /// </summary>
        public ThreadLocalLongAdder(long value)
        {
            this.local.Value.Value = value;
        }

        /// <summary>
        ///     Returns the current value of this adder. This method sums all the thread local variables and returns the result.
        /// </summary>
        /// <returns>The current value recored by this adder.</returns>
        public long GetValue()
        {
            long sum = 0;
            foreach (var value in this.local.Values)
            {
                sum += Volatile.Read(ref value.Value);
            }
            return sum;
        }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public long NonVolatileGetValue()
        {
            long sum = 0;
            foreach (var value in this.local.Values)
            {
                sum += value.Value;
            }
            return sum;
        }

        /// <summary>
        ///     Returns the current value of this adder and resets the value to zero.
        ///     This method sums all the thread local variables, resets their value and returns the result.
        /// </summary>
        /// <remarks>
        ///     This method is thread-safe. If updates happen during this method, they are either included in the final sum, or
        ///     reflected in the value after the reset.
        /// </remarks>
        /// <returns>The current value recored by this adder.</returns>
        public long GetAndReset()
        {
            long sum = 0;
            foreach (var val in this.local.Values)
            {
                sum += val.GetAndReset();
            }
            return sum;
        }

        /// <summary>
        ///     Resets the current value to zero.
        /// </summary>
        public void Reset()
        {
            foreach (var value in this.local.Values)
            {
                Volatile.Write(ref value.Value, 0L);
            }
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(long value)
        {
            this.local.Value.Value += value;
        }

        /// <summary>
        ///     Increment the value of this instance.
        /// </summary>
        public void Increment()
        {
            this.local.Value.Value++;
        }

        /// <summary>
        ///     Decrement the value of this instance.
        /// </summary>
        public void Decrement()
        {
            this.local.Value.Value--;
        }

        /// <summary>
        ///     Increment the value of this instance with <paramref name="value" />.
        /// </summary>
        public void Increment(long value)
        {
            Add(value);
        }

        /// <summary>
        ///     Decrement the value of this instance with <paramref name="value" />.
        /// </summary>
        public void Decrement(long value)
        {
            Add(-value);
        }
    }
}