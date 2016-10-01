// Written by Gil Tene of Azul Systems, and released to the public domain,
// as explained at http://creativecommons.org/publicdomain/zero/1.0/
// 
// Ported to .NET by Iulian Margarintescu under the same license and terms as the java version
// Java Version repo: https://github.com/HdrHistogram/HdrHistogram
// Latest ported version is available in the Java submodule in the root of the repo

using System;
using App.Metrics.App_Packages.Concurrency;

namespace App.Metrics.App_Packages.HdrHistogram
{
    /// <summary>
    /// This non-public AbstractHistogramBase super-class separation is meant to bunch "cold" fields
    /// separately from "hot" fields, in an attempt to force the JVM to place the (hot) fields
    /// commonly used in the value recording code paths close together.
    /// Subclass boundaries tend to be strongly control memory layout decisions in most practical
    /// JVM implementations, making this an effective method for control filed grouping layout.
    /// </summary>
    internal abstract class AbstractHistogramBase
    {
        private static AtomicLong constructionIdentityCount = new AtomicLong(0);

        protected AbstractHistogramBase(long lowestDiscernibleValue, int numberOfSignificantValueDigits, int wordSizeInBytes, bool autoResize)
        {
            // Verify argument validity
            if (lowestDiscernibleValue < 1)
            {
                throw new ArgumentException("lowestDiscernibleValue must be >= 1");
            }

            if ((numberOfSignificantValueDigits < 0) || (numberOfSignificantValueDigits > 5))
            {
                throw new ArgumentException("numberOfSignificantValueDigits must be between 0 and 5");
            }

            this.LowestDiscernibleValue = lowestDiscernibleValue;
            this.Identity = constructionIdentityCount.GetAndIncrement();
            this.NumberOfSignificantValueDigits = numberOfSignificantValueDigits;
            this.WordSizeInBytes = wordSizeInBytes;
            this.AutoResize = autoResize;

            this.recordedValuesIterator = new RecordedValuesIterator(this as AbstractHistogram);
        }

        // "Cold" accessed fields. Not used in the recording code path:
        internal protected readonly long Identity;
        internal protected readonly int NumberOfSignificantValueDigits;

        protected readonly bool AutoResize;
        protected readonly int WordSizeInBytes;

        protected readonly long LowestDiscernibleValue;
        internal protected long HighestTrackableValue;


        internal protected int bucketCount;
        internal protected int subBucketCount;
        internal int countsArrayLength;


        internal protected long startTimeStampMsec = long.MaxValue;
        internal protected long endTimeStampMsec = 0;

        internal protected double integerToDoubleValueConversionRatio = 1.0;

        protected readonly RecordedValuesIterator recordedValuesIterator;
    }
}