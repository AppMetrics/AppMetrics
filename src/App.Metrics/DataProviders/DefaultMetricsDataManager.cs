// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.DataProviders
{
    public class DefaultMetricsDataManager : IMetricsDataManager
    {
        private readonly IClock _clock;
        private readonly ILogger _logger;
        private readonly IEnumerable<EnvironmentInfoEntry> _environment;

        public DefaultMetricsDataManager(
            ILoggerFactory loggerFactory,
            IClock clock,
            IEnumerable<EnvironmentInfoEntry> environment)
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
        }

        public MetricsData GetMetricsData(IMetricsContext metricsContext)
        {
            _logger.MetricsDataGetExecuting();

            var registryDataProvider = metricsContext.Advanced.MetricRegistryManager;

            var metricsData = new MetricsData(metricsContext.GroupName, _clock.UtcDateTime,
                _environment,
                registryDataProvider.Gauges.ToArray(),
                registryDataProvider.Counters.ToArray(),
                registryDataProvider.Meters.ToArray(),
                registryDataProvider.Histograms.ToArray(),
                registryDataProvider.Timers.ToArray(),
                metricsContext.Groups.Values.Select(p => p.Advanced.MetricsDataManager.GetMetricsData(p)));

            _logger.MetricsDataGetExecuted();

            return metricsData;
        }
    }
}