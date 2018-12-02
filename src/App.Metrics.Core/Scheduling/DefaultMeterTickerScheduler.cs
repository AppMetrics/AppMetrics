// <copyright file="DefaultMeterTickerScheduler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using App.Metrics.Internal;
using App.Metrics.Logging;
using App.Metrics.Meter;

namespace App.Metrics.Scheduling
{
    public class DefaultMeterTickerScheduler : IMeterTickerScheduler
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMeterTickerScheduler>();
        private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);
        private static readonly Stopwatch TickStopWatch = new Stopwatch();
        private readonly object _syncLock = new object();
        private readonly ConcurrentBag<ITickingMeter> _meters = new ConcurrentBag<ITickingMeter>();
        private readonly IMetricsTaskSchedular _scheduler;
        private volatile bool _disposing;

        private DefaultMeterTickerScheduler()
        {
            _scheduler = new DefaultMetricsTaskSchedular(c => Tick());

            SetScheduler();
        }

        public static DefaultMeterTickerScheduler Instance { get; } = new DefaultMeterTickerScheduler();

        public void ScheduleTick(ITickingMeter meter) { _meters.Add(meter); }

        public void RemoveSchedule(ITickingMeter meter)
        {
            if (meter != null)
            {
                Logger.Trace(
                    _meters.TryTake(out meter)
                        ? "Successfully removed meter from {MeterTickScheduler} schedule."
                        : "Failed to remove meter from {MeterTickScheduler} schedule.", this);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Logger.Trace("Disposing {MeterTickScheduler} scheduler", this);

            lock (_syncLock)
            {
                if (_disposing)
                {
                    return;
                }

                _disposing = true;
            }

            _scheduler.Dispose();

            Logger.Trace("{MeterTickScheduler} scheduler disposed", this);

            Tick().GetAwaiter().GetResult();
        }

        private void SetScheduler()
        {
            Logger.Trace("Starting {MeterTickScheduler} scheduler", this);
            _scheduler.Start(TickInterval);
            Logger.Trace("{MeterTickScheduler} scheduler started", this);
        }

        private Task Tick()
        {
            try
            {
                if (Logger.IsDebugEnabled())
                {
                    TickStopWatch.Start();
                }

                foreach (var meter in _meters)
                {
                    meter.Tick();
                }

                if (Logger.IsDebugEnabled())
                {
                    Logger.Trace("{MeterCount} meters all ticked in {ElapsedTicks} ticks using {MeterTickScheduler}", _meters.Count, TickStopWatch.ElapsedTicks, this);
                    TickStopWatch.Reset();
                    TickStopWatch.Stop();
                }
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

#if !NETSTANDARD1_6
            return AppMetricsTaskHelper.CompletedTask();
#else
            return Task.CompletedTask;
#endif
        }
    }
}