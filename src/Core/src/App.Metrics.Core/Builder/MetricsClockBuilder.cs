// <copyright file="MetricsClockBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="IClock"/> used for timing when recording specific metrics types e.g. <see cref="MetricType.Timer"/>.
    /// </summary>
    public class MetricsClockBuilder : IMetricsClockBuilder
    {
        private readonly Action<IClock> _clock;

        internal MetricsClockBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IClock> clock)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder Clock(IClock clock)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            _clock(clock);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Clock<TClock>()
            where TClock : class, IClock, new()
        {
            _clock(new TClock());

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder StopwatchClock()
        {
            Clock<StopwatchClock>();

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder SystemClock()
        {
            Clock<SystemClock>();

            return Builder;
        }
    }
}