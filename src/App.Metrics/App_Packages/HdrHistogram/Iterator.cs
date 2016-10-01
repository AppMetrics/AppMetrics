// Written by Gil Tene of Azul Systems, and released to the public domain,
// as explained at http://creativecommons.org/publicdomain/zero/1.0/
// 
// Ported to .NET by Iulian Margarintescu under the same license and terms as the java version
// Java Version repo: https://github.com/HdrHistogram/HdrHistogram
// Latest ported version is available in the Java submodule in the root of the repo

// Ported to.NET Standard Library by Allan Hardy

using System.Collections;
using System.Collections.Generic;

namespace App.Metrics.App_Packages.HdrHistogram
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