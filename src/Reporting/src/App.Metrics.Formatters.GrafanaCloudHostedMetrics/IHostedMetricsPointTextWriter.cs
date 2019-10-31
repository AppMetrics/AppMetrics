// <copyright file="IHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics
{
    public interface IHostedMetricsPointTextWriter
    {
        Task Write(JsonWriter textWriter, HostedMetricsPoint point, bool writeTimestamp = true);
    }
}