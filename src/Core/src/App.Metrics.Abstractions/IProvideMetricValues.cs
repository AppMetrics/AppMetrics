// <copyright file="IProvideMetricValues.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Filters;

namespace App.Metrics
{
    /// <summary>
    ///     Provides access to the current metrics recorded by the application
    /// </summary>
    public interface IProvideMetricValues
    {
        /// <summary>
        ///     Returns the current metrics data for all contexts.
        /// </summary>
        /// <returns>
        ///     A snapshot of the current metrics data, if a global <see cref="IFilterMetrics" /> is configured this will be
        ///     applied on the result.
        /// </returns>
        MetricsDataValueSource Get();

        /// <summary>
        ///     Returns the current metrics data for the context for which this provider has been created.
        /// </summary>
        /// <param name="overrideGlobalFilter">
        ///     The override the configured global filter and filters metric data by the specified
        ///     <see cref="IFilterMetrics" />.
        /// </param>
        /// <returns>
        ///     A snapshot of the current metrics data filtered by the specified <see cref="IFilterMetrics" />
        /// </returns>
        MetricsDataValueSource Get(IFilterMetrics overrideGlobalFilter);

        /// <summary>
        ///     Retrieves a snapshot of the current metrics values recorded.
        /// </summary>
        /// <param name="context">The metric context to retreive.</param>
        /// <returns>Metrics data belonging to the specified context</returns>
        MetricsContextValueSource GetForContext(string context);
    }
}