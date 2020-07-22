// <copyright file="AtomicLong.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using App.Metrics.Concurrency.Internal;

// Written by Iulian Margarintescu and will retain the same license as the Java Version
// Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/AtomicLong.cs
// Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     Atomic long value. Operations exposed on this class are performed using System.Threading.Interlocked class and are
    ///     thread safe.
    ///     For AtomicLong values that are stored in arrays PaddedAtomicLong is recommended.
    /// </summary>
    /// <remarks>
    ///     The AtomicLong is a struct not a class and members of this type should *not* be declared readonly or changes will
    ///     not be reflected in the member instance.
    /// </remarks>
    public struct AtomicLong : IAtomicValue<long>, IValueAdder<long>
    {
        /// <summary>
        ///     The size in bytes occupied by an instance of this type
        /// </summary>
        public static readonly int SizeInBytes = 8;

        private long _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicLong" /> struct.
        /// </summary>
        /// <param name="value">Initial value of the instance.</param>
        public AtomicLong(long value) { _value = value; }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the resulting value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance + the amount added.</returns>
        public long Add(long value) { return Interlocked.Add(ref _value, value); }

        /// <summary>
        ///     Replace the value of this instance, if the current value is equal to the <paramref name="expected" /> value.
        /// </summary>
        /// <param name="expected">Value this instance is expected to be equal with.</param>
        /// <param name="updated">Value to set this instance to, if the current value is equal to the expected value</param>
        /// <returns>True if the update was made, false otherwise.</returns>
        public bool CompareAndSwap(long expected, long updated) { return Interlocked.CompareExchange(ref _value, updated, expected) == expected; }

        /// <summary>
        ///     Decrement this instance and return the value after the decrement.
        /// </summary>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public long Decrement() { return Interlocked.Decrement(ref _value); }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value after the decrement.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The value of the instance *after* the decrement.
        /// </returns>
        public long Decrement(long value) { return Add(-value); }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the value this instance had before the add operation.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance before the amount was added.</returns>
        public long GetAndAdd(long value) { return Add(value) - value; }

        /// <summary>
        ///     Decrement this instance and return the value the instance had before the decrement.
        /// </summary>
        /// <returns>
        ///     The value of the instance *before* the decrement.
        /// </returns>
        public long GetAndDecrement() { return Decrement() + 1; }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value the instance had before the decrement.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The value of the instance *before* the decrement.
        /// </returns>
        public long GetAndDecrement(long value) { return Decrement(value) + value; }

        /// <summary>
        ///     Increment this instance and return the value the instance had before the increment.
        /// </summary>
        /// <returns>The value of the instance *before* the increment.</returns>
        public long GetAndIncrement() { return Increment() - 1; }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value the instance had before the increment.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The value of the instance *before* the increment.
        /// </returns>
        public long GetAndIncrement(long value) { return Increment(value) - value; }

        /// <summary>
        ///     Returns the current value of the instance and sets it to zero as an atomic operation.
        /// </summary>
        /// <returns>
        ///     The current value of the instance.
        /// </returns>
        public long GetAndReset() { return GetAndSet(0L); }

        /// <summary>
        ///     Returns the current value of the instance and sets it to <paramref name="newValue" /> as an atomic operation.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        ///     The current value of the instance.
        /// </returns>
        public long GetAndSet(long newValue) { return Interlocked.Exchange(ref _value, newValue); }

        /// <summary>
        ///     Returns the latest value of this instance written by any processor.
        /// </summary>
        /// <returns>The latest written value of this instance.</returns>
        public long GetValue() { return Volatile.Read(ref _value); }

        /// <summary>
        ///     Increment this instance and return the value after the increment.
        /// </summary>
        /// <returns>
        ///     The value of the instance *after* the increment.
        /// </returns>
        public long Increment() { return Interlocked.Increment(ref _value); }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value after the increment.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The value of the instance *after* the increment.
        /// </returns>
        public long Increment(long value) { return Add(value); }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <returns>
        ///     The current value of the instance in a non-volatile way (might not observe changes on other threads).
        /// </returns>
        public long NonVolatileGetValue() { return _value; }

        /// <summary>
        ///     Set the value without using Volatile.Write fence and ordering.
        /// </summary>
        /// <param name="value">The new value for this instance.</param>
        public void NonVolatileSetValue(long value) { _value = value; }

        /// <summary>
        ///     Write a new value to this instance. The value is immediately seen by all processors.
        /// </summary>
        /// <param name="value">The new value for this instance.</param>
        public void SetValue(long value) { Volatile.Write(ref _value, value); }

        /// <summary>
        ///     Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void IValueAdder<long>.Add(long value) { Add(value); }

        void IValueAdder<long>.Decrement() { Decrement(); }

        void IValueAdder<long>.Decrement(long value) { Decrement(value); }

        // RemoveAtPack
        long IValueAdder<long>.GetAndReset() { return GetAndReset(); }

        long IValueReader<long>.GetValue() { return GetValue(); }

        void IValueAdder<long>.Increment() { Increment(); }

        void IValueAdder<long>.Increment(long value) { Increment(value); }

        void IValueAdder<long>.Reset() { SetValue(0L); }

        // EndRemoveAtPack
    }
}