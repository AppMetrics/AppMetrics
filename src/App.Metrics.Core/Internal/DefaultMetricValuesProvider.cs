// <copyright file="DefaultMetricValuesProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Internal.NoOp;
using App.Metrics.Logging;
using App.Metrics.Registry;

namespace App.Metrics.Internal
{
    public sealed class DefaultMetricValuesProvider : IProvideMetricValues
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricValuesProvider>();
        private readonly IFilterMetrics _globalFilter;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetricValuesProvider" /> class.
        /// </summary>
        /// <param name="globalFilter">The global filter.</param>
        /// <param name="registry">The registry.</param>
        public DefaultMetricValuesProvider(IFilterMetrics globalFilter, IMetricsRegistry registry)
        {
            _globalFilter = globalFilter ?? new NullMetricsFilter();
            _registry = registry ?? new NullMetricsRegistry();
        }

        /// <inheritdoc />
        public MetricsDataValueSource Get()
        {
            Logger.Trace("Getting metrics snaphot");

            var result = _registry.GetData(_globalFilter);

            Logger.Trace("Getting metrics snaphot found {MetricsContextCount}", result.Contexts?.Count());

            return result;
        }

        /// <inheritdoc />
        public MetricsDataValueSource Get(IFilterMetrics overrideGlobalFilter)
        {
            IFilterMetrics filter;

            if (overrideGlobalFilter == null || overrideGlobalFilter is NullMetricsFilter)
            {
                filter = _globalFilter;
                Logger.Trace("Getting metrics snaphot with custom filter");
            }
            else
            {
                filter = overrideGlobalFilter;
                Logger.Trace("Getting metrics snaphot with global filter");
            }

            return _registry.GetData(filter);
        }

        /// <inheritdoc />
        public MetricsContextValueSource GetForContext(string context)
        {
            Logger.Trace("Getting metrics snaphot for {MetricsContext}", context);

            var data = Get();

            var filter = new MetricsFilter().WhereContext(context);

            var contextData = data.Filter(filter);

            Logger.Trace("Getting metrics snaphot found {MetricsContextCount}", contextData.Contexts?.Count());

            return contextData.Contexts?.FirstOrDefault() ?? MetricsContextValueSource.Empty;
        }
    }
}