// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using App.Metrics.Concurrency;

#pragma warning disable 659

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
    // GetHashCode does not make sense for a histogram, even if Equals is implemented
#pragma warning disable

    // ReSharper disable ArrangeModifiersOrder
    // ReSharper disable ArrangeThisQualifier
    // ReSharper disable InconsistentNaming
    // ReSharper disable LocalVariableHidesMember

    /// <summary>
    ///     <h3>An abstract base class for integer values High Dynamic Range (HDR) Histograms</h3>
    ///     AbstractHistogram supports the recording and analyzing sampled data value counts across a configurable integer
    ///     value
    ///     range with configurable value precision within the range. Value precision is expressed as the number of significant
    ///     digits in the value recording, and provides control over value quantization behavior across the value range and the
    ///     subsequent value resolution at any given level.
    ///     For example, a Histogram could be configured to track the counts of observed integer values between 0 and
    ///     3,600,000,000 while maintaining a value precision of 3 significant digits across that range. Value quantization
    ///     within the range will thus be no larger than 1/1,000th (or 0.1%) of any value. This example Histogram could
    ///     be used to track and analyze the counts of observed response times ranging between 1 microsecond and 1 hour
    ///     in magnitude, while maintaining a value resolution of 1 microsecond up to 1 millisecond, a resolution of
    ///     1 millisecond (or better) up to one second, and a resolution of 1 second (or better) up to 1,000 seconds. At it's
    ///     maximum tracked value (1 hour), it would still maintain a resolution of 3.6 seconds (or better).
    ///     See package description for {@link org.HdrHistogram} for details.
    /// </summary>
    public abstract class AbstractHistogram : AbstractHistogramBase, IEquatable<AbstractHistogram>
    {
        internal int unitMagnitude;
        internal protected int subBucketHalfCount;
        internal protected int subBucketHalfCountMagnitude;

        // "Hot" accessed fields (used in the the value recording code path) are bunched here, such
        // that they will have a good chance of ending up in the same cache line as the totalCounts and
        // counts array reference fields that subclass implementations will typically add.
        // Sub-classes will typically add a totalCount field and a counts array field, which will likely be laid out
        // right around here due to the subclass layout rules in most practical JVM implementations.
        private int leadingZeroCountBase;
        private AtomicLong maxValue = new AtomicLong(0);
        private AtomicLong minNonZeroValue = new AtomicLong(long.MaxValue);
        private long subBucketMask;

        /// <summary>
        ///     Construct a Histogram given the Lowest and Highest values to be tracked and a number of significant
        ///     decimal digits. Providing a lowestDiscernibleValue is useful is situations where the units used
        ///     for the histogram's values are much smaller that the minimal accuracy required. E.g. when tracking
        ///     time values stated in nanosecond units, where the minimal accuracy required is a microsecond, the
        ///     proper value for lowestDiscernibleValue would be 1000.
        /// </summary>
        /// <param name="lowestDiscernibleValue">
        ///     The lowest value that can be discerned (distinguished from 0) by the histogram.
        ///     Must be a positive integer that is {@literal &gt;=} 1. May be internally rounded down to nearest power of 2.
        /// </param>
        /// <param name="highestTrackableValue">
        ///     The highest value to be tracked by the histogram. Must be a positive integer that
        ///     is {@literal &gt;=} (2 * lowestDiscernibleValue).
        /// </param>
        /// <param name="numberOfSignificantValueDigits">
        ///     Specifies the precision to use. This is the number of significant decimal
        ///     digits to which the histogram will maintain value resolution and separation. Must be a non-negative integer between
        ///     0 and 5.
        /// </param>
        /// <param name="wordSizeInBytes">The word size in bytes.</param>
        /// <param name="autoResize">if set to <c>true</c> [automatic resize].</param>
        /// <exception cref="System.ArgumentException">highestTrackableValue must be >= 2 * lowestDiscernibleValue</exception>
        protected AbstractHistogram(
            long lowestDiscernibleValue,
            long highestTrackableValue,
            int numberOfSignificantValueDigits,
            int wordSizeInBytes,
            bool autoResize)
            : base(lowestDiscernibleValue, numberOfSignificantValueDigits, wordSizeInBytes, autoResize)
        {
            if (highestTrackableValue < 2L * lowestDiscernibleValue)
            {
                throw new ArgumentException("highestTrackableValue must be >= 2 * lowestDiscernibleValue");
            }

            Init(highestTrackableValue, 1.0, 0);
        }

        /// <summary>
        ///     Construct a histogram with the same range settings as a given source histogram,
        ///     duplicating the source's start/end timestamps - but NOT its contents.
        /// </summary>
        /// <param name="source">The source histogram to duplicate/</param>
        protected AbstractHistogram(AbstractHistogram source)
            : this(
                source.getLowestDiscernibleValue(),
                source.getHighestTrackableValue(),
                source.getNumberOfSignificantValueDigits(),
                source.WordSizeInBytes,
                source.AutoResize)
        {
            setStartTimeStamp(source.getStartTimeStamp());
            setEndTimeStamp(source.getEndTimeStamp());
        }

        //
        //
        //
        // Copy support:
        //
        //
        //

        /**
         * Create a copy of this histogram, complete with data and everything.
         *
         * @return A distinct copy of this histogram.
         */

        abstract public AbstractHistogram copy();

        /**
         * Get a copy of this histogram, corrected for coordinated omission.
         * <p>
         * To compensate for the loss of sampled values when a recorded value is larger than the expected
         * interval between value samples, the new histogram will include an auto-generated additional series of
         * decreasingly-smaller (down to the expectedIntervalBetweenValueSamples) value records for each count found
         * in the current histogram that is larger than the expectedIntervalBetweenValueSamples.
         *
         * Note: This is a post-correction method, as opposed to the at-recording correction method provided
         * by {@link #RecordValueWithExpectedInterval(long, long) RecordValueWithExpectedInterval}. The two
         * methods are mutually exclusive, and only one of the two should be be used on a given data set to correct
         * for the same coordinated omission issue.
         * by
         * <p>
         * See notes in the description of the Histogram calls for an illustration of why this corrective behavior is
         * important.
         *
         * @param expectedIntervalBetweenValueSamples If expectedIntervalBetweenValueSamples is larger than 0, add
         *                                           auto-generated value records as appropriate if value is larger
         *                                           than expectedIntervalBetweenValueSamples
         * @return a copy of this histogram, corrected for coordinated omission.
         */

        abstract public AbstractHistogram copyCorrectedForCoordinatedOmission(long expectedIntervalBetweenValueSamples);

        /**
         * Get the total count of all recorded values in the histogram
         * @return the total count of all recorded values in the histogram
         */

        public abstract long getTotalCount();

        /// <summary>
        ///     Determine if this histogram is equivalent to another.
        /// </summary>
        /// <param name="other">the other histogram to compare to</param>
        /// <returns>True if this histogram are equivalent with the other.</returns>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Equals(other as AbstractHistogram);
        }

        //
        //
        //
        // Add support:
        //
        //
        //

        /**
         * Add the contents of another histogram to this one.
         * <p>
         * As part of adding the contents, the start/end timestamp range of this histogram will be
         * extended to include the start/end timestamp range of the other histogram.
         *
         * @param otherHistogram The other histogram.
         * @throws ArrayIndexOutOfBoundsException (may throw) if values in fromHistogram's are
         * higher than highestTrackableValue.
         */

        public void add(AbstractHistogram otherHistogram)
        {
            var highestRecordableValue = highestEquivalentValue(ValueFromIndex(countsArrayLength - 1));
            if (highestRecordableValue < otherHistogram.getMaxValue())
            {
                if (!AutoResize)
                {
                    throw new IndexOutOfRangeException("The other histogram includes values that do not fit in this histogram's range.");
                }

                resize(otherHistogram.getMaxValue());
            }

            if (bucketCount == otherHistogram.bucketCount &&
                subBucketCount == otherHistogram.subBucketCount &&
                unitMagnitude == otherHistogram.unitMagnitude &&
                getNormalizingIndexOffset() == otherHistogram.getNormalizingIndexOffset())
            {
                // Counts arrays are of the same length and meaning, so we can just iterate and add directly:
                long observedOtherTotalCount = 0;
                for (var i = 0; i < otherHistogram.countsArrayLength; i++)
                {
                    var otherCount = otherHistogram.getCountAtIndex(i);
                    if (otherCount > 0)
                    {
                        addToCountAtIndex(i, otherCount);
                        observedOtherTotalCount += otherCount;
                    }
                }

                setTotalCount(getTotalCount() + observedOtherTotalCount);
                updatedMaxValue(Math.Max(getMaxValue(), otherHistogram.getMaxValue()));
                updateMinNonZeroValue(Math.Min(getMinNonZeroValue(), otherHistogram.getMinNonZeroValue()));
            }
            else
            {
                // Arrays are not a direct match, so we can't just stream through and add them.
                // Instead, go through the array and add each non-zero value found at it's proper value:
                for (var i = 0; i < otherHistogram.countsArrayLength; i++)
                {
                    var otherCount = otherHistogram.getCountAtIndex(i);
                    if (otherCount > 0)
                    {
                        RecordValueWithCount(otherHistogram.ValueFromIndex(i), otherCount);
                    }
                }
            }

            setStartTimeStamp(Math.Min(startTimeStampMsec, otherHistogram.startTimeStampMsec));
            setEndTimeStamp(Math.Max(endTimeStampMsec, otherHistogram.endTimeStampMsec));
        }

        /**
         * Add the contents of another histogram to this one, while correcting the incoming data for coordinated omission.
         * <p>
         * To compensate for the loss of sampled values when a recorded value is larger than the expected
         * interval between value samples, the values added will include an auto-generated additional series of
         * decreasingly-smaller (down to the expectedIntervalBetweenValueSamples) value records for each count found
         * in the current histogram that is larger than the expectedIntervalBetweenValueSamples.
         *
         * Note: This is a post-recording correction method, as opposed to the at-recording correction method provided
         * by {@link #RecordValueWithExpectedInterval(long, long) RecordValueWithExpectedInterval}. The two
         * methods are mutually exclusive, and only one of the two should be be used on a given data set to correct
         * for the same coordinated omission issue.
         * by
         * <p>
         * See notes in the description of the Histogram calls for an illustration of why this corrective behavior is
         * important.
         *
         * @param otherHistogram The other histogram. highestTrackableValue and largestValueWithSingleUnitResolution must match.
         * @param expectedIntervalBetweenValueSamples If expectedIntervalBetweenValueSamples is larger than 0, add
         *                                           auto-generated value records as appropriate if value is larger
         *                                           than expectedIntervalBetweenValueSamples
         * @throws ArrayIndexOutOfBoundsException (may throw) if values exceed highestTrackableValue
         */

        public void addWhileCorrectingForCoordinatedOmission(AbstractHistogram otherHistogram, long expectedIntervalBetweenValueSamples)
        {
            var toHistogram = this;

            foreach (var v in otherHistogram.RecordedValues())
            {
                toHistogram.recordValueWithCountAndExpectedInterval(
                    v.GetValueIteratedTo(),
                    v.GetCountAtValueIteratedTo(),
                    expectedIntervalBetweenValueSamples);
            }
        }

        /**
         * Copy this histogram into the target histogram, overwriting it's contents.
         *
         * @param targetHistogram the histogram to copy into
         */

        public void copyInto(AbstractHistogram targetHistogram)
        {
            targetHistogram.reset();
            targetHistogram.add(this);
            targetHistogram.setStartTimeStamp(this.startTimeStampMsec);
            targetHistogram.setEndTimeStamp(this.endTimeStampMsec);
        }

        /**
         * Copy this histogram, corrected for coordinated omission, into the target histogram, overwriting it's contents.
         * (see {@link #copyCorrectedForCoordinatedOmission} for more detailed explanation about how correction is applied)
         *
         * @param targetHistogram the histogram to copy into
         * @param expectedIntervalBetweenValueSamples If expectedIntervalBetweenValueSamples is larger than 0, add
         *                                           auto-generated value records as appropriate if value is larger
         *                                           than expectedIntervalBetweenValueSamples
         */

        public void copyIntoCorrectedForCoordinatedOmission(AbstractHistogram targetHistogram, long expectedIntervalBetweenValueSamples)
        {
            targetHistogram.reset();
            targetHistogram.addWhileCorrectingForCoordinatedOmission(this, expectedIntervalBetweenValueSamples);
            targetHistogram.setStartTimeStamp(this.startTimeStampMsec);
            targetHistogram.setEndTimeStamp(this.endTimeStampMsec);
        }

        //
        //
        //
        // Comparison support:
        //
        //
        //

        /// <summary>
        ///     Determine if this histogram is equivalent to another.
        /// </summary>
        /// <param name="other">the other histogram to compare to</param>
        /// <returns>True if this histogram are equivalent with the other.</returns>
        public bool Equals(AbstractHistogram other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (LowestDiscernibleValue != other?.LowestDiscernibleValue ||
                HighestTrackableValue != other.HighestTrackableValue ||
                NumberOfSignificantValueDigits != other.NumberOfSignificantValueDigits ||

                // ReSharper disable CompareOfFloatsByEqualityOperator
                integerToDoubleValueConversionRatio != other.integerToDoubleValueConversionRatio)

                // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                return false;
            }
            if (countsArrayLength != other.countsArrayLength)
            {
                return false;
            }
            if (getTotalCount() != other.getTotalCount())
            {
                return false;
            }

            for (var i = 0; i < countsArrayLength; i++)
            {
                if (getCountAtIndex(i) != other.getCountAtIndex(i))
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Get the count of recorded values at a specific value (to within the histogram resolution at the value level).
         *
         * @param value The value for which to provide the recorded count
         * @return The total count of values recorded in the histogram within the value range that is
         * {@literal >=} lowestEquivalentValue(<i>value</i>) and {@literal <=} highestEquivalentValue(<i>value</i>)
         */

        public long getCountAtValue(long value)
        {
            var index = Math.Min(Math.Max(0, CountsArrayIndex(value)), countsArrayLength - 1);
            return getCountAtIndex(index);
        }

        /**
         * Get the count of recorded values within a range of value levels (inclusive to within the histogram's resolution).
         *
         * @param lowValue  The lower value bound on the range for which
         *                  to provide the recorded count. Will be rounded down with
         *                  {@link Histogram#lowestEquivalentValue lowestEquivalentValue}.
         * @param highValue  The higher value bound on the range for which to provide the recorded count.
         *                   Will be rounded up with {@link Histogram#highestEquivalentValue highestEquivalentValue}.
         * @return the total count of values recorded in the histogram within the value range that is
         * {@literal >=} lowestEquivalentValue(<i>lowValue</i>) and {@literal <=} highestEquivalentValue(<i>highValue</i>)
         */

        public long getCountBetweenValues(long lowValue, long highValue)
        {
            var lowIndex = Math.Max(0, CountsArrayIndex(lowValue));
            var highIndex = Math.Min(CountsArrayIndex(highValue), countsArrayLength - 1);
            long count = 0;
            for (var i = lowIndex; i <= highIndex; i++)
            {
                count += getCountAtIndex(i);
            }

            return count;
        }

        /**
         * get the end time stamp [optionally] stored with this histogram
         * @return the end time stamp [optionally] stored with this histogram
         */

        public long getEndTimeStamp() { return this.endTimeStampMsec; }

        /**
         * Provide a (conservatively high) estimate of the Histogram's total footprint in bytes
         *
         * @return a (conservatively high) estimate of the Histogram's total footprint in bytes
         */

        public int getEstimatedFootprintInBytes() { return _getEstimatedFootprintInBytes(); }

        /**
         * get the configured highestTrackableValue
         * @return highestTrackableValue
         */

        public long getHighestTrackableValue() { return HighestTrackableValue; }

        //
        //
        //
        // Histogram structure querying support:
        //
        //
        //

        /**
         * get the configured lowestDiscernibleValue
         * @return lowestDiscernibleValue
         */

        public long getLowestDiscernibleValue() { return LowestDiscernibleValue; }

        /**
         * Get the highest recorded value level in the histogram
         *
         * @return the Max value recorded in the histogram
         */

        public long getMaxValue()
        {
            var maxValue = this.maxValue.GetValue();
            return maxValue == 0 ? 0 : highestEquivalentValue(maxValue);
        }

        /**
         * Get the highest recorded value level in the histogram as a double
         *
         * @return the Max value recorded in the histogram
         */

        public double getMaxValueAsDouble() { return getMaxValue(); }

        /**
         * Get the computed mean value of all recorded values in the histogram
         *
         * @return the mean value (in value units) of the histogram data
         */

        public double getMean()
        {
            if (getTotalCount() == 0)
            {
                return 0.0;
            }

            recordedValuesIterator.reset();
            double totalValue = 0;
            while (recordedValuesIterator.hasNext())
            {
                var iterationValue = recordedValuesIterator.next();
                totalValue += medianEquivalentValue(iterationValue.GetValueIteratedTo())
                              * iterationValue.GetCountAtValueIteratedTo();
            }

            return totalValue * 1.0 / getTotalCount();
        }

        /**
         * Get the lowest recorded non-zero value level in the histogram
         *
         * @return the lowest recorded non-zero value level in the histogram
         */

        public long getMinNonZeroValue()
        {
            var minNonZeroValue = this.minNonZeroValue.GetValue();
            return minNonZeroValue == long.MaxValue ? long.MaxValue : lowestEquivalentValue(minNonZeroValue);
        }

        //
        //
        //
        // Histogram Data access support:
        //
        //
        //

        /**
         * Get the lowest recorded value level in the histogram
         *
         * @return the Min value recorded in the histogram
         */

        public long getMinValue()
        {
            if (getCountAtIndex(0) > 0 || getTotalCount() == 0)
            {
                return 0;
            }

            return getMinNonZeroValue();
        }

        /**
         * get the configured numberOfSignificantValueDigits
         * @return numberOfSignificantValueDigits
         */

        public int getNumberOfSignificantValueDigits() { return NumberOfSignificantValueDigits; }

        /**
         * Get the percentile at a given value.
         * The percentile returned is the percentile of values recorded in the histogram that are smaller
         * than or equivalent to the given value.
         * <p>
         * Note that two values are "equivalent" in this statement if
         * {@link org.HdrHistogram.AbstractHistogram#valuesAreEquivalent} would return true.
         *
         * @param value The value for which to return the associated percentile
         * @return The percentile of values recorded in the histogram that are smaller than or equivalent
         * to the given value.
         */

        public double getPercentileAtOrBelowValue(long value)
        {
            if (getTotalCount() == 0)
            {
                return 100.0;
            }

            var targetIndex = Math.Min(CountsArrayIndex(value), countsArrayLength - 1);
            long totalToCurrentIndex = 0;
            for (var i = 0; i <= targetIndex; i++)
            {
                totalToCurrentIndex += getCountAtIndex(i);
            }

            return 100.0 * totalToCurrentIndex / getTotalCount();
        }

        //
        //
        //
        // Timestamp support:
        //
        //
        //

        /**
         * get the start time stamp [optionally] stored with this histogram
         * @return the start time stamp [optionally] stored with this histogram
         */

        public long getStartTimeStamp() { return startTimeStampMsec; }

        /**
         * Get the computed standard deviation of all recorded values in the histogram
         *
         * @return the standard deviation (in value units) of the histogram data
         */

        public double getStdDeviation()
        {
            if (getTotalCount() == 0)
            {
                return 0.0;
            }

            var mean = getMean();
            var geometric_deviation_total = 0.0;
            recordedValuesIterator.reset();
            while (recordedValuesIterator.hasNext())
            {
                var iterationValue = recordedValuesIterator.next();
                var deviation = medianEquivalentValue(iterationValue.GetValueIteratedTo()) * 1.0 - mean;
                geometric_deviation_total += deviation * deviation * iterationValue.GetCountAddedInThisIterationStep();
            }

            var std_deviation = Math.Sqrt(geometric_deviation_total / getTotalCount());
            return std_deviation;
        }

        /**
         * Get the value at a given percentile.
         * When the given percentile is &gt; 0.0, the value returned is the value that the given
         * percentage of the overall recorded value entries in the histogram are either smaller than
         * or equivalent to. When the given percentile is 0.0, the value returned is the value that all value
         * entries in the histogram are either larger than or equivalent to.
         * <p>
         * Note that two values are "equivalent" in this statement if
         * {@link org.HdrHistogram.AbstractHistogram#valuesAreEquivalent} would return true.
         *
         * @param percentile  The percentile for which to return the associated value
         * @return The value that the given percentage of the overall recorded value entries in the
         * histogram are either smaller than or equivalent to. When the percentile is 0.0, returns the
         * value that all value entries in the histogram are either larger than or equivalent to.
         */

        public long getValueAtPercentile(double percentile)
        {
            var requestedPercentile = Math.Min(percentile, 100.0); // Truncate down to 100%
            var countAtPercentile = (long)(requestedPercentile / 100.0 * getTotalCount() + 0.5); // round to nearest
            countAtPercentile = Math.Max(countAtPercentile, 1); // Make sure we at least reach the first recorded entry
            long totalToCurrentIndex = 0;
            for (var i = 0; i < countsArrayLength; i++)
            {
                totalToCurrentIndex += getCountAtIndex(i);
                if (totalToCurrentIndex >= countAtPercentile)
                {
                    var valueAtIndex = ValueFromIndex(i);

                    // ReSharper disable CompareOfFloatsByEqualityOperator
                    return percentile == 0.0

                        // ReSharper restore CompareOfFloatsByEqualityOperator
                        ? lowestEquivalentValue(valueAtIndex)
                        : highestEquivalentValue(valueAtIndex);
                }
            }

            return 0;
        }

        /**
         * Get the highest value that is equivalent to the given value within the histogram's resolution.
         * Where "equivalent" means that value samples recorded for any two
         * equivalent values are counted in a common total count.
         *
         * @param value The given value
         * @return The highest value that is equivalent to the given value within the histogram's resolution.
         */

        public long highestEquivalentValue(long value) { return nextNonEquivalentValue(value) - 1; }

        /**
         * Get the lowest value that is equivalent to the given value within the histogram's resolution.
         * Where "equivalent" means that value samples recorded for any two
         * equivalent values are counted in a common total count.
         *
         * @param value The given value
         * @return The lowest value that is equivalent to the given value within the histogram's resolution.
         */

        public long lowestEquivalentValue(long value)
        {
            var bucketIndex = GetBucketIndex(value);
            var subBucketIndex = GetSubBucketIndex(value, bucketIndex);
            var thisValueBaseLevel = ValueFromIndex(bucketIndex, subBucketIndex);
            return thisValueBaseLevel;
        }

        /**
         * Get a value that lies in the middle (rounded up) of the range of values equivalent the given value.
         * Where "equivalent" means that value samples recorded for any two
         * equivalent values are counted in a common total count.
         *
         * @param value The given value
         * @return The value lies in the middle (rounded up) of the range of values equivalent the given value.
         */

        public long medianEquivalentValue(long value) { return lowestEquivalentValue(value) + (sizeOfEquivalentValueRange(value) >> 1); }

        /**
         * Get the next value that is not equivalent to the given value within the histogram's resolution.
         * Where "equivalent" means that value samples recorded for any two
         * equivalent values are counted in a common total count.
         *
         * @param value The given value
         * @return The next value that is not equivalent to the given value within the histogram's resolution.
         */

        public long nextNonEquivalentValue(long value) { return lowestEquivalentValue(value) + sizeOfEquivalentValueRange(value); }

        /// <summary>
        ///     Record a value in the histogram.
        /// </summary>
        /// <param name="value">value The value to be recorded.</param>
        public void RecordValue(long value)
        {
            RecordSingleValue(value);
        }

        /// <summary>
        ///     Record a value in the histogram (adding to the value's current count)
        /// </summary>
        /// <param name="value">The value to be recorded.</param>
        /// <param name="count">The number of occurrences of this value to record</param>
        public void RecordValueWithCount(long value, long count)
        {
            RecordCountAtValue(count, value);
        }

        /// <summary>
        ///     Record a value in the histogram.
        ///     To compensate for the loss of sampled values when a recorded value is larger than the expected
        ///     interval between value samples, Histogram will auto-generate an additional series of decreasingly-smaller
        ///     (down to the expectedIntervalBetweenValueSamples) value records.
        ///     Note: This is a at-recording correction method, as opposed to the post-recording correction method provided
        ///     by {@link #copyCorrectedForCoordinatedOmission(long)}.
        ///     The two methods are mutually exclusive, and only one of the two should be be used on a given data set to correct
        ///     for the same coordinated omission issue.
        ///     See notes in the description of the Histogram calls for an illustration of why this corrective behavior is
        ///     important.
        /// </summary>
        /// <param name="value">The value to record</param>
        /// <param name="expectedIntervalBetweenValueSamples">
        ///     If expectedIntervalBetweenValueSamples is larger than 0, add
        ///     auto-generated value records as appropriate if value is larger than expectedIntervalBetweenValueSamples
        /// </param>
        public void RecordValueWithExpectedInterval(long value, long expectedIntervalBetweenValueSamples)
        {
            recordSingleValueWithExpectedInterval(value, expectedIntervalBetweenValueSamples);
        }

        //
        //
        //
        // Clearing support:
        //
        //
        //

        /**
         * Reset the contents and stats of this histogram
         */

        public void reset()
        {
            clearCounts();
            resetMaxValue(0);
            resetMinNonZeroValue(long.MaxValue);
            setNormalizingIndexOffset(0);
        }

        /**
         * Set the end time stamp value associated with this histogram to a given value.
         * @param timeStampMsec the value to set the time stamp to, [by convention] in msec since the epoch.
         */

        public void setEndTimeStamp(long timeStampMsec) { this.endTimeStampMsec = timeStampMsec; }

        /**
         * Set the start time stamp value associated with this histogram to a given value.
         * @param timeStampMsec the value to set the time stamp to, [by convention] in msec since the epoch.
         */

        public void setStartTimeStamp(long timeStampMsec) { this.startTimeStampMsec = timeStampMsec; }

        /**
         * Get the size (in value units) of the range of values that are equivalent to the given value within the
         * histogram's resolution. Where "equivalent" means that value samples recorded for any two
         * equivalent values are counted in a common total count.
         *
         * @param value The given value
         * @return The lowest value that is equivalent to the given value within the histogram's resolution.
         */

        public long sizeOfEquivalentValueRange(long value)
        {
            var bucketIndex = GetBucketIndex(value);
            var subBucketIndex = GetSubBucketIndex(value, bucketIndex);
            var distanceToNextValue =
                1L << (unitMagnitude + (subBucketIndex >= subBucketCount ? bucketIndex + 1 : bucketIndex));
            return distanceToNextValue;
        }

        /**
         * Subtract the contents of another histogram from this one.
         * <p>
         * The start/end timestamps of this histogram will remain unchanged.
         *
         * @param otherHistogram The other histogram.
         * @throws ArrayIndexOutOfBoundsException (may throw) if values in otherHistogram's are higher than highestTrackableValue.
         *
         */

        public void subtract(AbstractHistogram otherHistogram)
        {
            var highestRecordableValue = ValueFromIndex(countsArrayLength - 1);
            if (highestRecordableValue < otherHistogram.getMaxValue())
            {
                if (!AutoResize)
                {
                    throw new IndexOutOfRangeException("The other histogram includes values that do not fit in this histogram's range.");
                }

                resize(otherHistogram.getMaxValue());
            }

            if (bucketCount == otherHistogram.bucketCount &&
                subBucketCount == otherHistogram.subBucketCount &&
                unitMagnitude == otherHistogram.unitMagnitude &&
                getNormalizingIndexOffset() == otherHistogram.getNormalizingIndexOffset())
            {
                // Counts arrays are of the same length and meaning, so we can just iterate and add directly:
                long observedOtherTotalCount = 0;
                for (var i = 0; i < otherHistogram.countsArrayLength; i++)
                {
                    var otherCount = otherHistogram.getCountAtIndex(i);
                    if (otherCount > 0)
                    {
                        if (getCountAtIndex(i) < otherCount)
                        {
                            throw new ArgumentException(
                                "otherHistogram count (" + otherCount + ") at value " +
                                ValueFromIndex(i) + " is larger than this one's (" + getCountAtIndex(i) + ")");
                        }

                        addToCountAtIndex(i, -otherCount);
                        observedOtherTotalCount += otherCount;
                    }
                }

                setTotalCount(getTotalCount() - observedOtherTotalCount);
                updatedMaxValue(Math.Max(getMaxValue(), otherHistogram.getMaxValue()));
                updateMinNonZeroValue(Math.Min(getMinNonZeroValue(), otherHistogram.getMinNonZeroValue()));
            }
            else
            {
                // Arrays are not a direct match, so we can't just stream through and add them.
                // Instead, go through the array and add each non-zero value found at it's proper value:
                for (var i = 0; i < otherHistogram.countsArrayLength; i++)
                {
                    var otherCount = otherHistogram.getCountAtIndex(i);
                    if (otherCount > 0)
                    {
                        var otherValue = otherHistogram.ValueFromIndex(i);
                        if (getCountAtValue(otherValue) < otherCount)
                        {
                            throw new ArgumentException(
                                "otherHistogram count (" + otherCount + ") at value " +
                                otherValue + " is larger than this one's (" + getCountAtValue(otherValue) + ")");
                        }

                        RecordValueWithCount(otherValue, -otherCount);
                    }
                }
            }

            // With subtraction, the max and minNonZero values could have changed:
            if (getCountAtValue(getMaxValue()) <= 0 || getCountAtValue(getMinNonZeroValue()) <= 0)
            {
                establishInternalTackingValues();
            }
        }

        /**
         * Determine if two values are equivalent with the histogram's resolution.
         * Where "equivalent" means that value samples recorded for any two
         * equivalent values are counted in a common total count.
         *
         * @param value1 first value to compare
         * @param value2 second value to compare
         * @return True if values are equivalent with the histogram's resolution.
         */

        public bool valuesAreEquivalent(long value1, long value2) { return lowestEquivalentValue(value1) == lowestEquivalentValue(value2); }

        internal static int NumberOfSubbuckets(int numberOfSignificantValueDigits)
        {
            var largestValueWithSingleUnitResolution = 2 * (long)Math.Pow(10, numberOfSignificantValueDigits);

            // We need to maintain power-of-two subBucketCount (for clean direct indexing) that is large enough to
            // provide unit resolution to at least largestValueWithSingleUnitResolution. So figure out
            // largestValueWithSingleUnitResolution's nearest power-of-two (rounded up), and use that:
            var subBucketCountMagnitude = (int)Math.Ceiling(Math.Log(largestValueWithSingleUnitResolution) / Math.Log(2));
            var subBucketCount = (int)Math.Pow(2, subBucketCountMagnitude);
            return subBucketCount;
        }

        internal abstract long getCountAtIndex(int index);

        internal int getBucketsNeededToCoverValue(long value)
        {
            var smallestUntrackableValue = (long)subBucketCount << unitMagnitude;
            var bucketsNeeded = 1;
            while (smallestUntrackableValue <= value)
            {
                if (smallestUntrackableValue > long.MaxValue / 2)
                {
                    return bucketsNeeded + 1;
                }

                smallestUntrackableValue <<= 1;
                bucketsNeeded++;
            }

            return bucketsNeeded;
        }

        internal int getLengthForNumberOfBuckets(int numberOfBuckets)
        {
            var lengthNeeded = (numberOfBuckets + 1) * (subBucketCount / 2);
            return lengthNeeded;
        }

        internal long ValueFromIndex(int index)
        {
            var bucketIndex = (index >> this.subBucketHalfCountMagnitude) - 1;
            var subBucketIndex = (index & (this.subBucketHalfCount - 1)) + this.subBucketHalfCount;
            if (bucketIndex < 0)
            {
                subBucketIndex -= this.subBucketHalfCount;
                bucketIndex = 0;
            }
            return ValueFromIndex(bucketIndex, subBucketIndex);
        }

        internal protected abstract int _getEstimatedFootprintInBytes();

        internal protected abstract void clearCounts();

        protected internal abstract void resize(long newHighestTrackableValue);

        //
        //
        //
        // Shifting support:
        //
        //
        //

        /**
         * Shift recorded values to the left (the equivalent of a &lt;&lt; shift operation on all recorded values). The
         * configured integer value range limits and value precision setting will remain unchanged.
         *
         * An {@link ArrayIndexOutOfBoundsException} will be thrown if any recorded values may be lost
         * as a result of the attempted operation, reflecting an "overflow" conditions. Expect such an overflow
         * exception if the operation would cause the current maxValue to be scaled to a value that is outside
         * of the covered value range.
         *
         * @param numberOfBinaryOrdersOfMagnitude The number of binary orders of magnitude to shift by
         */

        protected internal virtual void shiftValuesLeft(int numberOfBinaryOrdersOfMagnitude)
        {
            if (numberOfBinaryOrdersOfMagnitude < 0)
            {
                throw new ArgumentException("Cannot shift by a negative number of magnitudes");
            }

            if (numberOfBinaryOrdersOfMagnitude == 0)
            {
                return;
            }
            if (getTotalCount() == getCountAtIndex(0))
            {
                // (no need to shift any values if all recorded values are at the 0 value level:)
                return;
            }

            var shiftAmount = numberOfBinaryOrdersOfMagnitude << subBucketHalfCountMagnitude;
            var maxValueIndex = CountsArrayIndex(getMaxValue());

            // indicate overflow if maxValue is in the range being wrapped:
            if (maxValueIndex >= countsArrayLength - shiftAmount)
            {
                throw new IndexOutOfRangeException("Operation would overflow, would discard recorded value counts");
            }

            var maxValueBeforeShift = this.maxValue.GetAndSet(0);
            var minNonZeroValueBeforeShift = this.minNonZeroValue.GetAndSet(long.MaxValue);

            var lowestHalfBucketPopulated = minNonZeroValueBeforeShift < subBucketHalfCount;

            // Perform the shift:
            shiftNormalizingIndexByOffset(shiftAmount, lowestHalfBucketPopulated);

            // adjust min, max:
            UpdateMinAndMax(maxValueBeforeShift << numberOfBinaryOrdersOfMagnitude);
            if (minNonZeroValueBeforeShift < long.MaxValue)
            {
                UpdateMinAndMax(minNonZeroValueBeforeShift << numberOfBinaryOrdersOfMagnitude);
            }
        }

        /**
         * Shift recorded values to the right (the equivalent of a &gt;&gt; shift operation on all recorded values). The
         * configured integer value range limits and value precision setting will remain unchanged.
         * <p>
         * Shift right operations that do not underflow are reversible with a shift left operation with no loss of
         * information. An {@link ArrayIndexOutOfBoundsException} reflecting an "underflow" conditions will be thrown
         * if any recorded values may lose representation accuracy as a result of the attempted shift operation.
         * <p>
         * For a shift of a single order of magnitude, expect such an underflow exception if any recorded non-zero
         * values up to [numberOfSignificantValueDigits (rounded up to nearest power of 2) multiplied by
         * (2 ^ numberOfBinaryOrdersOfMagnitude) currently exist in the histogram.
         *
         * @param numberOfBinaryOrdersOfMagnitude The number of binary orders of magnitude to shift by
         */

        protected internal virtual void shiftValuesRight(int numberOfBinaryOrdersOfMagnitude)
        {
            if (numberOfBinaryOrdersOfMagnitude < 0)
            {
                throw new ArgumentException("Cannot shift by a negative number of magnitudes");
            }

            if (numberOfBinaryOrdersOfMagnitude == 0)
            {
                return;
            }
            if (getTotalCount() == getCountAtIndex(0))
            {
                // (no need to shift any values if all recorded values are at the 0 value level:)
                return;
            }

            var shiftAmount = subBucketHalfCount * numberOfBinaryOrdersOfMagnitude;

            // indicate underflow if minValue is in the range being shifted from:
            var minNonZeroValueIndex = CountsArrayIndex(getMinNonZeroValue());

            // Any shifting into the bottom-most half bucket would represents a loss of accuracy,
            // and a non-reversible operation. Therefore any non-0 value that falls in an
            // index below (shiftAmount + subBucketHalfCount) would represent an underflow:
            if (minNonZeroValueIndex < shiftAmount + subBucketHalfCount)
            {
                throw new IndexOutOfRangeException("Operation would underflow and lose precision of already recorded value counts");
            }

            // perform shift:
            var maxValueBeforeShift = this.maxValue.GetAndSet(0);
            var minNonZeroValueBeforeShift = this.minNonZeroValue.GetAndSet(long.MaxValue);

            // move normalizingIndexOffset
            shiftNormalizingIndexByOffset(-shiftAmount, false);

            // adjust min, max:
            UpdateMinAndMax(maxValueBeforeShift >> numberOfBinaryOrdersOfMagnitude);
            if (minNonZeroValueBeforeShift < long.MaxValue)
            {
                UpdateMinAndMax(minNonZeroValueBeforeShift >> numberOfBinaryOrdersOfMagnitude);
            }
        }

        protected static int NormalizeIndex(int index, int normalizingIndexOffset, int arrayLength)
        {
            if (normalizingIndexOffset == 0)
            {
                // Fastpath out of normalization. Keeps integer value histograms fast while allowing
                // others (like DoubleHistogram) to use normalization at a cost...
                return index;
            }

            return NormalizedNonZeroIndex(index, normalizingIndexOffset, arrayLength);
        }

        protected abstract void addToCountAtIndex(int index, long value);

        protected abstract void addToTotalCount(long value);

        protected abstract long getCountAtNormalizedIndex(int index);

        protected abstract int getNormalizingIndexOffset();

        protected abstract void incrementCountAtIndex(int index);

        protected abstract void incrementTotalCount();

        protected abstract void setCountAtIndex(int index, long value);

        protected abstract void setCountAtNormalizedIndex(int index, long value);

        protected abstract void setNormalizingIndexOffset(int normalizingIndexOffset);

        protected abstract void setTotalCount(long totalCount);

        protected abstract void shiftNormalizingIndexByOffset(int offsetToAdd, bool lowestHalfBucketPopulated);


        /**
         * Set internally tracked maxValue to new value if new value is greater than current one.
         * May be overridden by subclasses for synchronization or atomicity purposes.
         * @param value new maxValue to set
         */

        protected virtual void updatedMaxValue(long value)
        {
            long current;
            while (value > (current = maxValue.GetValue()))
            {
                maxValue.CompareAndSwap(current, value);
            }
        }

        /**
         * Set internally tracked minNonZeroValue to new value if new value is smaller than current one.
         * May be overridden by subclasses for synchronization or atomicity purposes.
         * @param value new minNonZeroValue to set
         */

        protected virtual void updateMinNonZeroValue(long value)
        {
            long current;
            while (value < (current = this.minNonZeroValue.GetValue()))
            {
                this.minNonZeroValue.CompareAndSwap(current, value);
            }
        }

        protected int CountsArrayIndex(long value)
        {
            if (value < 0)
            {
                throw new IndexOutOfRangeException("Histogram recorded value cannot be negative.");
            }

            var bucketIndex = GetBucketIndex(value);
            var subBucketIndex = GetSubBucketIndex(value, bucketIndex);
            return CountsArrayIndex(bucketIndex, subBucketIndex);
        }

        protected int determineArrayLengthNeeded(long highestTrackableValue)
        {
            if (highestTrackableValue < 2L * LowestDiscernibleValue)
            {
                throw new ArgumentException("highestTrackableValue (" + highestTrackableValue + ") cannot be < (2 * lowestDiscernibleValue)");
            }

            // TODO: Determine counts array length needed:
            return getLengthForNumberOfBuckets(getBucketsNeededToCoverValue(highestTrackableValue));
        }

        protected void establishSize(long newHighestTrackableValue)
        {
            // establish counts array length:
            countsArrayLength = determineArrayLengthNeeded(newHighestTrackableValue);

            // establish exponent range needed to support the trackable value with no overflow:
            bucketCount = getBucketsNeededToCoverValue(newHighestTrackableValue);

            // establish the new highest trackable value:
            HighestTrackableValue = newHighestTrackableValue;
        }

        protected void nonConcurrentNormalizingIndexShift(int shiftAmount, bool lowestHalfBucketPopulated)
        {
            // Save and clear the 0 value count:
            var zeroValueCount = getCountAtIndex(0);
            setCountAtIndex(0, 0);

            setNormalizingIndexOffset(getNormalizingIndexOffset() + shiftAmount);

            // Deal with lower half bucket if needed:
            if (lowestHalfBucketPopulated)
            {
                shiftLowestHalfBucketContentsLeft(shiftAmount);
            }

            // Restore the 0 value count:
            setCountAtIndex(0, zeroValueCount);
        }

        private static int NormalizedNonZeroIndex(int index, int normalizingIndexOffset, int arrayLength)
        {
            if (index > arrayLength || index < 0)
            {
                throw new IndexOutOfRangeException("index out of covered value range");
            }

            var normalizedIndex = index - normalizingIndexOffset;

            // The following is the same as an unsigned remainder operation, as long as no double wrapping happens
            // (which shouldn't happen, as normalization is never supposed to wrap, since it would have overflowed
            // or underflowed before it did). This (the + and - tests) seems to be faster than a % op with a
            // correcting if < 0...:
            if (normalizedIndex < 0)
            {
                normalizedIndex += arrayLength;
            }
            else if (normalizedIndex >= arrayLength)
            {
                normalizedIndex -= arrayLength;
            }
            return normalizedIndex;
        }

        private int CountsArrayIndex(int bucketIndex, int subBucketIndex)
        {
            Debug.Assert(subBucketIndex < this.subBucketCount);
            Debug.Assert(bucketIndex == 0 || subBucketIndex >= this.subBucketHalfCount);

            // Calculate the index for the first entry in the bucket:
            // (The following is the equivalent of ((bucketIndex + 1) * subBucketHalfCount) ):
            var bucketBaseIndex = (bucketIndex + 1) << this.subBucketHalfCountMagnitude;

            // Calculate the offset in the bucket (can be negative for first bucket):
            var offsetInBucket = subBucketIndex - this.subBucketHalfCount;

            // The following is the equivalent of ((subBucketIndex  - subBucketHalfCount) + bucketBaseIndex;
            return bucketBaseIndex + offsetInBucket;
        }

        private void establishInternalTackingValues()
        {
            resetMaxValue(0);
            resetMinNonZeroValue(long.MaxValue);
            var maxIndex = -1;
            var minNonZeroIndex = -1;
            long observedTotalCount = 0;
            for (var index = 0; index < countsArrayLength; index++)
            {
                long countAtIndex;
                if ((countAtIndex = getCountAtIndex(index)) > 0)
                {
                    observedTotalCount += countAtIndex;
                    maxIndex = index;
                    if (minNonZeroIndex == -1 && index != 0)
                    {
                        minNonZeroIndex = index;
                    }
                }
            }

            if (maxIndex >= 0)
            {
                updatedMaxValue(highestEquivalentValue(ValueFromIndex(maxIndex)));
            }
            if (minNonZeroIndex >= 0)
            {
                updateMinNonZeroValue(ValueFromIndex(minNonZeroIndex));
            }
            setTotalCount(observedTotalCount);
        }

        private int GetBucketIndex(long value) { return this.leadingZeroCountBase - MathUtils.NumberOfLeadingZeros(value | this.subBucketMask); }

        private int GetSubBucketIndex(long value, int bucketIndex) { return (int)((ulong)value >> (bucketIndex + this.unitMagnitude)); }

        private void handleRecordException(long count, long value, Exception ex)
        {
            if (!AutoResize)
            {
                throw new IndexOutOfRangeException("value outside of histogram covered range. Caused by: " + ex);
            }

            resize(value);
            var countsIndex = CountsArrayIndex(value);
            addToCountAtIndex(countsIndex, count);
            this.HighestTrackableValue = highestEquivalentValue(ValueFromIndex(countsArrayLength - 1));
        }

        // ReSharper disable ParameterHidesMember
        private void Init(long highestTrackableValue, double integerToDoubleValueConversionRatio, int normalizingIndexOffset)

            // ReSharper restore ParameterHidesMember
        {
            this.HighestTrackableValue = highestTrackableValue;
            this.integerToDoubleValueConversionRatio = integerToDoubleValueConversionRatio;
            if (normalizingIndexOffset != 0)
            {
                setNormalizingIndexOffset(normalizingIndexOffset);
            }

            var largestValueWithSingleUnitResolution = 2 * (long)Math.Pow(10, NumberOfSignificantValueDigits);

            unitMagnitude = (int)Math.Floor(Math.Log(LowestDiscernibleValue) / Math.Log(2));

            // We need to maintain power-of-two subBucketCount (for clean direct indexing) that is large enough to
            // provide unit resolution to at least largestValueWithSingleUnitResolution. So figure out
            // largestValueWithSingleUnitResolution's nearest power-of-two (rounded up), and use that:
            var subBucketCountMagnitude = (int)Math.Ceiling(Math.Log(largestValueWithSingleUnitResolution) / Math.Log(2));
            subBucketHalfCountMagnitude = (subBucketCountMagnitude > 1 ? subBucketCountMagnitude : 1) - 1;
            subBucketCount = (int)Math.Pow(2, subBucketHalfCountMagnitude + 1);
            subBucketHalfCount = subBucketCount / 2;
            subBucketMask = ((long)subBucketCount - 1) << unitMagnitude;


            // determine exponent range needed to support the trackable value with no overflow:
            establishSize(highestTrackableValue);

            // Establish leadingZeroCountBase, used in getBucketIndex() fast path:
            leadingZeroCountBase = 64 - unitMagnitude - subBucketHalfCountMagnitude - 1;
        }

        private void RecordCountAtValue(long count, long value)
        {
            var countsIndex = CountsArrayIndex(value);
            try
            {
                addToCountAtIndex(countsIndex, count);
            }
            catch (IndexOutOfRangeException ex)
            {
                handleRecordException(count, value, ex);
            }

            UpdateMinAndMax(value);
            addToTotalCount(count);
        }

        private void RecordSingleValue(long value)
        {
            var countsIndex = CountsArrayIndex(value);
            try
            {
                incrementCountAtIndex(countsIndex);
            }
            catch (IndexOutOfRangeException ex)
            {
                handleRecordException(1, value, ex);
            }
            UpdateMinAndMax(value);
            incrementTotalCount();
        }

        private void recordSingleValueWithExpectedInterval(long value, long expectedIntervalBetweenValueSamples)
        {
            RecordSingleValue(value);
            if (expectedIntervalBetweenValueSamples <= 0)
            {
                return;
            }

            for (var missingValue = value - expectedIntervalBetweenValueSamples;
                 missingValue >= expectedIntervalBetweenValueSamples;
                 missingValue -= expectedIntervalBetweenValueSamples)
            {
                RecordSingleValue(missingValue);
            }
        }

        private void recordValueWithCountAndExpectedInterval(long value, long count, long expectedIntervalBetweenValueSamples)
        {
            RecordCountAtValue(count, value);
            if (expectedIntervalBetweenValueSamples <= 0)
            {
                return;
            }

            for (var missingValue = value - expectedIntervalBetweenValueSamples;
                 missingValue >= expectedIntervalBetweenValueSamples;
                 missingValue -= expectedIntervalBetweenValueSamples)
            {
                RecordCountAtValue(count, missingValue);
            }
        }

        // ReSharper disable ParameterHidesMember
        private void resetMaxValue(long maxValue)

            // ReSharper restore ParameterHidesMember
        {
            this.maxValue.SetValue(maxValue);
        }

        // ReSharper disable ParameterHidesMember
        private void resetMinNonZeroValue(long minNonZeroValue)

            // ReSharper restore ParameterHidesMember
        {
            this.minNonZeroValue.SetValue(minNonZeroValue);
        }

        void shiftLowestHalfBucketContentsLeft(int shiftAmount)
        {
            var numberOfBinaryOrdersOfMagnitude = shiftAmount >> subBucketHalfCountMagnitude;

            // The lowest half-bucket (not including the 0 value) is special: unlike all other half
            // buckets, the lowest half bucket values cannot be scaled by simply changing the
            // normalizing offset. Instead, they must be individually re-recorded at the new
            // scale, and cleared from the current one.
            //
            // We know that all half buckets "below" the current lowest one are full of 0s, because
            // we would have overflowed otherwise. So we need to shift the values in the current
            // lowest half bucket into that range (including the current lowest half bucket itself).
            // Iterating up from the lowermost non-zero "from slot" and copying values to the newly
            // scaled "to slot" (and then zeroing the "from slot"), will work in a single pass,
            // because the scale "to slot" index will always be a lower index than its or any
            // preceding non-scaled "from slot" index:            
            // (Note that we specifically avoid slot 0, as it is directly handled in the outer case)
            for (var fromIndex = 1; fromIndex < subBucketHalfCount; fromIndex++)
            {
                var toValue = ValueFromIndex(fromIndex) << numberOfBinaryOrdersOfMagnitude;
                var toIndex = CountsArrayIndex(toValue);
                var countAtFromIndex = getCountAtNormalizedIndex(fromIndex);
                setCountAtIndex(toIndex, countAtFromIndex);
                setCountAtNormalizedIndex(fromIndex, 0);
            }

            // Note that the above loop only creates O(N) work for histograms that have values in
            // the lowest half-bucket (excluding the 0 value). Histograms that never have values
            // there (e.g. all integer value histograms used as internal storage in DoubleHistograms)
            // will never loop, and their shifts will remain O(1).
        }

        private void UpdateMinAndMax(long value)
        {
            // we can use non volatile get as the update method checks again
            if (value > this.maxValue.NonVolatileGetValue())
            {
                updatedMaxValue(value);
            }
            if (value < this.minNonZeroValue.NonVolatileGetValue() && value != 0)
            {
                updateMinNonZeroValue(value);
            }
        }

        private long ValueFromIndex(int bucketIndex, int subBucketIndex) { return (long)subBucketIndex << (bucketIndex + unitMagnitude); }
    }

#pragma warning restore
}

// ReSharper restore ArrangeModifiersOrder
// ReSharper restore ArrangeThisQualifier
// ReSharper restore InconsistentNaming
// ReSharper restore LocalVariableHidesMember