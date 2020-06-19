// <copyright file="AtomicDouble.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using App.Metrics.Concurrency.Internal;

namespace App.Metrics.Concurrency
{
    public struct AtomicDouble : IAtomicValue<double>, IValueAdder<double>
    {
        private double _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicDouble" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public AtomicDouble(double value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public double Add(double value)
        {
            double initialValue;
            double computedValue;

            do
            {
                initialValue = _value;
                computedValue = initialValue + value;
            }
            while (Math.Abs(initialValue - Interlocked.CompareExchange(ref _value, computedValue, initialValue)) > double.Epsilon);

            return computedValue;
        }

        /// <inheritdoc />
        public bool CompareAndSwap(double expected, double updated)
        {
            return Math.Abs(Interlocked.CompareExchange(ref _value, updated, expected) - expected) < double.Epsilon;
        }

        /// <inheritdoc />
        public double Decrement()
        {
            return Add(-1);
        }

        /// <inheritdoc />
        public double Decrement(double value)
        {
            return Add(-value);
        }

        /// <inheritdoc />
        public double GetAndAdd(double value)
        {
            return Add(value) - value;
        }

        /// <inheritdoc />
        public double GetAndDecrement()
        {
            return Decrement() + 1;
        }

        /// <inheritdoc />
        public double GetAndDecrement(double value)
        {
            return Decrement(value) + value;
        }

        /// <inheritdoc />
        public double GetAndIncrement()
        {
            return Increment() - 1;
        }

        /// <inheritdoc />
        public double GetAndIncrement(double value)
        {
            return Increment(value) - value;
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to zero as an atomic operation.
        /// </summary>
        /// <returns>
        ///     The current value of the instance.
        /// </returns>
        public double GetAndReset()
        {
            return GetAndSet(0.0);
        }

        /// <inheritdoc />
        public double GetAndSet(double newValue)
        {
            return Interlocked.Exchange(ref _value, newValue);
        }

        /// <summary>
        ///     Returns the latest value of this instance written by any processor.
        /// </summary>
        /// <returns>The latest written value of this instance.</returns>
        public double GetValue()
        {
            return Volatile.Read(ref _value);
        }

        /// <inheritdoc />
        public double Increment(double value)
        {
            return Add(value);
        }

        /// <summary>
        ///     Increment this instance and return the value after the increment.
        /// </summary>
        /// <returns>
        ///     The value of the instance *after* the increment.
        /// </returns>
        public double Increment()
        {
            return Add(1.0);
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value after the increment.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The value of the instance *after* the increment.
        /// </returns>
        public double Increment(long value)
        {
            return Add(value);
        }

        /// <inheritdoc />
        public double NonVolatileGetValue()
        {
            return _value;
        }

        /// <inheritdoc />
        public void NonVolatileSetValue(double value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public void SetValue(double newValue)
        {
            Volatile.Write(ref _value, newValue);
        }

        void IValueAdder<double>.Add(double value) { Add(value); }

        void IValueAdder<double>.Decrement() { Decrement(); }

        void IValueAdder<double>.Decrement(double value) { Decrement(value); }

        // RemoveAtPack
        double IValueAdder<double>.GetAndReset() { return GetAndReset(); }

        double IValueReader<double>.GetValue() { return GetValue(); }

        void IValueAdder<double>.Increment() { Increment(); }

        void IValueAdder<double>.Increment(double value) { Increment(value); }

        void IValueAdder<double>.Reset() { SetValue(0L); }

        // EndRemoveAtPack
    }
}