// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
    internal static class AbstractHistogramEnumerableExtensions
    {
        /// <summary>
        ///     Provide a means of iterating through all recorded histogram values using the finest granularity steps
        ///     supported by the underlying representation. The iteration steps through all non-zero recorded value counts,
        ///     and terminates when all recorded histogram values are exhausted.
        ///     <seealso cref="RecordedValuesIterator" />
        /// </summary>
        /// <param name="histogram">The histogram on which to iterate.</param>
        /// <returns>Enumerable histogram iteration values</returns>
        public static IEnumerable<HistogramIterationValue> RecordedValues(this AbstractHistogram histogram)
        {
            return IterateOver(new RecordedValuesIterator(histogram));
        }

        private static IEnumerable<HistogramIterationValue> IterateOver(AbstractHistogramIterator iterator)
        {
            using (iterator)
            {
                while (iterator.MoveNext())
                {
                    yield return iterator.Current;
                }
            }
        }
    }
}