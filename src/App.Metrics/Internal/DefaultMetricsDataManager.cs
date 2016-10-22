// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal
{
    internal class DefaultMetricsDataManager : IMetricsDataManager
    {
        private readonly ILogger _logger;
        private readonly IMetricsRegistry _registry;

        public DefaultMetricsDataManager(
            ILoggerFactory loggerFactory,
            IMetricsRegistry registry)
        {
            _logger = loggerFactory.CreateLogger<DefaultMetricsDataManager>();
            _registry = registry;
        }

        public MetricsData GetMetricsData()
        {
            _logger.MetricsDataGetExecuting();

            //TODO: AH - this class still needed?
            var metricsData =_registry.GetData();

            _logger.MetricsDataGetExecuted();

            return metricsData;
        }
    }
}