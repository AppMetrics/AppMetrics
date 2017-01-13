// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System.Collections;
using System.Collections.Generic;

namespace App.Metrics.Sampling.HdrHistogram
{
#pragma warning disable

    // ReSharper disable InconsistentNaming
    internal abstract class Iterable<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() { return iterator(); }

        protected abstract Iterator<T> iterator();

        /**
         * Returns an iterator over a set of elements of type T.
         *
         * @return an Iterator.
         */

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }

    // ReSharper restore InconsistentNaming
#pragma warning restore
}