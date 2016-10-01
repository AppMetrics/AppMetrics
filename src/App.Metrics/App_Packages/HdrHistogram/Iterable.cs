// Written by Gil Tene of Azul Systems, and released to the public domain,
// as explained at http://creativecommons.org/publicdomain/zero/1.0/
// 
// Ported to .NET by Iulian Margarintescu under the same license and terms as the java version
// Java Version repo: https://github.com/HdrHistogram/HdrHistogram
// Latest ported version is available in the Java submodule in the root of the repo

using System.Collections.Generic;

namespace App.Metrics.App_Packages.HdrHistogram
{
    internal abstract class Iterable<T> : IEnumerable<T>
    {

        /**
         * Returns an iterator over a set of elements of type T.
         *
         * @return an Iterator.
         */
        protected abstract Iterator<T> iterator();

        public IEnumerator<T> GetEnumerator()
        {
            return this.iterator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
