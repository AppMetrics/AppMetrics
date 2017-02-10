// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Concurrency;
using App.Metrics.Core.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Scheduling;
using App.Metrics.Scheduling.Abstractions;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.ReservoirSampling.ExponentialDecay
{
    /// <summary>
    ///     A histogram with an exponentially decaying reservoir produces
    ///     <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see> which are representative of (roughly) the last
    ///     five minutes of data.
    ///     <p>
    ///         The resevoir is produced by using a
    ///         <see href="http://dimacs.rutgers.edu/~graham/pubs/papers/fwddecay.pdf">forward-decaying resevoir</see> with an
    ///         exponential weighty towards recent data unlike a Uniform Reservoir which does not provide a sense of recency.
    ///     </p>
    ///     <p>
    ///         This sampling resevoir can be used when you are interested in recent changes to the distribution of data rather
    ///         than a median on the lifetime of the histgram.
    ///     </p>
    /// </summary>
    /// <seealso cref="IReservoir" />
    public sealed class DefaultForwardDecayingReservoir : IReservoir, IDisposable
    {
        private static readonly TimeSpan RescaleInterval = TimeSpan.FromHours(1);

        private readonly double _alpha;

        private readonly IClock _clock;

        private readonly IScheduler _rescaleScheduler;
        private readonly int _sampleSize;

        private readonly SortedList<double, WeightedSample> _values;
        private AtomicLong _count = new AtomicLong(0);
        private bool _disposed;

        private SpinLock _lock = default(SpinLock);
        private AtomicLong _startTime;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultForwardDecayingReservoir" /> class.
        /// </summary>
        /// <remarks>
        ///     The default size and alpha values offer a 99.9% confidence level with a 5% margin of error assuming a normal
        ///     distribution and heavily biases the reservoir to the past 5 minutes of measurements.
        /// </remarks>
        public DefaultForwardDecayingReservoir()
            : this(
                Constants.ReservoirSampling.DefaultSampleSize,
                Constants.ReservoirSampling.DefaultExponentialDecayFactor,
                new StopwatchClock(),
                new DefaultTaskScheduler())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultForwardDecayingReservoir" /> class.
        /// </summary>
        /// <remarks>
        ///     The default size and alpha values offer a 99.9% confidence level with a 5% margin of error assuming a normal
        ///     distribution and heavily biases the reservoir to the past 5 minutes of measurements.
        /// </remarks>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir.</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        public DefaultForwardDecayingReservoir(int sampleSize, double alpha)
            : this(sampleSize, alpha, new StopwatchClock(), new DefaultTaskScheduler())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultForwardDecayingReservoir" /> class.
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <param name="clock">The <see cref="IClock">clock</see> type to use for calculating processing time.</param>
        /// <param name="scheduler">
        ///     The scheduler to to rescale, allowing decayed weights to be tracked. Really only provided here
        ///     for testing purposes.
        /// </param>
        public DefaultForwardDecayingReservoir(int sampleSize, double alpha, IClock clock, IScheduler scheduler)
        {
            _sampleSize = sampleSize;
            _alpha = alpha;
            _clock = clock;

            _values = new SortedList<double, WeightedSample>(sampleSize, ReverseOrderDoubleComparer.Instance);

            _rescaleScheduler = scheduler;
            _rescaleScheduler.Interval(RescaleInterval, TaskCreationOptions.LongRunning, Rescale);

            _startTime = new AtomicLong(clock.Seconds);
        }

        /// <summary>
        ///     Gets the size.
        /// </summary>
        /// <value>
        ///     The size.
        /// </value>
        public int Size => Math.Min(_sampleSize, (int)_count.GetValue());

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
                    using (_rescaleScheduler)
                    {
                    }
                }
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot(bool resetReservoir)
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);
                var snapshot = new WeightedSnapshot(_count.GetValue(), _values.Values);
                if (resetReservoir)
                {
                    ResetReservoir();
                }

                return snapshot;
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot()
        {
            return GetSnapshot(false);
        }

        /// <inheritdoc cref="IReservoir" />
        public void Reset()
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);
                ResetReservoir();
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        /// <inheritdoc cref="IReservoir" />
        public void Update(long value, string userValue)
        {
            Update(value, userValue, _clock.Seconds);
        }

        /// <inheritdoc />
        public void Update(long value)
        {
            Update(value, null);
        }

        /// <summary>
        ///     A common feature of the above techniques—indeed, the key technique that
        ///     allows us to track the decayed weights efficiently—is that they maintain
        ///     counts and other quantities based on g(ti − L), and only scale by g(t − L)
        ///     at query time. But while g(ti −L)/g(t−L) is guaranteed to lie between zero
        ///     and one, the intermediate values of g(ti − L) could become very large. For
        ///     polynomial functions, these values should not grow too large, and should be
        ///     effectively represented in practice by floating point values without loss of
        ///     precision. For exponential functions, these values could grow quite large as
        ///     new values of (ti − L) become large, and potentially exceed the capacity of
        ///     common floating point types. However, since the values stored by the
        ///     algorithms are linear combinations of g values (scaled sums), they can be
        ///     rescaled relative to a new landmark. That is, by the analysis of exponential
        ///     decay in Section III-A, the choice of L does not affect the final result. We
        ///     can therefore multiply each value based on L by a factor of exp(−α(L′ − L)),
        ///     and obtain the correct value as if we had instead computed relative to a new
        ///     landmark L′ (and then use this new L′ at query time). This can be done with
        ///     a linear pass over whatever data structure is being used."
        /// </summary>
        private void Rescale()
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);
                var oldStartTime = _startTime.GetValue();
                _startTime.SetValue(_clock.Seconds);

                var scalingFactor = Math.Exp(-_alpha * (_startTime.GetValue() - oldStartTime));

                var keys = new List<double>(_values.Keys);
                foreach (var key in keys)
                {
                    var sample = _values[key];
                    _values.Remove(key);
                    var newKey = key * Math.Exp(-_alpha * (_startTime.GetValue() - oldStartTime));
                    var newSample = new WeightedSample(sample.Value, sample.UserValue, sample.Weight * scalingFactor);
                    _values[newKey] = newSample;
                }

                // make sure the counter is in sync with the number of stored samples.
                _count.SetValue(_values.Count);
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        private void ResetReservoir()
        {
            _values.Clear();
            _count.SetValue(0L);
            _startTime.SetValue(_clock.Seconds);
        }

        private void Update(long value, string userValue, long timestamp)
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);

                var itemWeight = Math.Exp(_alpha * (timestamp - _startTime.GetValue()));
                var sample = new WeightedSample(value, userValue, itemWeight);

                var random = 0.0;

                // Prevent division by 0
                while (random.Equals(.0))
                {
                    random = ThreadLocalRandom.NextDouble();
                }

                var priority = itemWeight / random;

                var newCount = _count.GetValue();
                newCount++;
                _count.SetValue(newCount);

                if (newCount <= _sampleSize)
                {
                    _values[priority] = sample;
                }
                else
                {
                    var first = _values.Keys[_values.Count - 1];
                    if (first < priority)
                    {
                        _values.Remove(first);
                        _values[priority] = sample;
                    }
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        private sealed class ReverseOrderDoubleComparer : IComparer<double>
        {
            public static readonly IComparer<double> Instance = new ReverseOrderDoubleComparer();

            public int Compare(double x, double y) { return y.CompareTo(x); }
        }
    }
}