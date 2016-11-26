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
    ///     A histogram with a uniform reservoir produces <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see>
    ///     which are valid for the entirely of the histogram’s lifetime.
    ///     <p>
    ///         This sampling resevoir can be used when you are interested in long-term measurements, it does not offer a sence
    ///         of recency.
    ///     </p>
    ///     <p>
    ///         All samples are equally likely to be evicted when the reservoir is at full capacity.
    ///     </p>
    /// </summary>
    /// <remarks>
    ///     Uses <see href="http://www.cs.umd.edu/~samir/498/vitter.pdf">Vitter's Algorithm R</see> for
    ///     <see cref="IReservoir">resevoir</see> sampling
    /// </remarks>
    /// <seealso cref="IReservoir" />
    public sealed class UniformReservoir : IReservoir
    {
        private const int DefaultSampleSize = 1028;

        private readonly UserValueWrapper[] _values;

        private AtomicLong _count = new AtomicLong();

        /// <summary>
        ///     Initializes a new instance of the <see cref="UniformReservoir" /> class.
        /// </summary>
        /// <remarks>
        ///     The default sample size is 1028
        /// </remarks>
        public UniformReservoir()
            : this(DefaultSampleSize)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UniformReservoir" /> class.
        /// </summary>
        /// <param name="sampleSize">The random sample size to generate, defaults to 1028</param>
        public UniformReservoir(int sampleSize)
        {
            _values = new UserValueWrapper[sampleSize];
        }

        /// <inheritdoc cref="IReservoir" />
        public int Size => Math.Min((int)_count.GetValue(), _values.Length);

        /// <inheritdoc cref="IReservoir" />
        public ISnapshot GetSnapshot(bool resetReservoir = false)
        {
            var size = Size;

            if (size == 0)
            {
                return new UniformSnapshot(0, Enumerable.Empty<long>());
            }

            var snapshotValues = new UserValueWrapper[size];

            Array.Copy(_values, snapshotValues, size);

            if (resetReservoir)
            {
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
            _count.SetValue(0L);
        }

        /// <summary>
        ///     Updates the sample set adding the specified value using <see href="http://www.cs.umd.edu/~samir/498/vitter.pdf">Vitter's Algorithm R</see>.
        /// </summary>
        /// Algorithm R pseudo code
        /// <example>
        ///     <code>
        ///     <![CDATA[
        ///     -- S has items to sample, R will contain the result
        ///     ReservoirSample(S[1..n], R[1..k])
        ///         -- fill the reservoir array
        ///         for i = 1 to k
        ///             R[i] := S[i]
        ///             -- replace elements with gradually decreasing probability
        ///         for i = k+1 to n
        ///             j := random(1, i)   -- important: inclusive range
        ///             if j <= k
        ///             R[j] := S[i]
        ///     ]]>        
        /// </code>
        /// </example>
        /// <param name="value">The value to add to the sample set.</param>
        /// <param name="userValue">The user value to track, which records the last, min and max user values within the sample.</param>
        public void Update(long value, string userValue = null)
        {
            var c = _count.Increment();

            if (c <= _values.Length)
            {
                _values[(int)c - 1] = new UserValueWrapper(value, userValue);
            }
            else
            {
                var r = ThreadLocalRandom.NextLong(c);

                if (r < _values.Length)
                {
                    _values[(int)r] = new UserValueWrapper(value, userValue);
                }
            }
        }
    }
}