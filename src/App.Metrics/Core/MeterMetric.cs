// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class MeterMetric : SimpleMeter, IMeterImplementation
    {
        private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);
        private readonly IClock _clock;
        private readonly IScheduler _tickScheduler;
        private bool _disposed = false;
        private ConcurrentDictionary<string, SimpleMeter> _setMeters;
        private long _startTime;

        public MeterMetric(IClock systemClock)
            : this(systemClock, new ActionScheduler())
        {
        }

        public MeterMetric(IClock clock, IScheduler scheduler)
        {
            _clock = clock;
            _startTime = _clock.Nanoseconds;
            _tickScheduler = scheduler;
            _tickScheduler.Start(TickInterval, (Action)Tick);
        }

        ~MeterMetric()
        {
            Dispose(false);
        }

        public MeterValue Value => GetValue();

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