using System.Runtime.InteropServices;
using System.Threading;
using App.Metrics.Concurrency.Internal;

namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     Padded version of the AtomicLong to avoid false CPU cache sharing. Recommended for cases where instances of
    ///     AtomicLong end up close to each other in memory - when stored in an array for ex.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 64 * 2)]
    public struct PaddedAtomicLong : IAtomicValue<long>, IValueAdder<long>
    {
        public static readonly int SizeInBytes = 128;

        [FieldOffset(64)] private long value;

        /// <summary>
        ///     Initializes a new instance with the specified <paramref name="value" />.
        /// </summary>
        /// <param name="value">Initial value of the instance.</param>
        public PaddedAtomicLong(long value)
        {
            this.value = value;
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the resulting value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance + the amount added.</returns>
        public long Add(long value)
        {
            return Interlocked.Add(ref this.value, value);
        }

        /// <summary>
        ///     Replace the value of this instance, if the current value is equal to the <paramref name="expected" /> value.
        /// </summary>
        /// <param name="expected">Value this instance is expected to be equal with.</param>
        /// <param name="updated">Value to set this instance to, if the current value is equal to the expected value</param>
        /// <returns>True if the update was made, false otherwise.</returns>
        public bool CompareAndSwap(long expected, long updated)
        {
            return Interlocked.CompareExchange(ref this.value, updated, expected) == expected;
        }

        /// <summary>
        ///     Decrement this instance and return the value after the decrement.
        /// </summary>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public long Decrement()
        {
            return Interlocked.Decrement(ref this.value);
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value after the decrement.
        /// </summary>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public long Decrement(long value)
        {
            return Add(-value);
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the value this instance had before the add operation.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance before the amount was added.</returns>
        public long GetAndAdd(long value)
        {
            return Add(value) - value;
        }

        /// <summary>
        ///     Decrement this instance and return the value the instance had before the decrement.
        /// </summary>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public long GetAndDecrement()
        {
            return Decrement() + 1;
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value the instance had before the decrement.
        /// </summary>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public long GetAndDecrement(long value)
        {
            return Decrement(value) + value;
        }

        /// <summary>
        ///     Increment this instance and return the value the instance had before the increment.
        /// </summary>
        /// <returns>The value of the instance *before* the increment.</returns>
        public long GetAndIncrement()
        {
            return Increment() - 1;
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value the instance had before the increment.
        /// </summary>
        /// <returns>The value of the instance *before* the increment.</returns>
        public long GetAndIncrement(long value)
        {
            return Increment(value) - value;
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to zero as an atomic operation.
        /// </summary>
        /// <returns>The current value of the instance.</returns>
        public long GetAndReset()
        {
            return GetAndSet(0L);
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to <paramref name="newValue" /> as an atomic operation.
        /// </summary>
        /// <returns>The current value of the instance.</returns>
        public long GetAndSet(long newValue)
        {
            return Interlocked.Exchange(ref this.value, newValue);
        }

        /// <summary>
        ///     Returns the latest value of this instance written by any processor.
        /// </summary>
        /// <returns>The latest written value of this instance.</returns>
        public long GetValue()
        {
            return Volatile.Read(ref this.value);
        }

        /// <summary>
        ///     Increment this instance and return the value after the increment.
        /// </summary>
        /// <returns>The value of the instance *after* the increment.</returns>
        public long Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value after the increment.
        /// </summary>
        /// <returns>The value of the instance *after* the increment.</returns>
        public long Increment(long value)
        {
            return Add(value);
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
        ///     Not sure if it is possible on CLR to implement this.
        /// </remarks>
        /// <param name="value">The new value for this instance.</param>
        public void LazySetValue(long value)
        {
            Volatile.Write(ref this.value, value);
        }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public long NonVolatileGetValue()
        {
            return this.value;
        }

        /// <summary>
        ///     Set the value without using Volatile.Write fence and ordering.
        /// </summary>
        /// <param name="value">The new value for this instance.</param>
        public void NonVolatileSetValue(long value)
        {
            this.value = value;
        }

        /// <summary>
        ///     Write a new value to this instance. The value is immediately seen by all processors.
        /// </summary>
        /// <param name="value">The new value for this instance.</param>
        public void SetValue(long value)
        {
            Volatile.Write(ref this.value, value);
        }

        void IValueAdder<long>.Add(long value)
        {
            this.Add(value);
        }

        void IValueAdder<long>.Decrement()
        {
            this.Decrement();
        }

        void IValueAdder<long>.Decrement(long value)
        {
            this.Decrement(value);
        }

        // RemoveAtPack
        long IValueAdder<long>.GetAndReset()
        {
            return this.GetAndReset();
        }

        long IValueReader<long>.GetValue()
        {
            return this.GetValue();
        }

        void IValueAdder<long>.Increment()
        {
            this.Increment();
        }

        void IValueAdder<long>.Increment(long value)
        {
            this.Increment(value);
        }

        void IValueAdder<long>.Reset()
        {
            this.SetValue(0L);
        }

        // EndRemoveAtPack
    }
}