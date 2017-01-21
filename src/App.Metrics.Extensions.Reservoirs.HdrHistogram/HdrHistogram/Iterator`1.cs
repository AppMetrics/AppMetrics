// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
#pragma warning disable
    // ReSharper disable InconsistentNaming
    public abstract class Iterator<T> : IEnumerator<T>
    {
        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        /**
         * Returns {@code true} if the iteration has more elements.
         * (In other words, returns {@code true} if {@link #next} would
         * return an element rather than throwing an exception.)
         *
         * @return {@code true} if the iteration has more elements
         */

        // ReSharper disable InconsistentNaming
        public abstract bool hasNext();

        // ReSharper restore InconsistentNaming

        /**
         * Returns the next element in the iteration.
         *
         * @return the next element in the iteration
         * @throws NoSuchElementException if the iteration has no more elements
         */

        // ReSharper disable InconsistentNaming
        public abstract T next();

        // ReSharper restore InconsistentNaming
        public void Dispose() { }

        public bool MoveNext()
        {
            if (hasNext())
            {
                Current = next();
                return true;
            }

            return false;
        }

        public void Reset() { }

        // ReSharper restore InconsistentNaming
    }
#pragma warning restore
}