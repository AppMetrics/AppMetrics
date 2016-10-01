using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public interface MeterImplementation : Meter, MetricValueProvider<MeterValue>
    {
    }

    public sealed class MeterMetric : SimpleMeter, MeterImplementation, IDisposable
    {
        private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);

        private readonly Clock _clock;
        private readonly Scheduler _tickScheduler;

        private ConcurrentDictionary<string, SimpleMeter> _setMeters;

        private long _startTime;

        public MeterMetric()
            : this(Clock.Default, new ActionScheduler())
        {
        }

        public MeterMetric(Clock clock, Scheduler scheduler)
        {
            _clock = clock;
            _startTime = _clock.Nanoseconds;
            _tickScheduler = scheduler;
            _tickScheduler.Start(TickInterval, (Action)Tick);
        }

        public MeterValue Value
        {
            get { return GetValue(); }
        }

        public void Dispose()
        {
            _tickScheduler.Stop();
            using (_tickScheduler)
            {
            }

            if (_setMeters != null)
            {
                _setMeters.Clear();
                _setMeters = null;
            }
        }

        public MeterValue GetValue(bool resetMetric = false)
        {
            if (_setMeters == null || _setMeters.Count == 0)
            {
                double elapsed = (_clock.Nanoseconds - _startTime);
                var value = base.GetValue(elapsed);
                if (resetMetric)
                {
                    Reset();
                }
                return value;
            }

            return GetValueWithSetItems(resetMetric);
        }

        public void Mark()
        {
            Mark(1L);
        }

        public new void Mark(long count)
        {
            base.Mark(count);
        }

        public void Mark(string item)
        {
            Mark(item, 1L);
        }

        public void Mark(string item, long count)
        {
            Mark(count);

            if (item == null)
            {
                return;
            }

            if (_setMeters == null)
            {
                Interlocked.CompareExchange(ref _setMeters, new ConcurrentDictionary<string, SimpleMeter>(), null);
            }

            Debug.Assert(_setMeters != null);
            _setMeters.GetOrAdd(item, v => new SimpleMeter()).Mark(count);
        }

        public new void Reset()
        {
            _startTime = _clock.Nanoseconds;
            base.Reset();
            if (_setMeters != null)
            {
                foreach (var meter in _setMeters.Values)
                {
                    meter.Reset();
                }
            }
        }

        private MeterValue GetValueWithSetItems(bool resetMetric)
        {
            double elapsed = _clock.Nanoseconds - _startTime;
            var value = base.GetValue(elapsed);

            Debug.Assert(_setMeters != null);

            var items = new MeterValue.SetItem[_setMeters.Count];
            var index = 0;

            foreach (var meter in _setMeters)
            {
                var itemValue = meter.Value.GetValue(elapsed);
                var percent = value.Count > 0 ? itemValue.Count / (double)value.Count * 100 : 0.0;
                items[index++] = new MeterValue.SetItem(meter.Key, percent, itemValue);
                if (index == items.Length)
                {
                    break;
                }
            }

            Array.Sort(items, MeterValue.SetItemComparer);
            var result = new MeterValue(value.Count, value.MeanRate, value.OneMinuteRate, value.FiveMinuteRate, value.FifteenMinuteRate,
                TimeUnit.Seconds, items);
            if (resetMetric)
            {
                Reset();
            }
            return result;
        }

        private new void Tick()
        {
            base.Tick();
            if (_setMeters != null)
            {
                foreach (var value in _setMeters.Values)
                {
                    value.Tick();
                }
            }
        }
    }
}