// <copyright file="MetricsFilterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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
    public class MetricsFilterBuilder : IMetricsFilterBuilder
    {
        private readonly IFilterMetrics _filter;
        private readonly Action<IFilterMetrics> _metricsFilter;

        internal MetricsFilterBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IFilterMetrics> metricsFilter)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _metricsFilter = metricsFilter ?? throw new ArgumentNullException(nameof(metricsFilter));
            _filter = new MetricsFilter();
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyContext(string context)
        {
            _filter.WhereContext(context);

            _metricsFilter(_filter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyTagKeyValues(TagKeyValueFilter tagKeyValues)
        {
            _filter.WhereTaggedWithKeyValue(tagKeyValues);

            _metricsFilter(_filter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyTags(params string[] tagKeys)
        {
            _filter.WhereTaggedWithKey(tagKeys);

            _metricsFilter(_filter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ByIncludingOnlyTypes(params MetricType[] types)
        {
            _filter.WhereType(types);

            _metricsFilter(_filter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder With(IFilterMetrics filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _metricsFilter(filter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder With(Action<IFilterMetrics> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            setupAction(_filter);

            _metricsFilter(_filter);

            return Builder;
        }
    }
}