// Written by Gil Tene of Azul Systems, and released to the public domain,
// as explained at http://creativecommons.org/publicdomain/zero/1.0/
// 
// Ported to .NET by Iulian Margarintescu under the same license and terms as the java version
// Java Version repo: https://github.com/HdrHistogram/HdrHistogram
// Latest ported version is available in the Java submodule in the root of the repo

using System.Collections.Generic;

namespace App.Metrics.App_Packages.HdrHistogram
{
    internal abstract class Iterator<E> : IEnumerator<E>
    {
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

        public E Current { get; private set; }
        public void Dispose() { }
        object System.Collections.IEnumerator.Current { get { return this.Current; } }
        public bool MoveNext()
        {
            if (hasNext())
            {
                this.Current = this.next();
                return true;
            }
            return false;
        }
        public void Reset() { }
    }
}
