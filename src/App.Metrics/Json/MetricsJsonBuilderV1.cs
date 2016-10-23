// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Globalization;
using System.Threading.Tasks;

namespace App.Metrics.Json
{
    internal class MetricsJsonBuilderV1 : IMetricsJsonBuilder
    {
        public const int Version = 1;

        private const bool DefaultIndented = true;

        public string MetricsMimeType { get; } = "application/vnd.app.metrics.v1.metrics+json";

        public Task<string> BuildJsonAsync(IMetricsContext metricsContext, IMetricsFilter filter)
        {
            return BuildJsonAsync(metricsContext, filter, DefaultIndented);
        }

        public async Task<string> BuildJsonAsync(IMetricsContext metricsContext, IMetricsFilter filter, bool indented)
        {
            var version = Version.ToString(CultureInfo.InvariantCulture);
            var metricsData = await metricsContext.Advanced.DataManager.GetMetricsDataAsync();
            metricsData = metricsData.Filter(filter);

            return JsonMetricsContext.FromContext(metricsData, version)
                .ToJsonObject(metricsContext.Advanced.Clock)
                .AsJson(indented);
        }
    }
}