// <copyright file="IMetricsReportingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Reporting;

namespace App.Metrics.Builder
{
    public interface IMetricsReportingBuilder
    {
        /// <summary>
        ///     Gets the <see cref="IMetricsBuilder" /> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <param name="reporter">An <see cref="IReportMetrics" /> instance used to report metric values.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using(IReportMetrics reporter);

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <typeparam name="TReportMetrics">
        ///     An <see cref="IReportMetrics" /> type used to report metric
        ///     values.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TReportMetrics>()
            where TReportMetrics : IReportMetrics, new();

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <typeparam name="TReportMetrics">
        ///     An <see cref="IReportMetrics" /> type used to report metric
        ///     values.
        /// </typeparam>
        /// <param name="filter">
        ///     An <see cref="IFilterMetrics" /> instance used to filter metrics for this
        ///     <see cref="IReportMetrics" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TReportMetrics>(IFilterMetrics filter)
            where TReportMetrics : IReportMetrics, new();

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <typeparam name="TReportMetrics">
        ///     An <see cref="IReportMetrics" /> type used to report metric
        ///     values.
        /// </typeparam>
        /// <param name="formatter">
        ///     An <see cref="IMetricsOutputFormatter" /> instance used to format metrics for this
        ///     <see cref="IReportMetrics" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TReportMetrics>(IMetricsOutputFormatter formatter)
            where TReportMetrics : IReportMetrics, new();

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <typeparam name="TReportMetrics">
        ///     An <see cref="IReportMetrics" /> type used to report metric
        ///     values.
        /// </typeparam>
        /// <param name="filter">
        ///     An <see cref="IFilterMetrics" /> instance used to filter metrics for this
        ///     <see cref="IReportMetrics" />.
        /// </param>
        /// <param name="flushInterval">The <see cref="TimeSpan" /> interval used if intended to schedule metrics reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TReportMetrics>(IFilterMetrics filter, TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new();

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <typeparam name="TReportMetrics">
        ///     An <see cref="IReportMetrics" /> type used to report metric
        ///     values.
        /// </typeparam>
        /// <param name="formatter">
        ///     An <see cref="IMetricsOutputFormatter" /> instance used to format metrics for this
        ///     <see cref="IReportMetrics" />.
        /// </param>
        /// <param name="filter">
        ///     An <see cref="IFilterMetrics" /> instance used to filter metrics for this
        ///     <see cref="IReportMetrics" />.
        /// </param>
        /// <param name="flushInterval">The <see cref="TimeSpan" /> interval used if intended to schedule metrics reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TReportMetrics>(IMetricsOutputFormatter formatter, IFilterMetrics filter, TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new();

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <typeparam name="TReportMetrics">
        ///     An <see cref="IReportMetrics" /> type used to report metric
        ///     values.
        /// </typeparam>
        /// <param name="formatter">
        ///     An <see cref="IMetricsOutputFormatter" /> instance used to format metrics for this
        ///     <see cref="IReportMetrics" />.
        /// </param>
        /// <param name="flushInterval">The <see cref="TimeSpan" /> interval used if intended to schedule metrics reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TReportMetrics>(IMetricsOutputFormatter formatter, TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new();

        /// <summary>
        ///     Reports metrics using the specifed <see cref="IReportMetrics" />.
        /// </summary>
        /// <typeparam name="TReportMetrics">
        ///     An <see cref="IReportMetrics" /> type used to report metric
        ///     values.
        /// </typeparam>
        /// <param name="flushInterval">The <see cref="TimeSpan" /> interval used if intended to schedule metrics reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TReportMetrics>(TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new();
    }
}