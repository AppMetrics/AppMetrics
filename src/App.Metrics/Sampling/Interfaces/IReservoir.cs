// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using App.Metrics.Configuration;
using App.Metrics.Core.Options;

namespace App.Metrics.Sampling.Interfaces
{
    /// <summary>
    ///     Provides access to a Resevoir Sampling implementation. Reservoir sampling is a family of randomized algorithms for
    ///     randomly choosing a sample of k items from a list S containing n items, where n is either a very large or unknown
    ///     number. Typically n is large enough that the list doesn't fit into main memory.
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 <see cref="UniformReservoir">UniformReservoir</see>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="ExponentiallyDecayingReservoir">ExponentiallyDecayingReservoir</see>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description><see cref="SlidingWindowReservoir">SlidingWindowReservoir</see>.</description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="HdrHistogramReservoir">HdrHistogramReservoir</see>
    ///             </description>
    ///         </item>
    ///     </list>
    /// </summary>
    /// <remarks>
    ///     To specify the type of reservoir sampling to use either set the <see cref="SamplingType.Default">default</see> type
    ///     globally using the<see cref="AppMetricsOptions">configuration options</see> on startup, or use the
    ///     <see cref="HistogramOptions">histogram options</see> or <see cref="TimerOptions">timer options</see> when defining
    ///     these metrics types which support resevoir sampling. If no sampling type is specified the
    ///     <see cref="ExponentiallyDecayingReservoir">resevoir</see> will be used.
    /// </remarks>
    public interface IReservoir
    {
        /// <summary>
        ///     Gets a statistical <see cref="ISnapshot">snapshot</see> including
        ///     <see href="https://en.wikipedia.org/wiki/Percentile">percentiles</see> of the current sample.
        /// </summary>
        /// <param name="resetReservoir">if set to <c>true</c> [reset reservoir].</param>
        /// <returns>A <see cref="ISnapshot">snapshot</see> of the current sample</returns>
        ISnapshot GetSnapshot(bool resetReservoir = false);

        /// <summary>
        ///     Reset all statistics, in addition to the underlying reservoir.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Update statistics and the reservoir with a new sample.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="userValue">The user value.</param>
        void Update(long value, string userValue = null);
    }
}