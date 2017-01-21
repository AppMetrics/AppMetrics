// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Concurrency;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
#pragma warning disable

    // ReSharper disable ArrangeModifiersOrder
    // ReSharper disable ArrangeThisQualifier
    // ReSharper disable InconsistentNaming

    /// <summary>
    ///     This non-public AbstractHistogramBase super-class separation is meant to bunch "cold" fields
    ///     separately from "hot" fields, in an attempt to force the JVM to place the (hot) fields
    ///     commonly used in the value recording code paths close together.
    ///     Subclass boundaries tend to be strongly control memory layout decisions in most practical
    ///     JVM implementations, making this an effective method for control filed grouping layout.
    /// </summary>
    public abstract class AbstractHistogramBase
    {
        internal int countsArrayLength;

        // "Cold" accessed fields. Not used in the recording code path:
        internal protected readonly long Identity;
        internal protected readonly int NumberOfSignificantValueDigits;


        internal protected int bucketCount;
        internal protected long endTimeStampMsec = 0;
        internal protected long HighestTrackableValue;

        internal protected double integerToDoubleValueConversionRatio = 1.0;


        internal protected long startTimeStampMsec = long.MaxValue;
        internal protected int subBucketCount;

        protected readonly bool AutoResize;

        protected readonly long LowestDiscernibleValue;

        protected readonly RecordedValuesIterator recordedValuesIterator;
        protected readonly int WordSizeInBytes;
        private static AtomicLong _constructionIdentityCount = new AtomicLong(0);

        protected AbstractHistogramBase(long lowestDiscernibleValue, int numberOfSignificantValueDigits, int wordSizeInBytes, bool autoResize)
        {
            // Verify argument validity
            if (lowestDiscernibleValue < 1)
            {
                throw new ArgumentException("lowestDiscernibleValue must be >= 1");
            }

            if (numberOfSignificantValueDigits < 0 || numberOfSignificantValueDigits > 5)
            {
                throw new ArgumentException("numberOfSignificantValueDigits must be between 0 and 5");
            }

            LowestDiscernibleValue = lowestDiscernibleValue;
            Identity = _constructionIdentityCount.GetAndIncrement();
            NumberOfSignificantValueDigits = numberOfSignificantValueDigits;
            WordSizeInBytes = wordSizeInBytes;
            AutoResize = autoResize;

            recordedValuesIterator = new RecordedValuesIterator(this as AbstractHistogram);
        }
    }

#pragma warning restore
}

// ReSharper restore ArrangeModifiersOrder
// ReSharper restore ArrangeThisQualifier
// ReSharper restore InconsistentNaming