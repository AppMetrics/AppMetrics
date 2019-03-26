// <copyright file="IReservoirSnapshot.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.ReservoirSampling
{
    /// <summary>
    ///     Provides access to a snapshot used for calculating
    ///     <see href="https://en.wikipedia.org/wiki/Quantile">quantile</see> statistics.
    /// </summary>
    public interface IReservoirSnapshot
    {
        /// <summary>
        ///     Gets the number of samples that the histogram has been updated with.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        long Count { get; }

        /// <summary>
        ///     Gets the maximum value of all samples
        /// </summary>
        /// <value>
        ///     The maximum.
        /// </value>
        long Max { get; }

        /// <summary>
        ///     Gets the maximum value which was recorded in the samples set
        /// </summary>
        /// <value>
        ///     The maximum user value.
        /// </value>
        string MaxUserValue { get; }

        /// <summary>
        ///     Gets the average of all samples since the histogram was created.
        /// </summary>
        /// <value>
        ///     The mean.
        /// </value>
        double Mean { get; }

        /// <summary>
        ///     Gets the median value of all samples
        /// </summary>
        /// <value>
        ///     The median.
        /// </value>
        double Median { get; }

        /// <summary>
        ///     Gets the minimum value of all samples.
        /// </summary>
        /// <value>
        ///     The minimum.
        /// </value>
        long Min { get; }

        /// <summary>
        ///     Gets the minumum value which was recorded in the samples set
        /// </summary>
        /// <value>
        ///     The minimum user value.
        /// </value>
        string MinUserValue { get; }

        /// <summary>
        ///     Gets the 75th percentile of all samples
        /// </summary>
        /// <value>
        ///     The percentile75.
        /// </value>
        double Percentile75 { get; }

        /// <summary>
        ///     Gets the 95th percentile of all samples
        /// </summary>
        /// <value>
        ///     The percentile95.
        /// </value>
        double Percentile95 { get; }

        /// <summary>
        ///     Gets the 98th percentile of all samples
        /// </summary>
        /// <value>
        ///     The percentile98.
        /// </value>
        double Percentile98 { get; }

        /// <summary>
        ///     Gets the 99th percentile of all samples
        /// </summary>
        /// <value>
        ///     The percentile99.
        /// </value>
        double Percentile99 { get; }

        /// <summary>
        ///     Gets the 99.9th percentile of all samples
        /// </summary>
        /// <value>
        ///     The percentile999.
        /// </value>
        double Percentile999 { get; }

        /// <summary>
        ///     Gets the current size of the histogram's reservoir
        /// </summary>
        /// <value>
        ///     The size.
        /// </value>
        int Size { get; }

        /// <summary>
        ///     Gets the standard deviation of all samples.
        /// </summary>
        /// <value>
        ///     The standard dev.
        /// </value>
        double StdDev { get; }

        /// <summary>
        ///     Gets the sum of observed values.
        /// </summary>
        /// <value>
        ///     The sum.
        /// </value>
        double Sum { get; }

        /// <summary>
        ///     Gets the samples of the snapshot
        /// </summary>
        /// <value>
        ///     The values.
        /// </value>
        IEnumerable<long> Values { get; }

        /// <summary>
        ///     Calculate an arbitrary quantile value for the snapshot. Values below zero or greater than one will be clamped to
        ///     the range [0, 1]
        /// </summary>
        /// <param name="quantile">The quantile.</param>
        /// <returns>The quantile value</returns>
        // ReSharper disable UnusedMemberInSuper.Global
        double GetValue(double quantile);
        // ReSharper restore UnusedMemberInSuper.Global
    }
}