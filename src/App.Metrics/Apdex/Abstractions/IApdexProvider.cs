// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.ReservoirSampling;

namespace App.Metrics.Apdex.Abstractions
{
    /// <summary>
    ///     Provides access to an ApdexProvider Implementation responsible for sampling measured duration to calcualte an apdex
    ///     score
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IApdexProvider : IDisposable
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