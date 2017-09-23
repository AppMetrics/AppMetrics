// <copyright file="DefaultReservoirRescaleScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using App.Metrics.Internal;
using App.Metrics.Logging;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Scheduling
{
    public class DefaultReservoirRescaleScheduler : IReservoirRescaleScheduler
    {
        private static readonly ILog Logger = LogProvider.For<DefaultReservoirRescaleScheduler>();
        private static readonly TimeSpan TickInterval = TimeSpan.FromHours(1);
        private static readonly Stopwatch TickStopWatch = new Stopwatch();
        private readonly object _syncLock = new object();
        private readonly ConcurrentBag<IRescalingReservoir> _reservoirs = new ConcurrentBag<IRescalingReservoir>();
        private readonly IMetricsTaskSchedular _scheduler;
        private volatile bool _disposing;

        private DefaultReservoirRescaleScheduler()
        {
            _scheduler = new DefaultMetricsTaskSchedular(c => Rescale());

            SetScheduler();
        }

        public static DefaultReservoirRescaleScheduler Instance { get; } = new DefaultReservoirRescaleScheduler();

        public void RemoveSchedule(IRescalingReservoir reservoir)
        {
            if (reservoir != null)
            {
                Logger.Debug(
                    _reservoirs.TryTake(out reservoir)
                        ? "Successfully removed reservoir from {ReservoirRescaleScheduler} schedule."
                        : "Failed to remove reservoir from {ReservoirRescaleScheduler} schedule.", this);
            }
        }

        public void ScheduleReScaling(IRescalingReservoir reservoir)
        {
            _reservoirs.Add(reservoir);
        }

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

            _scheduler?.Dispose();

            Logger.Trace("{ReservoirRescaleScheduler} Disposed", this);

            Rescale().GetAwaiter().GetResult();
        }

        private void SetScheduler()
        {
            Logger.Debug("Starting {ReservoirRescaleScheduler} Schedule", this);
            _scheduler?.Start(TickInterval);
            Logger.Debug("{ReservoirRescaleScheduler} Schedule Started", this);
        }

        private Task Rescale()
        {
            try
            {
                if (Logger.IsDebugEnabled())
                {
                    TickStopWatch.Start();
                }

                foreach (var reservoir in _reservoirs)
                {
                    reservoir.Rescale();
                }

                if (Logger.IsDebugEnabled())
                {
                    Logger.Debug("{ReservoirCount} reservoirs all rescaled in {ElapsedTicks} ticks use {ReservoirRescaleScheduler} ", _reservoirs.Count, TickStopWatch.ElapsedTicks, this);
                    TickStopWatch.Reset();
                    TickStopWatch.Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while attempting to rescale all rescalable reservoirs in {0}: {1}", this, ex);
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