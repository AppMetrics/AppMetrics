// <copyright file="IApdexProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.ReservoirSampling;

namespace App.Metrics.Apdex
{
    /// <summary>
    ///     Provides access to an ApdexProvider Implementation responsible for sampling measured duration to calculate an apdex
    ///     score
    /// </summary>
    public interface IApdexProvider
    {
        /// <summary>
        ///     Gets a <see cref="ApdexSnapshot">snapshot</see> including the number of satisfied, tolerating, frustrating
        ///     requests, the apdex score and the number of samples used to calculate the result.
        /// </summary>
        /// <param name="resetReservoir">if set to <c>true</c> [reset reservoir].</param>
        /// <returns>The apdex snapshot</returns>
        ApdexSnapshot GetSnapshot(bool resetReservoir = false);

        /// <summary>
        ///     Reset all values, in addition to the underlying reservoir.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Update the chosen <see cref="IReservoir">reservoir</see> with a new sample.
        /// </summary>
        /// <param name="value">The value.</param>
        void Update(long value);
    }
}