// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using App.Metrics.App_Packages.Concurrency;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class TimerMetric : ITimerMetric, IDisposable
    {
        private readonly StripedLongAdder _activeSessionsCounter = new StripedLongAdder();
        private bool _disposed = false;
        private readonly IClock _clock;
        private readonly IHistogramMetric _histogram;
        private readonly IMeterMetric _meter;
        private readonly StripedLongAdder _totalRecordedTime = new StripedLongAdder();

        public TimerMetric(SamplingType samplingType, IClock systemClock)
            : this(new HistogramMetric(samplingType), new MeterMetric(systemClock), systemClock)
        {
        }

        public TimerMetric(IHistogramMetric histogram, IClock systemClock)
            : this(histogram, new MeterMetric(systemClock), systemClock)
        {
        }

        public TimerMetric(IReservoir reservoir, IClock systemClock)
            : this(new HistogramMetric(reservoir), new MeterMetric(systemClock), systemClock)
        {
        }

        public TimerMetric(SamplingType samplingType, IMeterMetric meter, IClock clock)
            : this(new HistogramMetric(samplingType), meter, clock)
        {
        }

        public TimerMetric(IHistogramMetric histogram, IMeterMetric meter, IClock clock)
        {
            _clock = clock;
            _meter = meter;
            _histogram = histogram;
        }

        public TimerValue Value => GetValue();

        public long CurrentTime()
        {
            return _clock.Nanoseconds;
        }

        ~TimerMetric()
        {
            Dispose(false);
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

        public long EndRecording()
        {
            _activeSessionsCounter.Decrement();
            return _clock.Nanoseconds;
        }

        public TimerValue GetValue(bool resetMetric = false)
        {
            var total = resetMetric ? _totalRecordedTime.GetAndReset() : _totalRecordedTime.GetValue();
            return new TimerValue(_meter.GetValue(resetMetric), _histogram.GetValue(resetMetric), _activeSessionsCounter.GetValue(), total,
                TimeUnit.Nanoseconds);
        }

        public TimerContext NewContext(string userValue = null)
        {
            return new TimerContext(this, userValue);
        }

        public void Record(long duration, TimeUnit unit, string userValue = null)
        {
            var nanos = unit.ToNanoseconds(duration);
            if (nanos < 0) return;

            _histogram.Update(nanos, userValue);
            _meter.Mark(userValue);
            _totalRecordedTime.Add(nanos);
        }

        public void Reset()
        {
            _meter.Reset();
            _histogram.Reset();
        }

        public long StartRecording()
        {
            _activeSessionsCounter.Increment();
            return _clock.Nanoseconds;
        }

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