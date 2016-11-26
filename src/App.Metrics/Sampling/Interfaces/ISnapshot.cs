// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System.Collections.Generic;

namespace App.Metrics.Sampling.Interfaces
{
    /// <summary>
    ///     Provides access to a snapshot used for calculating
    ///     <see href="https://en.wikipedia.org/wiki/Quantile">quantile</see> statistics.
    /// </summary>
    public interface ISnapshot
    {
        /// <summary>
        ///     Get the number of samples that the histogram has been updated with.
        /// </summary>
        long Count { get; }

        /// <summary>
        ///     Get the maximum value of all samples
        /// </summary>
        long Max { get; }

        /// <summary>
        ///     Get the maximum value which was recorded in the samples set
        /// </summary>
        string MaxUserValue { get; }

        /// <summary>
        ///     Get the average of all samples since the histogram was created.
        /// </summary>
        double Mean { get; }

        /// <summary>
        ///     Get the median value of all samples
        /// </summary>
        double Median { get; }

        /// <summary>
        ///     Get the minimum value of all samples.
        /// </summary>
        long Min { get; }

        /// <summary>
        ///     Get the minumum value which was recorded in the samples set
        /// </summary>
        string MinUserValue { get; }

        /// <summary>
        ///     Calculate the 75th percentile of all samples
        /// </summary>
        double Percentile75 { get; }

        /// <summary>
        ///     Calculate the 95th percentile of all samples
        /// </summary>
        double Percentile95 { get; }

        /// <summary>
        ///     Calculate the 98th percentile of all samples
        /// </summary>
        double Percentile98 { get; }

        /// <summary>
        ///     Calculate the 99th percentile of all samples
        /// </summary>
        double Percentile99 { get; }

        /// <summary>
        ///     Calculate the 99.9th percentile of all samples
        /// </summary>
        double Percentile999 { get; }

        /// <summary>
        ///     Get the current size of the histogram's reservoir
        /// </summary>
        int Size { get; }

        /// <summary>
        ///     Gets the standard deviation of all samples.
        /// </summary>
        double StdDev { get; }

        /// <summary>
        ///     Gets the samples of the snapshot
        /// </summary>
        IEnumerable<long> Values { get; }

        /// <summary>
        ///     Calculate an arbitrary quantile value for the snapshot. Values below zero or greater than one will be clamped to
        ///     the range [0, 1]
        /// </summary>
        double GetValue(double quantile);
    }
}