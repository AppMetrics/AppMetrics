// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Ported to.NET Standard Library by Allan Hardy

using System.Collections;
using System.Collections.Generic;

namespace App.Metrics.Sampling.HdrHistogram
{
    internal abstract class Iterator<E> : IEnumerator<E>
    {
        public E Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        /**
         * Returns {@code true} if the iteration has more elements.
         * (In other words, returns {@code true} if {@link #next} would
         * return an element rather than throwing an exception.)
         *
         * @return {@code true} if the iteration has more elements
         */

        public abstract bool hasNext();

        /**
         * Returns the next element in the iteration.
         *
         * @return the next element in the iteration
         * @throws NoSuchElementException if the iteration has no more elements
         */

        public abstract E next();

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