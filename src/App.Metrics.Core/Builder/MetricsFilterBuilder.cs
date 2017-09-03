// <copyright file="MetricsFilterBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filtering;
using App.Metrics.Filters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="IFilterMetrics" /> used for filtering metrics when their values are fetched.
    /// </summary>
    public class MetricsFilterBuilder
    {
        private readonly IFilterMetrics _filter;
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly Action<IFilterMetrics> _metricsFilter;

        internal MetricsFilterBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IFilterMetrics> metricsFilter)
        {
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _metricsFilter = metricsFilter ?? throw new ArgumentNullException(nameof(metricsFilter));
            _filter = new DefaultMetricsFilter();
        }

        /// <summary>
        ///     Filters metrics the specified context. Metrics can be grouped using a context label. The default context label used
        ///     can be configured on <see cref="MetricsOptions.DefaultContextLabel" />.
        /// </summary>
        /// <param name="context">The context label to filter by.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder ByIncludingOnlyContext(string context)
        {
            _filter.WhereContext(context);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

        /// <summary>
        ///     Filters metrics the specified tag key value pair. When fetched only metrics with a matching key and value will be returned.
        /// </summary>
        /// <param name="tagKeyValues">The tag key value pair to filter by.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder ByIncludingOnlyTagKeyValues(TagKeyValueFilter tagKeyValues)
        {
            _filter.WhereMetricTaggedWithKeyValue(tagKeyValues);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

        /// <summary>
        ///     Filters metrics the specified tag key. When fetched only metrics with a matching tag key.
        /// </summary>
        /// <param name="tagKeys">The tag to filter by.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder ByIncludingOnlyTags(params string[] tagKeys)
        {
            _filter.WhereMetricTaggedWithKey(tagKeys);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

        /// <summary>
        ///     Filters metrisc the specified <see cref="MetricType" />s.
        /// </summary>
        /// <param name="types">The <see cref="MetricType" />s to include when fetching metrics.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder ByIncludingOnlyTypes(params MetricType[] types)
        {
            _filter.WhereType(types);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }

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
        public IMetricsBuilder With(IFilterMetrics filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _metricsFilter(filter);

            return _metricsBuilder;
        }

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
        public IMetricsBuilder With(Action<IFilterMetrics> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            setupAction(_filter);

            _metricsFilter(_filter);

            return _metricsBuilder;
        }
    }
}