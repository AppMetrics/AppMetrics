// <copyright file="HostedMetricsFormatterConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal
{
    public static class HostedMetricsFormatterConstants
    {
        public static class GraphiteDefaults
        {
            public static readonly Func<IHostedMetricsPointTextWriter> MetricPointTextWriter = () => new DefaultHostedMetricsPointTextWriter();
        }
    }
}