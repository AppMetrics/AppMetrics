// <copyright file="DefaultTimerManager.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Registry;

namespace App.Metrics.Timer
{
    internal sealed class DefaultTimerManager : IMeasureTimerMetrics
    {
        private readonly IClock _clock;
        private readonly IMetricsRegistry _registry;
        private readonly IBuildTimerMetrics _timerBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultTimerManager" /> class.
        /// </summary>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="timerBuilder">The timer builder.</param>
        /// <param name="clock">The clock.</param>
        public DefaultTimerManager(IBuildTimerMetrics timerBuilder, IMetricsRegistry registry, IClock clock)
        {
            _clock = clock;
            _registry = registry;
            _timerBuilder = timerBuilder;
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, Action action)
        {
            using (
                _registry.Timer(
                              options,
                              () => _timerBuilder.Build(options.Reservoir, _clock)).
                          NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, MetricTags tags, Action action)
        {
            using (
                _registry.Timer(
                              options,
                              tags,
                              () => _timerBuilder.Build(options.Reservoir, _clock)).
                          NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, Action action, string userValue)
        {
            using (
                _registry.Timer(
                              options,
                              () => _timerBuilder.Build(options.Reservoir, _clock)).
                          NewContext(userValue))
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, MetricTags tags, Action action, string userValue)
        {
            using (
                _registry.Timer(
                              options,
                              tags,
                              () => _timerBuilder.Build(options.Reservoir, _clock)).
                          NewContext(userValue))
            {
                action();
            }
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options, MetricTags tags, string userValue)
        {
            return _registry.Timer(
                                 options,
                                 tags,
                                 () => _timerBuilder.Build(
                                     options.Reservoir,
                                     _clock)).
                             NewContext(userValue);
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options, MetricTags tags)
        {
            return
                _registry.Timer(
                              options,
                              tags,
                              () => _timerBuilder.Build(options.Reservoir, _clock)).
                          NewContext();
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options)
        {
            return
                _registry.Timer(
                              options,
                              () => _timerBuilder.Build(options.Reservoir, _clock)).
                          NewContext();
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options, string userValue)
        {
            return _registry.Timer(
                                 options,
                                 () => _timerBuilder.Build(
                                     options.Reservoir,
                                     _clock)).
                             NewContext(userValue);
        }

        public void Time(TimerOptions options, long time)
        {
            var context = _registry.Timer(options,
                                () => _timerBuilder.Build(
                                     options.Reservoir,
                                     _clock));
            context.Record(time, options.DurationUnit);
        }

        public void Time(TimerOptions options, MetricTags tags, long time)
        {
            var context = _registry.Timer(options, tags, 
                                () => _timerBuilder.Build(
                                     options.Reservoir,
                                     _clock));
            context.Record(time, options.DurationUnit);
        }
    }
}