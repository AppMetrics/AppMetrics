// <copyright file="DefaultMeterTickerScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using App.Metrics.Logging;
using App.Metrics.Meter;

namespace App.Metrics.Scheduling
{
    public class DefaultMeterTickerScheduler
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMeterTickerScheduler>();
        private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);
        private readonly ConcurrentBag<SimpleMeter> _meters = new ConcurrentBag<SimpleMeter>();
        private IScheduler _scheduler;

        private DefaultMeterTickerScheduler()
        {
            _scheduler = new DefaultTaskScheduler();
            _scheduler.Interval(TickInterval, TaskCreationOptions.LongRunning, Tick);
        }

        public static DefaultMeterTickerScheduler Instance { get; } = new DefaultMeterTickerScheduler();

        public void ScheduleTick(SimpleMeter meter) { _meters.Add(meter); }

        public DefaultMeterTickerScheduler WithScheduler(IScheduler scheduler)
        {
            _scheduler.Dispose();
            _scheduler = scheduler;
            _scheduler.Interval(TickInterval, TaskCreationOptions.LongRunning, Tick);

            return this;
        }

        public void RemoveSchedule(SimpleMeter meter)
        {
            if (meter != null)
            {
                Logger.Debug(
                    _meters.TryTake(out meter)
                        ? "Successfully removed meter tick schedule."
                        : "Failed to remove meter tick schedule.");
            }
        }

        private void Tick()
        {
            var sw = new Stopwatch();
            sw.Start();

            foreach (var meter in _meters)
            {
                meter.Tick();
            }

            Logger.Debug("{MeterCount} meters all ticked in {ElapsedTicks} ticks", _meters.Count, sw.ElapsedTicks);
        }
    }
}