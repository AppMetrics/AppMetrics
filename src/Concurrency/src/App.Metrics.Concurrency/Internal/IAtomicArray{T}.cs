// <copyright file="IAtomicArray{T}.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Concurrency.Internal
{
    // Written by Iulian Margarintescu and will retain the same license as the Java Version
    // Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/
    // Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
    internal interface IAtomicArray<T>
    {
        int Length { get; }

        T Add(int index, T value);

        bool CompareAndSwap(int index, T expected, T updated);

        T Decrement(int index);

        T Decrement(int index, T value);

        T GetAndAdd(int index, T value);

        T GetAndDecrement(int index);

        T GetAndDecrement(int index, T value);

        T GetAndIncrement(int index);

        T GetAndIncrement(int index, T value);

        T GetAndReset(int index);

        T GetAndSet(int index, T newValue);

        T GetValue(int index);

        T Increment(int index);

        T Increment(int index, T value);

        T NonVolatileGetValue(int index);

        void NonVolatileSetValue(int index, T value);

        void SetValue(int index, T value);
    }
}