// <copyright file="ApdexProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Internal;

namespace App.Metrics.Apdex
{
    /// <summary>
    ///     The default <see cref="IApdexProvider">IApdexProvider</see> implementation which uses the specified
    ///     <see cref="IReservoir">reservoir</see> to sample values in order to caclulate an apdex score
    /// </summary>
    /// <seealso>
    ///     <cref>App.Metrics.Apdex.Interfaces.IApdexProvider</cref>
    /// </seealso>
    public class ApdexProvider : IApdexProvider
    {
        private readonly double _apdexTSeconds;
        private readonly Lazy<IReservoir> _reservoir;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApdexProvider" /> class.
        /// </summary>
        /// <param name="reservoir">The reservoir used to sample values in order to caclulate an apdex score.</param>
        /// <param name="apdexTSeconds">The apdex t seconds used to calculate satisfied, tolerating and frustrating counts.</param>
        public ApdexProvider(Lazy<IReservoir> reservoir, double apdexTSeconds = Constants.ReservoirSampling.DefaultApdexTSeconds)
        {
            _reservoir = reservoir;
            _apdexTSeconds = apdexTSeconds;
        }

        // <inheritdoc />
        public ApdexSnapshot GetSnapshot(bool resetReservoir = false)
        {
            var reservoirSnapshot = _reservoir.Value.GetSnapshot(resetReservoir);

            return new ApdexSnapshot(reservoirSnapshot.Values, _apdexTSeconds);
        }

        // <inheritdoc />
        public void Reset() { _reservoir.Value.Reset(); }

        // <inheritdoc />
        public void Update(long value) { _reservoir.Value.Update(value); }
    }
}