// <copyright file="DefaultTimerMetricProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;
using App.Metrics.Registry;

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
        public ITimer Instance(TimerOptions options, MetricTags tags)
        {
            return Instance(
                options,
                tags,
                () => _timerBuilder.Build(options.Reservoir, _clock));
        }

        /// <inheritdoc />
        public ITimer Instance<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric
        {
            return _registry.Timer(options, builder);
        }

        public ITimer Instance<T>(TimerOptions options, MetricTags tags, Func<T> builder)
            where T : ITimerMetric
        {
            return _registry.Timer(options, tags, builder);
        }

        /// <inheritdoc />
        public ITimer WithHistogram<T>(TimerOptions options, Func<T> histogramMetricBuilder)
            where T : IHistogramMetric
        {
            return Instance(
                options,
                () => _timerBuilder.Build(histogramMetricBuilder(), _clock));
        }

        /// <inheritdoc />
        public ITimer WithHistogram<T>(TimerOptions options, MetricTags tags, Func<T> histogramMetricBuilder)
            where T : IHistogramMetric
        {
            return Instance(
                options,
                tags,
                () => _timerBuilder.Build(histogramMetricBuilder(), _clock));
        }
    }
}