// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

//Written by Iulian Margarintescu and will retain the same license as the Java Version
//Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/VolatileDouble.cs
//Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained

using System.Threading;
using App.Metrics.Concurrency.Internal;

namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     Double value on which the GetValue/SetValue operations are performed using Volatile.Read/Volatile.Write.
    /// </summary>
    /// <remarks>
    ///     This datastructure is a struct. If a member is declared readonly VolatileDouble calling set will *NOT* modify the value.
    ///     GetValue/SetValue are expressed as methods to make it obvious that a non-trivial operation is performed.
    /// </remarks>
    public struct VolatileDouble : IVolatileValue<double>
    {
        /// <summary>
        ///     The size in bytes occupied by an instance of this type
        /// </summary>
        public static readonly int SizeInBytes = 8;

        private double _value;

        /// <summary>
        ///     Initialize the value of this instance
        /// </summary>
        /// <param name="value">Initial value of the instance.</param>
        public VolatileDouble(double value)
        {
            _value = value;
        }

        /// <summary>
        ///     Get the current value of this instance
        /// </summary>
        /// <returns>The current value of the instance</returns>
        public double GetValue()
        {
            return Volatile.Read(ref _value);
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
        /// <param name="value">The new value for this instance.</param>
        public void LazySetValue(double value)
        {
            Volatile.Write(ref _value, value);
        }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public double NonVolatileGetValue()
        {
            return _value;
        }

        /// <summary>
        ///     Set the value without using Volatile.Write fence and ordering.
        /// </summary>
        /// <param name="value">The new value for this instance.</param>
        public void NonVolatileSetValue(double value)
        {
            _value = value;
        }

        /// <summary>
        ///     Set the the value of this instance to <paramref name="newValue" />
        /// </summary>
        /// <remarks>
        ///     Don't call Set on readonly fields.
        /// </remarks>
        /// <param name="newValue">New value for this instance</param>
        public void SetValue(double newValue)
        {
            Volatile.Write(ref _value, newValue);
        }
    }
}