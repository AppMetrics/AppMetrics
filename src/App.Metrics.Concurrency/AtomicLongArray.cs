using System;
using System.Collections.Generic;
using System.Threading;
using App.Metrics.Concurrency.Internal;

namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     Array of longs which provides atomic operations on the array elements.
    /// </summary>
    public struct AtomicLongArray : IAtomicArray<long>
    {
        private readonly long[] _array;

        public AtomicLongArray(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length must be positive", "length");
            }
            _array = new long[length];
        }

        public AtomicLongArray(IReadOnlyList<long> source)
        {
            var clone = new long[source.Count];
            for (var i = 0; i < source.Count; i++)
            {
                clone[i] = source[i];
            }
            _array = clone;
        }

        /// <summary>
        ///     The length of the underlying array
        /// </summary>
        public int Length
        {
            get { return _array.Length; }
        }

        /// <summary>
        ///     Returns the size in bytes occupied by an AtomicLongArray instance.
        /// </summary>
        /// <param name="instance">instance for whch to calculate the size.</param>
        /// <returns>The size of the instance in bytes.</returns>
        public static int GetEstimatedFootprintInBytes(AtomicIntArray instance)
        {
            return instance.Length * sizeof(long) + 16 + IntPtr.Size; // array reference & overhead
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the resulting value.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance + the amount added.</returns>
        public long Add(int index, long value)
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
        public bool CompareAndSwap(int index, long expected, long updated)
        {
            return Interlocked.CompareExchange(ref _array[index], updated, expected) == expected;
        }

        /// <summary>
        ///     Decrement this instance and return the value after the decrement.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public long Decrement(int index)
        {
            return Interlocked.Decrement(ref _array[index]);
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
            return Interlocked.Exchange(ref _array[index], newValue);
        }

        /// <summary>
        ///     Returns the latest value of this instance written by any processor.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The latest written value of this instance.</returns>
        public long GetValue(int index)
        {
            return Volatile.Read(ref _array[index]);
        }

        /// <summary>
        ///     Increment this instance and return the value after the increment.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The value of the instance *after* the increment.</returns>
        public long Increment(int index)
        {
            return Interlocked.Increment(ref _array[index]);
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
        ///     From the Java Version:
        ///     Eventually sets to the given value.
        ///     The semantics are that the write is guaranteed not to be re-ordered with any previous write,
        ///     but may be reordered with subsequent operations (or equivalently, might not be visible to other threads)
        ///     until some other volatile write or synchronizing action occurs).
        /// </summary>
        /// <remarks>
        ///     Currently implemented by calling Volatile.Write which is different from the java version.
        ///     Not sure if it is possible on CLR to implement 
        /// </remarks>
        /// <param name="index">index in the array</param>
        /// <param name="value">The new value for this instance.</param>
        public void LazySetValue(int index, long value)
        {
            Volatile.Write(ref _array[index], value);
        }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public long NonVolatileGetValue(int index)
        {
            return _array[index];
        }

        /// <summary>
        ///     Set the value without using Volatile.Write fence and ordering.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The new value for this instance.</param>
        public void NonVolatileSetValue(int index, long value)
        {
            _array[index] = value;
        }

        /// <summary>
        ///     Write a new value to this instance. The value is immediately seen by all processors.
        /// </summary>
        /// <param name="index">index in the array</param>
        /// <param name="value">The new value for this instance.</param>
        public void SetValue(int index, long value)
        {
            Volatile.Write(ref _array[index], value);
        }
    }
}