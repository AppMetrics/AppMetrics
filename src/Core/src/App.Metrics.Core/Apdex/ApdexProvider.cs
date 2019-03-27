// <copyright file="ApdexProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.ReservoirSampling;

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
        private readonly IReservoir _reservoir;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApdexProvider" /> class.
        /// </summary>
        /// <param name="reservoir">The reservoir used to sample values in order to caclulate an apdex score.</param>
        /// <param name="apdexTSeconds">The apdex t seconds used to calculate satisfied, tolerating and frustrating counts.</param>
        public ApdexProvider(IReservoir reservoir, double apdexTSeconds = AppMetricsReservoirSamplingConstants.DefaultApdexTSeconds)
        {
            _reservoir = reservoir;
            _apdexTSeconds = apdexTSeconds;
        }

        // <inheritdoc />
        public ApdexSnapshot GetSnapshot(bool resetReservoir = false)
        {
            var reservoirSnapshot = _reservoir.GetSnapshot(resetReservoir);

            return new ApdexSnapshot(reservoirSnapshot.Values, _apdexTSeconds);
        }

        // <inheritdoc />
        public void Reset() { _reservoir.Reset(); }

        // <inheritdoc />
        public void Update(long value) { _reservoir.Update(value); }
    }
}