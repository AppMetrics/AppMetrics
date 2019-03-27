// <copyright file="IMetricsFilterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IMetricsFilterBuilder
    {
        /// <summary>
        /// Gets the <see cref="IMetricsBuilder"/> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        /// <summary>
        ///     Filters metrics the specified context. Metrics can be grouped using a context label. The default context label used
        ///     can be configured on <see cref="MetricsOptions.DefaultContextLabel" />.
        /// </summary>
        /// <param name="context">The context label to filter by.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ByIncludingOnlyContext(string context);

        /// <summary>
        ///     Filters metrics the specified tag key value pair. When fetched only metrics with a matching key and value will be returned.
        /// </summary>
        /// <param name="tagKeyValues">The tag key value pair to filter by.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ByIncludingOnlyTagKeyValues(TagKeyValueFilter tagKeyValues);

        /// <summary>
        ///     Filters metrics the specified tag key. When fetched only metrics with a matching tag key.
        /// </summary>
        /// <param name="tagKeys">The tag to filter by.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ByIncludingOnlyTags(params string[] tagKeys);

        /// <summary>
        ///     Filters metrisc the specified <see cref="MetricType" />s.
        /// </summary>
        /// <param name="types">The <see cref="MetricType" />s to include when fetching metrics.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ByIncludingOnlyTypes(params MetricType[] types);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IFilterMetrics" /> to ftiler metrics when their values are fetched.
        ///     </para>
        ///     <para>
        ///         A single <see cref="IFilterMetrics" /> should be configured. The last <see cref="IFilterMetrics" /> configured
        ///         will be used.
        ///     </para>
        /// </summary>
        /// <param name="filter">An <see cref="IFilterMetrics" /> instance used for filtering metrics.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder With(IFilterMetrics filter);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IFilterMetrics" /> setup <see cref="Action" /> to ftiler metrics when their values
        ///         are fetched.
        ///     </para>
        /// </summary>
        /// <param name="setupAction">An <see cref="IFilterMetrics" /> instance used for filtering metrics.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder With(Action<IFilterMetrics> setupAction);
    }
}