// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Apdex.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Sampling.Interfaces;

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
        private bool _disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApdexProvider" /> class.
        /// </summary>
        /// <remarks>
        ///     The apdex T <see cref="Constants.ReservoirSampling">default</see> value will be used
        /// </remarks>
        /// <param name="reservoir">The reservoir used to sample values in order to caclulate an apdex score.</param>
        public ApdexProvider(IReservoir reservoir)
            : this(reservoir, Constants.ReservoirSampling.DefaultApdexTSeconds) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApdexProvider" /> class.
        /// </summary>
        /// <param name="reservoir">The reservoir used to sample values in order to caclulate an apdex score.</param>
        /// <param name="apdexTSeconds">The apdex t seconds used to calculate satisfied, tolerating and frustrating counts.</param>
        public ApdexProvider(IReservoir reservoir, double apdexTSeconds)
        {
            _reservoir = reservoir;
            _apdexTSeconds = apdexTSeconds;
        }

        // <inheritdoc />
        ~ApdexProvider() { Dispose(false); }

        // <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    var reservoir = _reservoir as IDisposable;
                    reservoir?.Dispose();
                }
            }

            _disposed = true;
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