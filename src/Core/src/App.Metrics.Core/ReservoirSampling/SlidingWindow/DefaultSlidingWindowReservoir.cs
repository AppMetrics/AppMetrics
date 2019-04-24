// <copyright file="DefaultSlidingWindowReservoir.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Concurrency;
using App.Metrics.ReservoirSampling.Uniform;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.ReservoirSampling.SlidingWindow
{
    /// <summary>
    ///     A Reservoir implementation backed by a sliding window that stores only the measurements made in the last N seconds
    ///     (or other time unit).
    /// </summary>
    /// <seealso cref="IReservoir" />
    public sealed class DefaultSlidingWindowReservoir : IReservoir
    {
        private readonly UserValueWrapper[] _values;
        private AtomicLong _count = new AtomicLong(0);
        private AtomicDouble _sum = new AtomicDouble(0.0);

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultSlidingWindowReservoir" /> class.
        /// </summary>
        public DefaultSlidingWindowReservoir()
            : this(AppMetricsReservoirSamplingConstants.DefaultSampleSize) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultSlidingWindowReservoir" /> class.
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir</param>
        public DefaultSlidingWindowReservoir(int sampleSize) { _values = new UserValueWrapper[sampleSize]; }

        /// <inheritdoc cref="IReservoir" />
        public IReservoirSnapshot GetSnapshot(bool resetReservoir)
        {
            var size = Math.Min((int)_count.GetValue(), _values.Length);

            if (size == 0)
            {
                return new UniformSnapshot(0, 0.0, Enumerable.Empty<long>());
            }

            var snapshotValues = new UserValueWrapper[size];

            Array.Copy(_values, snapshotValues, size);

            Array.Sort(snapshotValues, UserValueWrapper.Comparer);

            var minValue = snapshotValues[0].UserValue;
            var maxValue = snapshotValues[size - 1].UserValue;

            var result = new UniformSnapshot(
                _count.GetValue(),
                _sum.GetValue(),
                snapshotValues.Select(v => v.Value),
                true,
                minValue,
                maxValue);

            if (resetReservoir)
            {
                Array.Clear(_values, 0, snapshotValues.Length);
                _count.SetValue(0L);
                _sum.SetValue(0.0);
            }

            return result;
        }

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot() { return GetSnapshot(false); }

        /// <inheritdoc cref="IReservoir" />
        public void Reset()
        {
            Array.Clear(_values, 0, _values.Length);
            _count.SetValue(0L);
            _sum.SetValue(0.0);
        }

        /// <inheritdoc cref="IReservoir" />
        public void Update(long value, string userValue)
        {
            var newCount = _count.Increment();

            _sum.Add(value);

            _values[(int)((newCount - 1) % _values.Length)] = new UserValueWrapper(value, userValue);
        }

        /// <inheritdoc />
        public void Update(long value) { Update(value, null); }
    }
}