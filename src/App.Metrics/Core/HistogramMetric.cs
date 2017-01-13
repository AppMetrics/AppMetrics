// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Data;
using App.Metrics.Internal;
using App.Metrics.Sampling;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Core
{
    public sealed class HistogramMetric : IHistogramMetric
    {
        private readonly IReservoir _reservoir;
        private bool _disposed;
        private UserValueWrapper _last;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HistogramMetric" /> class.
        /// </summary>
        /// <param name="samplingType">Type of the reservoir sampling to use.</param>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        public HistogramMetric(SamplingType samplingType, int sampleSize, double alpha)
            : this(ReservoirBuilder.Build(samplingType, sampleSize, alpha)) { }

        public HistogramMetric(IReservoir reservoir)
        {
            if (reservoir == null)
            {
                throw new ArgumentNullException(nameof(reservoir));
            }

            _reservoir = reservoir;
        }

        [AppMetricsExcludeFromCodeCoverage]
        ~HistogramMetric() { Dispose(false); }

        public HistogramValue Value => GetValue();

        [AppMetricsExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [AppMetricsExcludeFromCodeCoverage]
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public HistogramValue GetValue(bool resetMetric = false)
        {
            var value = new HistogramValue(_last.Value, _last.UserValue, _reservoir.GetSnapshot(resetMetric));

            if (resetMetric)
            {
                _last = UserValueWrapper.Empty;
            }

            return value;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _last = UserValueWrapper.Empty;
            _reservoir.Reset();
        }

        /// <inheritdoc />
        public void Update(long value, string userValue = null)
        {
            _last = new UserValueWrapper(value, userValue);
            _reservoir.Update(value, userValue);
        }
    }
}