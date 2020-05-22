// <copyright file="IHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Text.Json;
using System.Threading.Tasks;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics
{
    public interface IHostedMetricsPointTextWriter
    {
        Task Write(Utf8JsonWriter jsonWriter, HostedMetricsPoint point, bool writeTimestamp = true);
    }
}