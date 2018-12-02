// <copyright file="TestReservoirRescaleScheduler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Internal;
using App.Metrics.Logging;
using App.Metrics.ReservoirSampling;
using App.Metrics.Scheduling;

namespace App.Metrics.FactsCommon
{
    public class TestReservoirRescaleScheduler : IReservoirRescaleScheduler
    {
        private static readonly ILog Logger = LogProvider.For<DefaultReservoirRescaleScheduler>();
        private static readonly TimeSpan DefaultTickInterval = TimeSpan.FromHours(1);
        private readonly object _syncLock = new object();
        private readonly ConcurrentBag<IRescalingReservoir> _reservoirs = new ConcurrentBag<IRescalingReservoir>();
        private long _lastRun;
        private volatile bool _disposing;

        public TestReservoirRescaleScheduler(IClock clock)
            : this(clock, DefaultTickInterval)
        { }

        public TestReservoirRescaleScheduler(IClock clock, TimeSpan tickInterval)
        {
            clock.Advanced += (s, l) =>
            {
                try
                {
                    var clockSeconds = clock.Seconds;
                    var elapsed = clockSeconds - _lastRun;
                    var times = elapsed / tickInterval.TotalSeconds;

                    using (new CancellationTokenSource())
                    {
                        while (times-- >= 1)
                        {
                            _lastRun = clockSeconds;

                            Rescale().GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };
        }

        public void RemoveSchedule(IRescalingReservoir reservoir)
        {
            if (reservoir != null)
            {
                Logger.Trace(
                    _reservoirs.TryTake(out reservoir)
                        ? "Successfully removed reservoir from {ReservoirRescaleScheduler} schedule."
                        : "Failed to remove reservoir from {ReservoirRescaleScheduler} schedule.", this);
            }
        }

        public void ScheduleReScaling(IRescalingReservoir reservoir) { _reservoirs.Add(reservoir); }

        /// <inheritdoc/>
        public void Dispose()
        {
            Logger.Trace("Disposing {ReservoirRescaleScheduler}", this);

            lock (_syncLock)
            {
                if (_disposing)
                {
                    return;
                }

                _disposing = true;
            }

            Logger.Trace("{ReservoirRescaleScheduler} Disposed", this);

            Rescale().GetAwaiter().GetResult();
        }

        private void SetScheduler()
        {
            Logger.Trace("Starting {ReservoirRescaleScheduler} Schedule", this);
            Logger.Trace("{ReservoirRescaleScheduler} Schedule Started", this);
        }

        private Task Rescale()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();

                foreach (var reservoir in _reservoirs)
                {
                    reservoir.Rescale();
                }

                Logger.Trace("{ReservoirCount} reservoirs all rescaled in {ElapsedTicks} ticks use {ReservoirRescaleScheduler} ", _reservoirs.Count, sw.ElapsedTicks, this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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