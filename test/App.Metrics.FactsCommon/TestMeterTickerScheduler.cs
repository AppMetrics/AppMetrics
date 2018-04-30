// <copyright file="TestMeterTickerScheduler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Internal;
using App.Metrics.Logging;
using App.Metrics.Meter;
using App.Metrics.Scheduling;

namespace App.Metrics.FactsCommon
{
    public class TestMeterTickerScheduler : IMeterTickerScheduler
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMeterTickerScheduler>();
        private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);
        private readonly object _syncLock = new object();
        private readonly ConcurrentBag<ITickingMeter> _meters = new ConcurrentBag<ITickingMeter>();
        private long _lastRun;
        private volatile bool _disposing;

        public TestMeterTickerScheduler(IClock clock)
        {
            clock.Advanced += (s, l) =>
            {
                try
                {
                    var clockSeconds = clock.Seconds;
                    var elapsed = clockSeconds - _lastRun;
                    var times = elapsed / TickInterval.TotalSeconds;

                    using (new CancellationTokenSource())
                    {
                        while (times-- >= 1)
                        {
                            _lastRun = clockSeconds;

                            Tick().GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };
        }

        public void ScheduleTick(ITickingMeter meter) { _meters.Add(meter); }

        public void RemoveSchedule(ITickingMeter meter)
        {
            if (meter != null)
            {
                Logger.Trace(
                    _meters.TryTake(out meter)
                        ? "Successfully removed meter from {MeterTickScheuler} schedule."
                        : "Failed to remove meter from {MeterTickScheuler} schedule.", this);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Logger.Trace("Disposing {MeterTickScheuler} scheduler", this);

            lock (_syncLock)
            {
                if (_disposing)
                {
                    return;
                }

                _disposing = true;
            }

            Logger.Trace("{MeterTickScheuler} scheduler disposed", this);

            Tick().GetAwaiter().GetResult();
        }

        private void SetScheduler()
        {
            Logger.Trace("Starting {MeterTickScheuler} scheduler", this);
            Logger.Trace("{MeterTickScheuler} scheduler started", this);
        }

        private Task Tick()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();

                foreach (var meter in _meters)
                {
                    meter.Tick();
                }

                Logger.Trace("{MeterCount} meters all ticked in {ElapsedTicks} ticks using {MeterTickScheuler}", _meters.Count, sw.ElapsedTicks, this);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while attempting to tick all meters in {0}: {1}", this, ex);
            }
            finally
            {
                lock (_syncLock)
                {
                    if (!_disposing)
                    {
                        SetScheduler();
                    }
                }
            }

            return AppMetricsTaskHelper.CompletedTask();
        }
    }
}