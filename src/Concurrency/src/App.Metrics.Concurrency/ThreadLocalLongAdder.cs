// <copyright file="ThreadLocalLongAdder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using App.Metrics.Concurrency.Internal;

// Written by Iulian Margarintescu and will retain the same license as the Java Version
// Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/ThreadLocalLongAdder.cs
// Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     This class is similar in functionality with the StripedLongAdder, but uses the ThreadLocal class to
    ///     keep a value for each thread. The GetValue method sums all the values and returns the result.
    ///     This class is a bit faster (in micro-benchmarks) than the StripedLongAdder for incrementing a value on multiple
    ///     threads, but it creates a ValueHolder instance for each thread that increments the value, not just for when
    ///     contention is
    ///     present. Considering this, the StripedLongAdder might be a better solution in some cases (multiple threads,
    ///     relatively low
    ///     contention).
    /// </summary>
    public sealed class ThreadLocalLongAdder : IValueAdder<long>
    {
        /// <summary>
        ///     We store a ValueHolder instance for each thread that requires one.
        /// </summary>
        private readonly ThreadLocal<ValueHolder> _local = new ThreadLocal<ValueHolder>(() => new ValueHolder(), true);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThreadLocalLongAdder" /> class.
        /// </summary>
        /// <remarks>
        ///     Creates a new instance of the adder with initial value of zero.
        /// </remarks>
        public ThreadLocalLongAdder() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThreadLocalLongAdder" /> class.
        /// </summary>
        /// <param name="value">The initial value of the instance.</param>
        /// <remarks>
        ///     Creates a new instance of the adder with initial <paramref name="value" />.
        /// </remarks>
        public ThreadLocalLongAdder(long value) { _local.Value.Value = value; }

        /// <summary>
        ///     Returns the size in bytes occupied by an ThreadLocalAddre instance.
        /// </summary>
        /// <param name="instance">instance for whch to calculate the size.</param>
        /// <returns>The size of the instance in bytes.</returns>
        public static int GetEstimatedFootprintInBytes(ThreadLocalLongAdder instance)
        {
            // ReSharper disable ArrangeRedundantParentheses SA1407
            return (instance._local.Values.Count * (IntPtr.Size + ValueHolder.SizeInBytes)) + 64;
            // very rough estimation for thread local & values list
            // ReSharper restore ArrangeRedundantParentheses
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(long value) { _local.Value.Value += value; }

        /// <summary>
        ///     Decrement the value of this instance.
        /// </summary>
        public void Decrement() { _local.Value.Value--; }

        /// <summary>
        ///     Decrement the value of this instance with <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Decrement(long value) { Add(-value); }

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

            foreach (var val in _local.Values)
            {
                sum += val.GetAndReset();
            }

            return sum;
        }

        /// <summary>
        ///     Returns the current value of this adder. This method sums all the thread local variables and returns the result.
        /// </summary>
        /// <returns>The current value recored by this adder.</returns>
        public long GetValue()
        {
            long sum = 0;

            foreach (var value in _local.Values)
            {
                sum += Volatile.Read(ref value.Value);
            }

            return sum;
        }

        /// <summary>
        ///     Increment the value of this instance.
        /// </summary>
        public void Increment() { _local.Value.Value++; }

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
            long sum = 0;

            foreach (var value in _local.Values)
            {
                sum += value.Value;
            }

            return sum;
        }

        /// <summary>
        ///     Resets the current value to zero.
        /// </summary>
        public void Reset()
        {
            foreach (var value in _local.Values)
            {
                Volatile.Write(ref value.Value, 0L);
            }
        }

        private sealed class ValueHolder
        {
            public const int SizeInBytes = sizeof(long) + 16;

#pragma warning disable SA1401
            public long Value;
#pragma warning restore SA1401

            public long GetAndReset() { return Interlocked.Exchange(ref Value, 0L); }
        }
    }
}