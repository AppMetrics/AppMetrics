// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Ported to.NET Standard Library by Allan Hardy

using System.Collections;
using System.Collections.Generic;

namespace App.Metrics.Sampling.HdrHistogram
{
    // ReSharper disable InconsistentNaming
    internal abstract class Iterator<E> : IEnumerator<E>
        // ReSharper restore InconsistentNaming
    {
        public E Current { get; private set; }

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
        public abstract E next();
        // ReSharper restore InconsistentNaming

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (hasNext())
            {
                Current = next();
                return true;
            }
            return false;
        }

        public void Reset()
        {
        }
    }
}