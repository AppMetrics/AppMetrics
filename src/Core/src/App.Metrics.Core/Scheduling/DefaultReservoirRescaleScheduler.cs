// <copyright file="DefaultReservoirRescaleScheduler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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
    /// <summary>
    /// A scheduler for an <see cref="IRescalingReservoir"/> that uses a fixed time period schedule for rescaling.
    /// </summary>
    public class DefaultReservoirRescaleScheduler : IReservoirRescaleScheduler
    {
        /// <summary>
        /// Default rescaling period for the <see cref="DefaultReservoirRescaleScheduler"/> (one hour).
        /// </summary>
        public static readonly TimeSpan DefaultRescalePeriod = TimeSpan.FromHours(1);

        private static readonly ILog Logger = LogProvider.For<DefaultReservoirRescaleScheduler>();
        private static readonly Stopwatch TickStopWatch = new Stopwatch();
        private readonly object _syncLock = new object();
        private readonly ConcurrentBag<IRescalingReservoir> _reservoirs = new ConcurrentBag<IRescalingReservoir>();
        private readonly IMetricsTaskSchedular _scheduler;
        private readonly TimeSpan _rescalePeriod;
        private volatile bool _disposing;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultReservoirRescaleScheduler"/> class.
        /// <see cref="DefaultRescalePeriod"/> is used as the rescaling period.
        /// </summary>
        public DefaultReservoirRescaleScheduler()
            : this(DefaultRescalePeriod)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultReservoirRescaleScheduler"/> class.
        /// </summary>
        /// <param name="rescalePeriod">Rescaling period.</param>
        public DefaultReservoirRescaleScheduler(TimeSpan rescalePeriod)
            : this(rescalePeriod, null)
        {
        }

        internal DefaultReservoirRescaleScheduler(TimeSpan rescalePeriod, IMetricsTaskSchedular scheduler)
        {
            if (rescalePeriod < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(rescalePeriod));
            }

            _scheduler = scheduler ?? new DefaultMetricsTaskSchedular();
            _scheduler.SetTaskSource(cToken => Rescale());
            _rescalePeriod = rescalePeriod;

            SetScheduler();
        }

        /// <summary>
        /// Default instance of the <see cref="DefaultReservoirRescaleScheduler"/>, using <see cref="DefaultRescalePeriod"/> as rescaling period.
        /// </summary>
        public static DefaultReservoirRescaleScheduler Instance { get; } = new DefaultReservoirRescaleScheduler();

        /// <summary>
        /// Returns the rescaling period used by the <see cref="DefaultReservoirRescaleScheduler"/>.
        /// </summary>
        public TimeSpan RescalePeriod { get => _rescalePeriod; }

        /// <summary>
        /// Removes given reservoir from the <see cref="DefaultReservoirRescaleScheduler"/>.
        /// </summary>
        /// <param name="reservoir"><see cref="IRescalingReservoir"/> to remove from the scheduler.</param>
        /// <remarks>Removing a reservoir from the scheduler means the scheduler will not initiate any rescaling operations on that reservoir anymore.</remarks>
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

        /// <summary>
        /// Adds given reservoir to the <see cref="DefaultReservoirRescaleScheduler"/>.
        /// </summary>
        /// <param name="reservoir">Reservoir to add.</param>
        /// <remarks>Adding a reservoir to the scheduler causes the scheduler to periodically initiate a rescaling operation on that reservoir.</remarks>
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
            Logger.Trace("Starting {ReservoirRescaleScheduler} Schedule with period {RescalePeriod}", this, _rescalePeriod);
            _scheduler?.Start(_rescalePeriod);
            Logger.Trace("{ReservoirRescaleScheduler} Schedule Started", this);
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
                    Logger.Trace("{ReservoirCount} reservoirs all rescaled in {ElapsedTicks} ticks use {ReservoirRescaleScheduler} ", _reservoirs.Count, TickStopWatch.ElapsedTicks, this);
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