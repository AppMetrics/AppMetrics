// <copyright file="DefaultTimerManager.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Registry;
using App.Metrics.Timer;

namespace App.Metrics.BucketTimer
{
    internal sealed class DefaultBucketTimerManager : IMeasureBucketTimerMetrics
    {
        private readonly IClock _clock;
        private readonly IMetricsRegistry _registry;
        private readonly IBuildBucketTimerMetrics _timerBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultBucketTimerManager" /> class.
        /// </summary>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="timerBuilder">The timer builder.</param>
        /// <param name="clock">The clock.</param>
        public DefaultBucketTimerManager(IBuildBucketTimerMetrics timerBuilder, IMetricsRegistry registry, IClock clock)
        {
            _clock = clock;
            _registry = registry;
            _timerBuilder = timerBuilder;
        }

        /// <inheritdoc />
        public void Time(BucketTimerOptions options, Action action)
        {
            using (
                _registry.BucketTimer(
                              options,
                              () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                          NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(BucketTimerOptions options, MetricTags tags, Action action)
        {
            using (
                _registry.BucketTimer(
                              options,
                              tags,
                              () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                          NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(BucketTimerOptions options, Action action, string userValue)
        {
            using (
                _registry.BucketTimer(
                              options,
                              () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                          NewContext(userValue))
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(BucketTimerOptions options, MetricTags tags, Action action, string userValue)
        {
            using (
                _registry.BucketTimer(
                              options,
                              tags,
                              () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                          NewContext(userValue))
            {
                action();
            }
        }

        /// <inheritdoc />
        public TimerContext Time(BucketTimerOptions options, MetricTags tags, string userValue)
        {
            return _registry.BucketTimer(
                                 options,
                                 tags,
                                 () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                             NewContext(userValue);
        }

        /// <inheritdoc />
        public TimerContext Time(BucketTimerOptions options, MetricTags tags)
        {
            return
                _registry.BucketTimer(
                              options,
                              tags,
                              () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                          NewContext();
        }

        /// <inheritdoc />
        public TimerContext Time(BucketTimerOptions options)
        {
            return
                _registry.BucketTimer(
                              options,
                              () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                          NewContext();
        }

        /// <inheritdoc />
        public TimerContext Time(BucketTimerOptions options, string userValue)
        {
            return _registry.BucketTimer(
                                 options,
                                 () => _timerBuilder.Build(options.Buckets, _clock, options.DurationUnit)).
                             NewContext(userValue);
        }
    }
}