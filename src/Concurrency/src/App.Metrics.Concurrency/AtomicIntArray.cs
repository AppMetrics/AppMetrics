// <copyright file="AtomicIntArray.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using App.Metrics.Concurrency.Internal;

// Written by Iulian Margarintescu and will retain the same license as the Java Version
// Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/AtomicIntArray.cs
// Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     Array of ints which provides atomic operations on the array elements.
    /// </summary>
    public struct AtomicIntArray : IAtomicArray<int>
    {
        private readonly int[] _array;

        public AtomicIntArray(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length must be positive", nameof(length));
            }

            _array = new int[length];
        }

        public AtomicIntArray(IReadOnlyList<int> source)
        {
            var clone = new int[source.Count];
            for (var i = 0; i < source.Count; i++)
            {
                clone[i] = source[i];
            }

            _array = clone;
        }

        /// <summary>
        ///     Gets the length of the underlying array
        /// </summary>
        /// <value>
        ///     The length.
        /// </value>
        public int Length => _array.Length;

        /// <summary>
        ///     Returns the size in bytes occupied by an AtomicIntArray instance.
        /// </summary>
        /// <param name="instance">instance for whch to calculate the size.</param>
        /// <returns>The size of the instance in bytes.</returns>
        public static int GetEstimatedFootprintInBytes(AtomicIntArray instance)
        {
            // ReSharper disable ArrangeRedundantParentheses SA1407
            return (instance.Length * sizeof(int)) + 16 + IntPtr.Size; // array reference & overhead
            // ReSharper restore ArrangeRedundantParentheses
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the resulting value.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance + the amount added.</returns>
        public int Add(int index, int value)
        {
            return Interlocked.Add(ref _array[index], value);
        }

        /// <summary>
        ///     Replace the value of this instance, if the current value is equal to the <paramref name="expected" /> value.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="expected">Value this instance is expected to be equal with.</param>
        /// <param name="updated">Value to set this instance to, if the current value is equal to the expected value</param>
        /// <returns>True if the update was made, false otherwise.</returns>
        public bool CompareAndSwap(int index, int expected, int updated)
        {
            return Interlocked.CompareExchange(ref _array[index], updated, expected) == expected;
        }

        /// <summary>
        ///     Decrement this instance and return the value after the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public int Decrement(int index)
        {
            return Interlocked.Decrement(ref _array[index]);
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value after the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to decrement with</param>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public int Decrement(int index, int value)
        {
            return Add(index, -value);
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the value this instance had before the add operation.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance before the amount was added.</returns>
        public int GetAndAdd(int index, int value)
        {
            return Add(index, value) - value;
        }

        /// <summary>
        ///     Decrement this instance and return the value the instance had before the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public int GetAndDecrement(int index)
        {
            return Decrement(index) + 1;
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value the instance had before the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to decrement with</param>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public int GetAndDecrement(int index, int value)
        {
            return Decrement(index, value) + value;
        }

        /// <summary>
        ///     Increment this instance and return the value the instance had before the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *before* the increment.</returns>
        public int GetAndIncrement(int index)
        {
            return Increment(index) - 1;
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value the instance had before the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to increment with</param>
        /// <returns>The value of the instance *before* the increment.</returns>
        public int GetAndIncrement(int index, int value)
        {
            return Increment(index, value) - value;
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to zero as an atomic operation.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The current value of the instance.</returns>
        public int GetAndReset(int index)
        {
            return GetAndSet(index, 0);
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to <paramref name="newValue" /> as an atomic operation.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="newValue">value that will be set in the array at <paramref name="index" /></param>
        /// <returns>The current value of the instance.</returns>
        public int GetAndSet(int index, int newValue)
        {
            return Interlocked.Exchange(ref _array[index], newValue);
        }

        /// <summary>
        ///     Returns the latest value of this instance written by any processor.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The latest written value of this instance.</returns>
        public int GetValue(int index)
        {
            return Volatile.Read(ref _array[index]);
        }

        /// <summary>
        ///     Increment this instance and return the value after the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *after* the increment.</returns>
        public int Increment(int index)
        {
            return Interlocked.Increment(ref _array[index]);
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value after the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">value to increment with</param>
        /// <returns>The value of the instance *after* the increment.</returns>
        public int Increment(int index, int value)
        {
            return Add(index, value);
        }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public int NonVolatileGetValue(int index)
        {
            return _array[index];
        }

        /// <summary>
        ///     Set the value without using Volatile.Write fence and ordering.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The new value for this instance.</param>
        public void NonVolatileSetValue(int index, int value)
        {
            _array[index] = value;
        }

        /// <summary>
        ///     Write a new value to this instance. The value is immediately seen by all processors.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The new value for this instance.</param>
        public void SetValue(int index, int value)
        {
            Volatile.Write(ref _array[index], value);
        }
    }
}