// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Concurrency.Internal
{
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

        void LazySetValue(int index, T value);

        T NonVolatileGetValue(int index);

        void NonVolatileSetValue(int index, T value);

        void SetValue(int index, T value);
    }
}