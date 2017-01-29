// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Options;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Timer.Abstractions;

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
                             () => _timerBuilder.Build(options.Reservoir, _clock))
                         .NewContext())
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
                             () => _timerBuilder.Build(options.Reservoir, _clock))
                         .NewContext(userValue))
            {
                action();
            }
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options)
        {
            return
                _registry.Timer(
                             options,
                             () => _timerBuilder.Build(options.Reservoir, _clock))
                         .NewContext();
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options, string userValue)
        {
            return _registry.Timer(
                                options,
                                () => _timerBuilder.Build(
                                    options.Reservoir,
                                    _clock))
                            .NewContext(userValue);
        }
    }
}