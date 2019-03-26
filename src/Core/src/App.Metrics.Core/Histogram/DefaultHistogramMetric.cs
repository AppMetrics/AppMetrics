// <copyright file="DefaultHistogramMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Histogram
{
    public sealed class DefaultHistogramMetric : IHistogramMetric
    {
        private readonly IReservoir _reservoir;
        private bool _disposed;
        private UserValueWrapper _last;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHistogramMetric" /> class.
        /// </summary>
        /// <param name="reservoir">The reservoir to use for sampling.</param>
        public DefaultHistogramMetric(IReservoir reservoir)
        {
            _reservoir = reservoir ?? throw new ArgumentNullException(nameof(reservoir));
        }

        public HistogramValue Value => GetValue();

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
        }

        [ExcludeFromCodeCoverage]
        // ReSharper disable MemberCanBePrivate.Global
        public void Dispose(bool disposing)
            // ReSharper restore MemberCanBePrivate.Global
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    if (_reservoir != null)
                    {
                        using (_reservoir as IDisposable)
                        {
                        }
                    }
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
        public void Update(long value, string userValue)
        {
            _last = new UserValueWrapper(value, userValue);
            _reservoir.Update(value, userValue);
        }

        /// <inheritdoc />
        public void Update(long value)
        {
            _last = new UserValueWrapper(value);
            _reservoir.Update(value);
        }
    }
}