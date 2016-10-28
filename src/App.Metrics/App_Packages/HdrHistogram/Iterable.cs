// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Ported to.NET Standard Library by Allan Hardy

using System.Collections;
using System.Collections.Generic;

namespace App.Metrics.App_Packages.HdrHistogram
{
    internal abstract class Iterable<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            return iterator();
        }

        /**
         * Returns an iterator over a set of elements of type T.
         *
         * @return an Iterator.
         */

        protected abstract Iterator<T> iterator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}