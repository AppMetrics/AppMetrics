// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using System.Linq;
using App.Metrics.Concurrency;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Sampling
{
    /// <summary>
    ///     A Reservoir implementation backed by a sliding window that stores only the measurements made in the last N seconds
    ///     (or other time unit).
    /// </summary>
    /// <seealso cref="IReservoir" />
    public sealed class SlidingWindowReservoir : IReservoir
    {
        private const int DefaultSampleSize = 1028;

        private readonly UserValueWrapper[] _values;
        private AtomicLong _count = new AtomicLong();

        /// <summary>
        ///     Initializes a new instance of the <see cref="SlidingWindowReservoir" /> class.
        /// </summary>
        /// <remarks>
        ///     The default sample size is 1028
        /// </remarks>
        public SlidingWindowReservoir()
            : this(DefaultSampleSize)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SlidingWindowReservoir" /> class.
        /// </summary>
        /// <param name="sampleSize">The sample size to generate, defaults to 1028.</param>
        public SlidingWindowReservoir(int sampleSize)
        {
            _values = new UserValueWrapper[sampleSize];
        }

        /// <inheritdoc cref="IReservoir" />
        public ISnapshot GetSnapshot(bool resetReservoir = false)
        {
            var size = Math.Min((int)_count.GetValue(), _values.Length);

            if (size == 0)
            {
                return new UniformSnapshot(0, Enumerable.Empty<long>());
            }

            var snapshotValues = new UserValueWrapper[size];

            Array.Copy(_values, snapshotValues, size);

            if (resetReservoir)
            {
                Array.Clear(_values, 0, snapshotValues.Length);
                _count.SetValue(0L);
            }

            Array.Sort(snapshotValues, UserValueWrapper.Comparer);

            var minValue = snapshotValues[0].UserValue;
            var maxValue = snapshotValues[size - 1].UserValue;

            return new UniformSnapshot(_count.GetValue(),
                snapshotValues.Select(v => v.Value),
                valuesAreSorted: true,
                minUserValue: minValue,
                maxUserValue: maxValue);
        }

        /// <inheritdoc cref="IReservoir" />
        public void Reset()
        {
            Array.Clear(_values, 0, _values.Length);
            _count.SetValue(0L);
        }

        /// <inheritdoc cref="IReservoir" />
        public void Update(long value, string userValue = null)
        {
            var newCount = _count.Increment();

            _values[(int)((newCount - 1) % _values.Length)] = new UserValueWrapper(value, userValue);
        }
    }
}