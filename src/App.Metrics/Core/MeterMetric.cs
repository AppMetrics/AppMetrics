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

        private readonly Clock clock;
        private readonly Scheduler tickScheduler;

        private ConcurrentDictionary<string, SimpleMeter> setMeters;

        private long startTime;

        public MeterMetric()
            : this(Clock.Default, new ActionScheduler())
        {
        }

        public MeterMetric(Clock clock, Scheduler scheduler)
        {
            this.clock = clock;
            this.startTime = this.clock.Nanoseconds;
            this.tickScheduler = scheduler;
            this.tickScheduler.Start(TickInterval, (Action)Tick);
        }

        public MeterValue Value
        {
            get { return GetValue(); }
        }

        public void Dispose()
        {
            this.tickScheduler.Stop();
            using (this.tickScheduler)
            {
            }

            if (this.setMeters != null)
            {
                this.setMeters.Clear();
                this.setMeters = null;
            }
        }

        public MeterValue GetValue(bool resetMetric = false)
        {
            if (this.setMeters == null || this.setMeters.Count == 0)
            {
                double elapsed = (this.clock.Nanoseconds - this.startTime);
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

            if (this.setMeters == null)
            {
                Interlocked.CompareExchange(ref this.setMeters, new ConcurrentDictionary<string, SimpleMeter>(), null);
            }

            Debug.Assert(this.setMeters != null);
            this.setMeters.GetOrAdd(item, v => new SimpleMeter()).Mark(count);
        }

        public new void Reset()
        {
            this.startTime = this.clock.Nanoseconds;
            base.Reset();
            if (this.setMeters != null)
            {
                foreach (var meter in this.setMeters.Values)
                {
                    meter.Reset();
                }
            }
        }

        private MeterValue GetValueWithSetItems(bool resetMetric)
        {
            double elapsed = this.clock.Nanoseconds - this.startTime;
            var value = base.GetValue(elapsed);

            Debug.Assert(this.setMeters != null);

            var items = new MeterValue.SetItem[this.setMeters.Count];
            var index = 0;

            foreach (var meter in this.setMeters)
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
            if (this.setMeters != null)
            {
                foreach (var value in this.setMeters.Values)
                {
                    value.Tick();
                }
            }
        }
    }
}