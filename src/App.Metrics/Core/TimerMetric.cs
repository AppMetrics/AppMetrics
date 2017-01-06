// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using App.Metrics.Concurrency;
using App.Metrics.Core.Interfaces;
using App.Metrics.Data;
using App.Metrics.Sampling.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class TimerMetric : ITimerMetric, IDisposable
    {
        private readonly StripedLongAdder _activeSessionsCounter = new StripedLongAdder();
        private readonly IClock _clock;
        private readonly IHistogramMetric _histogram;
        private readonly IMeterMetric _meter;
        private readonly StripedLongAdder _totalRecordedTime = new StripedLongAdder();
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerMetric" /> class.
        /// </summary>
        /// <param name="samplingType">Type of the sampling to use to generate the resevoir of values.</param>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        public TimerMetric(SamplingType samplingType, int sampleSize, double alpha, IClock clock)
            : this(new HistogramMetric(samplingType, sampleSize, alpha), new MeterMetric(clock), clock)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimerMetric" /> class.
        /// </summary>
        /// <param name="histogram">The histogram implementation to use.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        public TimerMetric(IHistogramMetric histogram, IClock clock)
            : this(histogram, new MeterMetric(clock), clock)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimerMetric" /> class.
        /// </summary>
        /// <param name="reservoir">The reservoir implementation to use for sampling values to generate the histogram.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        public TimerMetric(IReservoir reservoir, IClock clock)
            : this(new HistogramMetric(reservoir), new MeterMetric(clock), clock)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerMetric" /> class.
        /// </summary>
        /// <param name="samplingType">Type of the sampling to use to generate the resevoir of values.</param>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir used by the histogram</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <param name="meter">The meter implementation to use to genreate the rate of events over time.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        public TimerMetric(SamplingType samplingType, int sampleSize, double alpha, IMeterMetric meter, IClock clock)
            : this(new HistogramMetric(samplingType, sampleSize, alpha), meter, clock)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimerMetric" /> class.
        /// </summary>
        /// <param name="histogram">The histogram implementation to use.</param>
        /// <param name="meter">The meter implementation to use to genreate the rate of events over time.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        public TimerMetric(IHistogramMetric histogram, IMeterMetric meter, IClock clock)
        {
            _clock = clock;
            _meter = meter;
            _histogram = histogram;
        }

        ~TimerMetric()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public TimerValue Value => GetValue();

        /// <inheritdoc />
        public long CurrentTime()
        {
            return _clock.Nanoseconds;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
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
        public TimerValue GetValue(bool resetMetric = false)
        {
            var total = resetMetric ? _totalRecordedTime.GetAndReset() : _totalRecordedTime.GetValue();
            return new TimerValue(_meter.GetValue(resetMetric), _histogram.GetValue(resetMetric), _activeSessionsCounter.GetValue(), total,
                TimeUnit.Nanoseconds);
        }

        /// <inheritdoc />
        public TimerContext NewContext(string userValue = null)
        {
            return new TimerContext(this, userValue);
        }

        /// <inheritdoc />
        public void Record(long duration, TimeUnit unit, string userValue = null)
        {
            var nanos = unit.ToNanoseconds(duration);
            if (nanos < 0) return;

            _histogram.Update(nanos, userValue);
            _meter.Mark(userValue);
            _totalRecordedTime.Add(nanos);
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
        public void Time(Action action, string userValue = null)
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
        public T Time<T>(Func<T> action, string userValue = null)
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
    }
}