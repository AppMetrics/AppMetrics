// <copyright file="IMetricsClockBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IMetricsClockBuilder
    {
        /// <summary>
        /// Gets the <see cref="IMetricsBuilder"/> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IClock"/> to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <param name="clock">An <see cref="IClock"/> instance used for timing. e.g. "StopwatchClock"</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Clock(IClock clock);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IClock"/> to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <typeparam name="TClock">An <see cref="IClock"/> type used for timing. e.g. "StopwatchClock"</typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Clock<TClock>()
            where TClock : class, IClock, new();

        /// <summary>
        ///     <para>
        ///         Uses the "StopwatchClock" to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder StopwatchClock();

        /// <summary>
        ///     <para>
        ///         Uses the "SystemClock" to time specific <see cref="MetricType"/>s. e.g. <see cref="MetricType.Timer"/>.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IClock"/> should be configured. The last <see cref="IClock"/> configured will be used.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder SystemClock();
    }
}