// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Scheduling;
using App.Metrics.Scheduling.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Meter
{
    public sealed class DefaultMeterMetric : SimpleMeter, IMeterMetric
    {
        private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);
        private readonly IClock _clock;
        private readonly IScheduler _tickScheduler;
        private bool _disposed;
        private ConcurrentDictionary<string, SimpleMeter> _setMeters;
        private long _startTime;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMeterMetric" /> class.
        /// </summary>
        /// <param name="systemClock">The system clock.</param>
        public DefaultMeterMetric(IClock systemClock)
            : this(systemClock, new DefaultTaskScheduler())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMeterMetric" /> class.
        /// </summary>
        /// <param name="clock">The clock.</param>
        /// <param name="scheduler">The scheduler.</param>
        public DefaultMeterMetric(IClock clock, IScheduler scheduler)
        {
            _clock = clock;
            _startTime = _clock.Nanoseconds;
            _tickScheduler = scheduler;
            _tickScheduler.Interval(TickInterval, TaskCreationOptions.LongRunning, Tick);
        }

        /// <inheritdoc />
        public MeterValue Value => GetValue();

        /// <inheritdoc />
        public override void Reset()
        {
            _startTime = _clock.Nanoseconds;
            base.Reset();
            if (_setMeters == null)
            {
                return;
            }

            foreach (var meter in _setMeters.Values)
            {
                meter.Reset();
            }
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
                    if (_tickScheduler != null)
                    {
                        _tickScheduler.Stop();
                        _tickScheduler.Dispose();
                    }

                    if (_setMeters != null)
                    {
                        _setMeters.Clear();
                        _setMeters = null;
                    }
                }
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public MeterValue GetValue(bool resetMetric = false)
        {
            if (_setMeters == null || _setMeters.Count == 0)
            {
                double elapsed = _clock.Nanoseconds - _startTime;

                var value = GetValue(elapsed);

                if (resetMetric)
                {
                    Reset();
                }

                return value;
            }

            return GetValueWithSetItems(resetMetric);
        }

        /// <inheritdoc />
        public void Mark()
        {
            Mark(1L);
        }

        /// <inheritdoc />
        public void Mark(string item)
        {
            Mark(item, 1L);
        }

        /// <inheritdoc />
        public void Mark(MetricSetItem setItem)
        {
            Mark(setItem.ToString());
        }

        /// <inheritdoc />
        public void Mark(MetricSetItem setItem, long amount)
        {
            Mark(setItem.ToString(), amount);
        }

        /// <inheritdoc />
        public void Mark(string item, long amount)
        {
            Mark(amount);

            if (item == null)
            {
                return;
            }

            if (_setMeters == null)
            {
                Interlocked.CompareExchange(ref _setMeters, new ConcurrentDictionary<string, SimpleMeter>(), null);
            }

            Debug.Assert(_setMeters != null, "set meters not null");

            _setMeters.GetOrAdd(item, v => new SimpleMeter()).Mark(amount);
        }

        private MeterValue GetValueWithSetItems(bool resetMetric)
        {
            double elapsed = _clock.Nanoseconds - _startTime;

            var value = GetValue(elapsed);

            Debug.Assert(_setMeters != null, "set meters not null");

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

            var result = new MeterValue(
                value.Count,
                value.MeanRate,
                value.OneMinuteRate,
                value.FiveMinuteRate,
                value.FifteenMinuteRate,
                TimeUnit.Seconds,
                items);

            if (resetMetric)
            {
                Reset();
            }

            return result;
        }

        private new void Tick()
        {
            base.Tick();
            if (_setMeters == null)
            {
                return;
            }

            foreach (var value in _setMeters.Values)
            {
                value.Tick();
            }
        }
    }
}