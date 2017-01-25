// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Core.Options;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Registry.Interfaces;
using App.Metrics.Timer.Interfaces;

namespace App.Metrics.Timer
{
    public class DefaultTimerMetricProvider : IProvideTimerMetrics
    {
        private readonly IClock _clock;
        private readonly IMetricsRegistry _registry;
        private readonly IBuildTimerMetrics _timerBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultTimerMetricProvider" /> class.
        /// </summary>
        /// <param name="timerBuilder">The timer builder.</param>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="clock">The clock.</param>
        public DefaultTimerMetricProvider(IBuildTimerMetrics timerBuilder, IMetricsRegistry registry, IClock clock)
        {
            _registry = registry;
            _clock = clock;
            _timerBuilder = timerBuilder;
        }

        /// <inheritdoc />
        public ITimer Instance(TimerOptions options)
        {
            return Instance(
                options,
                () => _timerBuilder.Build(options.Reservoir, _clock));
        }

        /// <inheritdoc />
        public ITimer Instance<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric { return _registry.Timer(options, builder); }

        /// <inheritdoc />
        public ITimer WithHistogram<T>(TimerOptions options, Func<T> histogramMetricBuilder)
            where T : IHistogramMetric
        {
            return Instance(
                options,
                () => _timerBuilder.Build(histogramMetricBuilder(), _clock));
        }
    }
}