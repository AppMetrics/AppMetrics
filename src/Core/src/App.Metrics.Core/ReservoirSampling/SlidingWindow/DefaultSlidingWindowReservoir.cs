// <copyright file="DefaultSlidingWindowReservoir.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.ReservoirSampling.Abstractions;
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
    public sealed class DefaultSlidingWindowReservoir : ReservoirBase<UserValueWrapper[]>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultSlidingWindowReservoir" /> class.
        /// </summary>
        public DefaultSlidingWindowReservoir()
            : this(AppMetricsReservoirSamplingConstants.DefaultSampleSize)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultSlidingWindowReservoir" /> class.
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir</param>
        public DefaultSlidingWindowReservoir(int sampleSize)
            : base(new UserValueWrapper[sampleSize])
        {
        }

        /// <inheritdoc cref="IReservoir" />
        public override IReservoirSnapshot GetSnapshot(bool resetReservoir)
        {
            int size = Size;

            if (size == 0)
            {
                return new UniformSnapshot(0, 0.0, Enumerable.Empty<long>());
            }

            var snapshotValues = new UserValueWrapper[size];

            Array.Copy(ValuesCollection, snapshotValues, size);

            Array.Sort(snapshotValues, UserValueWrapper.Comparer);

            var minValue = snapshotValues[0].UserValue;
            var maxValue = snapshotValues[size - 1].UserValue;

            var result = new UniformSnapshot(
                Count.GetValue(),
                Sum.GetValue(),
                snapshotValues.Select(v => v.Value),
                true,
                minValue,
                maxValue);

            if (resetReservoir)
            {
                Array.Clear(ValuesCollection, 0, snapshotValues.Length);
                Count.SetValue(0L);
                Sum.SetValue(0.0);
            }

            return result;
        }

        /// <inheritdoc cref="IReservoir" />
        public override void Reset()
        {
            Array.Clear(ValuesCollection, 0, ValuesCollection.Length);
            Count.SetValue(0L);
            Sum.SetValue(0.0);
        }

        /// <inheritdoc cref="IReservoir" />
        public override void Update(long value, string userValue)
        {
            var newCount = Count.Increment();

            Sum.Add(value);

            ValuesCollection[(int)((newCount - 1) % ValuesCollection.Length)] = new UserValueWrapper(value, userValue);
        }
    }
}