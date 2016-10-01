using System;
using App.Metrics.App_Packages.Concurrency;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public interface TimerImplementation : Timer, MetricValueProvider<TimerValue>
    {
    }

    public sealed class TimerMetric : TimerImplementation, IDisposable
    {
        private readonly StripedLongAdder _activeSessionsCounter = new StripedLongAdder();
        private readonly Clock _clock;
        private readonly HistogramImplementation _histogram;
        private readonly MeterImplementation _meter;
        private readonly StripedLongAdder _totalRecordedTime = new StripedLongAdder();

        public TimerMetric()
            : this(new HistogramMetric(), new MeterMetric(), Clock.Default)
        {
        }

        public TimerMetric(SamplingType samplingType)
            : this(new HistogramMetric(samplingType), new MeterMetric(), Clock.Default)
        {
        }

        public TimerMetric(HistogramImplementation histogram)
            : this(histogram, new MeterMetric(), Clock.Default)
        {
        }

        public TimerMetric(Reservoir reservoir)
            : this(new HistogramMetric(reservoir), new MeterMetric(), Clock.Default)
        {
        }

        public TimerMetric(SamplingType samplingType, MeterImplementation meter, Clock clock)
            : this(new HistogramMetric(samplingType), meter, clock)
        {
        }

        public TimerMetric(HistogramImplementation histogram, MeterImplementation meter, Clock clock)
        {
            _clock = clock;
            _meter = meter;
            _histogram = histogram;
        }

        public TimerValue Value
        {
            get { return GetValue(); }
        }

        public long CurrentTime()
        {
            return _clock.Nanoseconds;
        }

        public void Dispose()
        {
            using (_histogram as IDisposable)
            {
            }
            using (_meter as IDisposable)
            {
            }
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
            if (nanos >= 0)
            {
                _histogram.Update(nanos, userValue);
                _meter.Mark(userValue);
                _totalRecordedTime.Add(nanos);
            }
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