// <copyright file="DefaultTimerMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.BucketHistogram;
using App.Metrics.Concurrency;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.Scheduling;
using App.Metrics.Timer;

namespace App.Metrics.BucketTimer
{
    public sealed class DefaultBucketTimerMetric : IBucketTimerMetric, IDisposable
    {
        private readonly StripedLongAdder _activeSessionsCounter = new StripedLongAdder();
        private readonly IClock _clock;
        private readonly TimeUnit _timeUnit;
        private readonly IBucketHistogramMetric _histogram;
        private readonly IMeterMetric _meter;
        private bool _disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultBucketTimerMetric" /> class.
        /// </summary>
        /// <param name="histogram">The histogram implementation to use.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        /// <param name="timeUnit">The time unit for this timer.</param>
        public DefaultBucketTimerMetric(IBucketHistogramMetric histogram, IClock clock, TimeUnit timeUnit)
        {
            _clock = clock;
            _timeUnit = timeUnit;
            _histogram = histogram;
            _meter = new DefaultMeterMetric(clock);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultBucketTimerMetric" /> class.
        /// </summary>
        /// <param name="histogram">The histogram implementation to use.</param>
        /// <param name="meter">The meter implementation to use to genreate the rate of events over time.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        /// <param name="timeUnit">The time unit for this timer.</param>
        public DefaultBucketTimerMetric(IBucketHistogramMetric histogram, IMeterMetric meter, IClock clock, TimeUnit timeUnit)
        {
            _clock = clock;
            _timeUnit = timeUnit;
            _meter = meter;
            _histogram = histogram;
        }

        /// <inheritdoc />
        public BucketTimerValue Value => GetValue();

        /// <inheritdoc />
        public long CurrentTime()
        {
            return _clock.Nanoseconds;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        // ReSharper disable MemberCanBePrivate.Global
        public void Dispose(bool disposing)
            // ReSharper restore MemberCanBePrivate.Global
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    _histogram?.Dispose();
                    _meter?.Dispose();
                }
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public long EndRecording()
        {
            _activeSessionsCounter.Decrement();
            return _clock.Nanoseconds;
        }

        /// <inheritdoc />
        public BucketTimerValue GetValue(bool resetMetric = false)
        {
            return new BucketTimerValue(
                _meter.GetValue(resetMetric),
                _histogram.GetValue(resetMetric),
                _activeSessionsCounter.GetValue(),
                _timeUnit);
        }

        /// <inheritdoc />
        public TimerContext NewContext(string userValue)
        {
            return new TimerContext(this, userValue);
        }

        /// <inheritdoc />
        public TimerContext NewContext()
        {
            return NewContext(null);
        }

        /// <inheritdoc />
        public void Record(long duration, TimeUnit unit, string userValue)
        {
            var time = unit.Convert(_timeUnit, duration);
            if (time < 0)
            {
                return;
            }

            _histogram.Update(time, userValue);
            _meter.Mark();
        }

        /// <inheritdoc />
        public void Record(long time, TimeUnit unit)
        {
            Record(time, unit, null);
        }

        /// <inheritdoc />
        public void Reset()
        {
            _meter.Reset();
            _histogram.Reset();
        }

        /// <inheritdoc />
        public long StartRecording()
        {
            _activeSessionsCounter.Increment();
            return _clock.Nanoseconds;
        }

        /// <inheritdoc />
        public void Time(Action action, string userValue)
        {
            var start = _clock.Nanoseconds;
            try
            {
                _activeSessionsCounter.Increment();
                action();
            }
            finally
            {
                _activeSessionsCounter.Decrement();
                Record(_clock.Nanoseconds - start, TimeUnit.Nanoseconds, userValue);
            }
        }

        /// <inheritdoc />
        public T Time<T>(Func<T> action, string userValue)
        {
            var start = _clock.Nanoseconds;
            try
            {
                _activeSessionsCounter.Increment();
                return action();
            }
            finally
            {
                _activeSessionsCounter.Decrement();
                Record(_clock.Nanoseconds - start, TimeUnit.Nanoseconds, userValue);
            }
        }

        /// <inheritdoc />
        public void Time(Action action)
        {
            Time(action, null);
        }

        /// <inheritdoc />
        public T Time<T>(Func<T> action)
        {
            return Time(action, null);
        }
    }
}