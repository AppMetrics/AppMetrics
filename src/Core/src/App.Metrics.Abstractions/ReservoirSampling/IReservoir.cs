// <copyright file="IReservoir.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.ReservoirSampling
{
    /// <summary>
    ///     Provides access to a Reservoir Sampling implementation. Reservoir sampling is a family of randomized algorithms for
    ///     randomly choosing a sample of k items from a list S containing n items, where n is either a very large or unknown
    ///     number. Typically n is large enough that the list doesn't fit into main memory.
    /// </summary>
    public interface IReservoir
    {
        /// <summary>
        ///     Gets a statistical <see cref="IReservoirSnapshot">snapshot</see> including
        ///     <see href="https://en.wikipedia.org/wiki/Percentile">percentiles</see> of the current sample.
        /// </summary>
        /// <param name="resetReservoir">if set to <c>true</c> [reset reservoir].</param>
        /// <returns>A <see cref="IReservoirSnapshot">snapshot</see> of the current sample</returns>
        IReservoirSnapshot GetSnapshot(bool resetReservoir);

        /// <summary>
        ///     Gets a statistical <see cref="IReservoirSnapshot">snapshot</see> including
        ///     <see href="https://en.wikipedia.org/wiki/Percentile">percentiles</see> of the current sample.
        /// </summary>
        /// <returns>A <see cref="IReservoirSnapshot">snapshot</see> of the current sample</returns>
        IReservoirSnapshot GetSnapshot();

        /// <summary>
        ///     Reset all statistics, in addition to the underlying reservoir.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Update statistics and the reservoir with a new sample.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="userValue">The user value.</param>
        void Update(long value, string userValue);

        /// <summary>
        ///     Update statistics and the reservoir with a new sample.
        /// </summary>
        /// <param name="value">The value.</param>
        void Update(long value);
    }
}