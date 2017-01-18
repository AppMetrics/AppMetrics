// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using App.Metrics.Core.Interfaces;
using App.Metrics.Data;
using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultDataManager : IMetricsDataProvider
    {
        private readonly IMetricsFilter _globalFilter;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultDataManager" /> class.
        /// </summary>
        /// <param name="globalFilter">The global filter.</param>
        /// <param name="registry">The registry.</param>
        public DefaultDataManager(IMetricsFilter globalFilter, IMetricsRegistry registry)
        {
            _globalFilter = globalFilter ?? new NoOpMetricsFilter();
            _registry = registry ?? new NullMetricsRegistry();
        }

        /// <inheritdoc />
        public MetricsContextValueSource ReadContext(string context)
        {
            var data = ReadData();

            var filter = new DefaultMetricsFilter().WhereContext(context);

            var contextData = data.Filter(filter);

            return contextData.Contexts.Single();
        }

        /// <inheritdoc />
        public MetricsDataValueSource ReadData() { return _registry.GetData(_globalFilter); }

        /// <inheritdoc />
        public MetricsDataValueSource ReadData(IMetricsFilter overrideGlobalFilter) { return _registry.GetData(overrideGlobalFilter); }
    }
}