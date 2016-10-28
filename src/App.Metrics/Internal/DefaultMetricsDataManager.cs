// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics.MetricData;

namespace App.Metrics.Internal
{
    internal class DefaultMetricsDataManager : IMetricsDataManager
    {
        private readonly IMetricsRegistry _registry;

        public DefaultMetricsDataManager(
            IMetricsRegistry registry)
        {
            _registry = registry;
        }

        public Task<MetricsData> GetMetricsDataAsync()
        {
            //TODO: AH - this class still needed?
            return _registry.GetDataAsync();
        }
    }
}