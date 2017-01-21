// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
    // Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
    // Ported/Refactored to .NET Standard Library by Allan Hardy
#pragma warning disable

    // ReSharper disable InconsistentNaming

    /**
 * Written by Gil Tene of Azul Systems, and released to the public domain,
 * as explained at http://creativecommons.org/publicdomain/zero/1.0/
 *
 * @author Gil Tene
 */


    /**
     * Used for iterating through all recorded histogram values using the finest granularity steps supported by the
     * underlying representation. The iteration steps through all non-zero recorded value counts, and terminates when
     * all recorded histogram values are exhausted.
     */

    public class RecordedValuesIterator : AbstractHistogramIterator
    {
        int visitedIndex;

        /**
         * @param histogram The histogram this iterator will operate on
         */

        public RecordedValuesIterator(AbstractHistogram histogram) { reset(histogram); }

        /**
         * Reset iterator for re-use in a fresh iteration over the same histogram data set.
         */

        public void reset() { reset(Histogram); }


        protected override void IncrementIterationLevel() { visitedIndex = CurrentIndex; }


        protected override bool ReachedIterationLevel()
        {
            var currentCount = Histogram.getCountAtIndex(CurrentIndex);
            return currentCount != 0 && visitedIndex != CurrentIndex;
        }

        private void reset(AbstractHistogram histogram)
        {
            ResetIterator(histogram);
            visitedIndex = -1;
        }
    }
#pragma warning restore
}

// ReSharper restore InconsistentNaming