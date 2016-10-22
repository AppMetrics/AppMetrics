// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Globalization;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    internal class MetricsJsonBuilderV1 : IMetricsJsonBuilder
    {
        public const int Version = 1;

        private const bool DefaultIndented = true;

        public string MetricsMimeType { get; } = "application/vnd.app.metrics.v1.metrics+json";

        public string BuildJson(IMetricsContext metricsContext, EnvironmentInfo environmentInfo,
            IMetricsFilter filter)
        {
            return BuildJson(metricsContext, environmentInfo, filter, DefaultIndented);
        }

        public string BuildJson(IMetricsContext metricsContext, EnvironmentInfo environmentInfo, 
            IMetricsFilter filter, bool indented)
        {
          
            var version = Version.ToString(CultureInfo.InvariantCulture);
            var metricsData = metricsContext.Advanced.DataManager
                .GetMetricsData()
                .Filter(filter);

            return JsonMetricsContext.FromContext(metricsData, environmentInfo, version)
                .ToJsonObject(metricsContext.Advanced.Clock)
                .AsJson(indented);
        }
    }
}