// <copyright file="DefaultForwaredDecayingReservoirRescaleScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Scheduling.Abstractions;

namespace App.Metrics.Scheduling
{
    public class DefaultForwaredDecayingReservoirRescaleScheduler
    {
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
                _reservoirs.TryTake(out reservoir);
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

            Debug.WriteLine($"Reservoirs all rescaled in {sw.ElapsedMilliseconds}ms");
        }
    }
}