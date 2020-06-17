// <copyright file="DefaultForwardDecayingReservoir.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using App.Metrics.Concurrency;
using App.Metrics.Infrastructure;
using App.Metrics.Logging;
using App.Metrics.Scheduling;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.ReservoirSampling.ExponentialDecay
{
    /// <summary>
    ///     A histogram with an exponentially decaying reservoir produces
    ///     <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see> which are representative of (roughly) the last
    ///     five minutes of data.
    ///     <p>
    ///         The reservoir is produced by using a
    ///         <see href="http://dimacs.rutgers.edu/~graham/pubs/papers/fwddecay.pdf">forward-decaying reservoir</see> with an
    ///         exponential weighty towards recent data unlike a Uniform Reservoir which does not provide a sense of recency.
    ///     </p>
    ///     <p>
    ///         This sampling reservoir can be used when you are interested in recent changes to the distribution of data rather
    ///         than a median on the lifetime of the histgram.
    ///     </p>
    /// </summary>
    /// <seealso cref="IReservoir" />
    public sealed class DefaultForwardDecayingReservoir : IRescalingReservoir, IDisposable
    {
        private static readonly ILog Logger = LogProvider.For<DefaultForwardDecayingReservoir>();
        private readonly double _alpha;
        private readonly double _minimumSampleWeight;
        private readonly IClock _clock;
        private readonly IReservoirRescaleScheduler _rescaleScheduler;
        private readonly int _sampleSize;

        private SortedList<double, WeightedSample> _values;
        private AtomicLong _count = new AtomicLong(0);
        private bool _disposed;
        private SpinLock _lock = default;
        private AtomicLong _startTime;
        private AtomicDouble _sum = new AtomicDouble(0.0);

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultForwardDecayingReservoir" /> class.
        /// </summary>
        /// <remarks>
        ///     The default size and alpha values offer a 99.9% confidence level with a 5% margin of error assuming a normal
        ///     distribution and heavily biases the reservoir to the past 5 minutes of measurements.
        /// </remarks>
        public DefaultForwardDecayingReservoir()
            : this(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                AppMetricsReservoirSamplingConstants.DefaultMinimumSampleWeight,
                new StopwatchClock())
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
            : this(sampleSize, alpha, AppMetricsReservoirSamplingConstants.DefaultMinimumSampleWeight, new StopwatchClock())
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
        /// <param name="minimumSampleWeight">
        ///     Minimum weight required for a sample to be retained during reservoir rescaling. Samples with weights less than this value will be discarded.
        ///     This behavior is useful if there are longer periods of very low or no activity. Default value is zero, which preserves all samples during rescaling.
        /// </param>
        public DefaultForwardDecayingReservoir(int sampleSize, double alpha, double minimumSampleWeight)
            : this(sampleSize, alpha, minimumSampleWeight, new StopwatchClock())
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
        /// <param name="minimumSampleWeight">
        ///     Minimum weight required for a sample to be retained during reservoir rescaling. Samples with weights less than this value will be discarded.
        ///     This behavior is useful if there are longer periods of very low or no activity. Default value is zero, which preserves all samples during rescaling.
        /// </param>
        /// <param name="clock">The <see cref="IClock">clock</see> type to use for calculating processing time.</param>
        // ReSharper disable MemberCanBePrivate.Global
        public DefaultForwardDecayingReservoir(int sampleSize, double alpha, double minimumSampleWeight, IClock clock)
            // ReSharper restore MemberCanBePrivate.Global
        {
            _sampleSize = sampleSize;
            _alpha = alpha;
            _minimumSampleWeight = minimumSampleWeight;
            _clock = clock;

            _values = new SortedList<double, WeightedSample>(sampleSize, ReverseOrderDoubleComparer.Instance);
            _startTime = new AtomicLong(clock.Seconds);
            _rescaleScheduler = DefaultReservoirRescaleScheduler.Instance;
            _rescaleScheduler.ScheduleReScaling(this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultForwardDecayingReservoir" /> class.
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <param name="minimumSampleWeight">
        ///     Minimum weight required for a sample to be retained during reservoir rescaling. Samples with weights less than this value will be discarded.
        ///     This behavior is useful if there are longer periods of very low or no activity. Default value is zero, which preserves all samples during rescaling.
        /// </param>
        /// <param name="clock">The <see cref="IClock">clock</see> type to use for calculating processing time.</param>
        /// <param name="rescaleScheduler">The schedular used to rescale the reservoir.</param>
        // ReSharper disable MemberCanBePrivate.Global
        public DefaultForwardDecayingReservoir(int sampleSize, double alpha, double minimumSampleWeight, IClock clock, IReservoirRescaleScheduler rescaleScheduler)
            // ReSharper restore MemberCanBePrivate.Global
        {
            _sampleSize = sampleSize;
            _alpha = alpha;
            _minimumSampleWeight = minimumSampleWeight;
            _clock = clock;
            _rescaleScheduler = rescaleScheduler;

            _values = new SortedList<double, WeightedSample>(sampleSize, ReverseOrderDoubleComparer.Instance);
            _startTime = new AtomicLong(clock.Seconds);
            _rescaleScheduler.ScheduleReScaling(this);
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
        // ReSharper disable MemberCanBePrivate.Global
        public void Dispose(bool disposing)
            // ReSharper restore MemberCanBePrivate.Global
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _rescaleScheduler.RemoveSchedule(this);
                }
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot(bool resetReservoir)
        {
            var lockTaken = false;

            Logger.Trace("Getting {Reservoir} snapshot", this);

            try
            {
                _lock.Enter(ref lockTaken);

                Logger.Trace("Lock entered for {Reservoir} snapshot", this);

                var snapshot = new WeightedSnapshot(_count.GetValue(), _sum.GetValue(), _values.Values);
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
                    Logger.Trace("Lock exited after getting {Reservoir} snapshot", this);
                }

                Logger.Trace("Retrieved {Reservoir} snapshot", this);
            }
        }

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot()
        {
            return GetSnapshot(false);
        }

        /// <inheritdoc />
        public void Reset()
        {
            var lockTaken = false;

            Logger.Trace("Resetting {Reservoir}", this);

            try
            {
                _lock.Enter(ref lockTaken);

                Logger.Trace("Lock entered for {Reservoir} reset", this);

                ResetReservoir();
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                    Logger.Trace("Lock exited after resetting {Reservoir}", this);
                }

                Logger.Trace("{Reservoir} reset", this);
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
        public void Rescale()
        {
            var lockTaken = false;

            Logger.Trace("Rescaling {Reservoir}", this);

            try
            {
                _lock.Enter(ref lockTaken);

                Logger.Trace("Lock entered for rescaling {Reservoir}", this);

                var oldStartTime = _startTime.GetValue();
                _startTime.SetValue(_clock.Seconds);

                var scalingFactor = Math.Exp(-_alpha * (_startTime.GetValue() - oldStartTime));
                var newSamples = new Dictionary<double, WeightedSample>(_values.Count);

                foreach (var keyValuePair in _values)
                {
                    var sample = keyValuePair.Value;

                    var newWeight = sample.Weight * scalingFactor;
                    if (newWeight < _minimumSampleWeight)
                    {
                        continue;
                    }

                    var newKey = keyValuePair.Key * scalingFactor;
                    var newSample = new WeightedSample(sample.Value, sample.UserValue, newWeight);
                    newSamples[newKey] = newSample;
                }

                _values = new SortedList<double, WeightedSample>(newSamples, ReverseOrderDoubleComparer.Instance);
                _values.Capacity = _sampleSize;

                // make sure the counter is in sync with the number of stored samples.
                _count.SetValue(_values.Count);
                _sum.SetValue(_values.Values.Sum(sample => sample.Value));
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                    Logger.Trace("Lock exited after rescaling {Reservoir}", this);
                }

                Logger.Trace("{Reservoir} rescaled", this);
            }
        }

        private void ResetReservoir()
        {
            Logger.Trace("Resetting {Reservoir}", this);
            _values.Clear();
            _count.SetValue(0L);
            _sum.SetValue(0.0);
            _startTime.SetValue(_clock.Seconds);
            Logger.Trace("{Reservoir} reset", this);
        }

        private void Update(long value, string userValue, long timestamp)
        {
            Logger.Trace("Updating {Reservoir}", this);
            var lockTaken = false;
            try
            {
                var itemWeight = Math.Exp(_alpha * (timestamp - _startTime.GetValue()));
                
                if (double.IsInfinity(itemWeight))
                {
                    ResetReservoir();
                    itemWeight = Math.Exp(_alpha * (timestamp - _startTime.GetValue()));
                }
                
                var sample = new WeightedSample(value, userValue, itemWeight);

                var random = 0.0;

                // Prevent division by 0
                while (random.Equals(.0))
                {
                    random = ThreadLocalRandom.NextDouble();
                }

                var priority = itemWeight / random;

                _lock.Enter(ref lockTaken);

                Logger.Trace("Lock entered for reservoir update");

                var newCount = _count.GetValue();
                newCount++;
                _count.SetValue(newCount);
                _sum.Add(value);

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
                    Logger.Trace("Lock exited after updating {Reservoir}", this);
                }

                Logger.Trace("{Reservoir} updated", this);
            }
        }

        private sealed class ReverseOrderDoubleComparer : IComparer<double>
        {
            public static readonly IComparer<double> Instance = new ReverseOrderDoubleComparer();

            public int Compare(double x, double y) { return y.CompareTo(x); }
        }
    }
}