// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.DataProviders
{
    internal class DefaultMetricsDataManager : IMetricsDataManager
    {
        private readonly IClock _clock;
        private readonly ILogger _logger;
        private readonly IEnumerable<EnvironmentInfoEntry> _environment;
        private readonly IMetricsRegistry _registry;

        public DefaultMetricsDataManager(
            ILoggerFactory loggerFactory,
            IClock clock,
            IEnumerable<EnvironmentInfoEntry> environment,
            IMetricsRegistry registry)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _logger = loggerFactory.CreateLogger<DefaultMetricsDataManager>();
            _clock = clock;
            _environment = environment;
            _registry = registry;
        }

        public MetricsData GetMetricsData()
        {
            _logger.MetricsDataGetExecuting();

            var metricsData =_registry.GetData();

            _logger.MetricsDataGetExecuted();

            return metricsData;
        }
    }
}