// <copyright file="MetricsClockBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
    public class MetricsClockBuilder
    {
        private readonly Action<IClock> _clock;
        private readonly IMetricsBuilder _metricsBuilder;

        internal MetricsClockBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IClock> clock)
        {
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IClock"/> to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <param name="clock">An <see cref="IClock"/> instance used for timing. e.g. <see cref="StopwatchClock"/></param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder Clock(IClock clock)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            _clock(clock);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IClock"/> to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <typeparam name="TClock">An <see cref="IClock"/> type used for timing. e.g. <see cref="StopwatchClock"/></typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder Clock<TClock>()
            where TClock : class, IClock, new()
        {
            _clock(new TClock());

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="StopwatchClock"/> to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder StopwatchClock()
        {
            Clock<StopwatchClock>();

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="SystemClock"/> to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder SystemClock()
        {
            Clock<SystemClock>();

            return _metricsBuilder;
        }
    }
}