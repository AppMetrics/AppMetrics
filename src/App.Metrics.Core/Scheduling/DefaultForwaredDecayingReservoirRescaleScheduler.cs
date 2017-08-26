// <copyright file="DefaultForwaredDecayingReservoirRescaleScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using App.Metrics.Logging;
using App.Metrics.ReservoirSampling.ExponentialDecay;

namespace App.Metrics.Scheduling
{
    public class DefaultForwaredDecayingReservoirRescaleScheduler
    {
        private static readonly ILog Logger = LogProvider.For<DefaultForwaredDecayingReservoirRescaleScheduler>();
        private static readonly TimeSpan TickInterval = TimeSpan.FromHours(1);
        private readonly ConcurrentBag<DefaultForwardDecayingReservoir> _reservoirs = new ConcurrentBag<DefaultForwardDecayingReservoir>();
        private IScheduler _scheduler;

        private DefaultForwaredDecayingReservoirRescaleScheduler()
        {
            _scheduler = new DefaultTaskScheduler();
            _scheduler.Interval(TickInterval, TaskCreationOptions.LongRunning, Rescale);
        }

        public static DefaultForwaredDecayingReservoirRescaleScheduler Instance { get; } = new DefaultForwaredDecayingReservoirRescaleScheduler();

        public void RemoveSchedule(DefaultForwardDecayingReservoir reservoir)
        {
            if (reservoir != null)
            {
                Logger.Debug(
                    _reservoirs.TryTake(out reservoir)
                        ? "Successfully removed reservoir rescale schedule."
                        : "Failed to remove reservoir rescale schedule.");
            }
        }

        public void ScheduleReScaling(DefaultForwardDecayingReservoir reservoir) { _reservoirs.Add(reservoir); }

        public DefaultForwaredDecayingReservoirRescaleScheduler WithScheduler(IScheduler scheduler)
        {
            _scheduler.Dispose();
            _scheduler = scheduler;
            _scheduler.Interval(TickInterval, TaskCreationOptions.LongRunning, Rescale);

            return this;
        }

        private void Rescale()
        {
            var sw = new Stopwatch();
            sw.Start();

            foreach (var reservoir in _reservoirs)
            {
                reservoir.Rescale();
            }

            Logger.Debug("{ReservoirCount} reservoirs all rescaled in {ElapsedTicks} ticks", _reservoirs.Count, sw.ElapsedTicks);
        }
    }
}