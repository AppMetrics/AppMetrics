// <copyright file="DefaultAlgorithmRReservoir.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Concurrency;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.ReservoirSampling.Uniform
{
    /// <summary>
    ///     A histogram with a uniform reservoir produces <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see>
    ///     which are valid for the entirely of the histogram’s lifetime.
    ///     <p>
    ///         This sampling reservoir can be used when you are interested in long-term measurements, it does not offer a sence
    ///         of recency.
    ///     </p>
    ///     <p>
    ///         All samples are equally likely to be evicted when the reservoir is at full capacity.
    ///     </p>
    /// </summary>
    /// <remarks>
    ///     Uses <see href="http://www.cs.umd.edu/~samir/498/vitter.pdf">Vitter's Algorithm R</see> for
    ///     <see cref="IReservoir">reservoir</see> sampling
    /// </remarks>
    /// <seealso cref="IReservoir" />
    public sealed class DefaultAlgorithmRReservoir : IReservoir
    {
        private readonly UserValueWrapper[] _values;

        private AtomicLong _count = new AtomicLong(0);
        private AtomicDouble _sum = new AtomicDouble(0.0);

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultAlgorithmRReservoir" /> class.
        /// </summary>
        public DefaultAlgorithmRReservoir()
            : this(AppMetricsReservoirSamplingConstants.DefaultSampleSize)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultAlgorithmRReservoir" /> class.
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir</param>
        public DefaultAlgorithmRReservoir(int sampleSize)
        {
            _values = new UserValueWrapper[sampleSize];
        }

        /// <summary>
        ///     Gets the size.
        /// </summary>
        /// <value>
        ///     The size.
        /// </value>
        // ReSharper disable MemberCanBePrivate.Global
        public int Size => Math.Min((int)_count.GetValue(), _values.Length);
        // ReSharper restore MemberCanBePrivate.Global

        /// <inheritdoc cref="IReservoir" />
        public IReservoirSnapshot GetSnapshot(bool resetReservoir)
        {
            var size = Size;

            if (size == 0)
            {
                return new UniformSnapshot(0, 0.0, Enumerable.Empty<long>());
            }

            var snapshotValues = new UserValueWrapper[size];

            Array.Copy(_values, snapshotValues, size);

            if (resetReservoir)
            {
                _count.SetValue(0L);
                _sum.SetValue(0.0);
            }

            Array.Sort(snapshotValues, UserValueWrapper.Comparer);

            var minValue = snapshotValues[0].UserValue;
            var maxValue = snapshotValues[size - 1].UserValue;

            return new UniformSnapshot(
                _count.GetValue(),
                _sum.GetValue(),
                snapshotValues.Select(v => v.Value),
                valuesAreSorted: true,
                minUserValue: minValue,
                maxUserValue: maxValue);
        }

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot()
        {
            return GetSnapshot(false);
        }

        /// <inheritdoc cref="IReservoir" />
        public void Reset()
        {
            _count.SetValue(0L);
            _sum.SetValue(0.0);
        }

        /// <summary>
        ///     Updates the sample set adding the specified value using
        ///     <see href="http://www.cs.umd.edu/~samir/498/vitter.pdf">Vitter's Algorithm R</see>.
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
        public void Update(long value, string userValue)
        {
            var c = _count.Increment();

            _sum.Add(value);

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

        /// <inheritdoc />
        public void Update(long value)
        {
            Update(value, null);
        }
    }
}