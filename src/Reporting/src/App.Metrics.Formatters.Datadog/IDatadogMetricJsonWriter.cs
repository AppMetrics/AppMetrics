// <copyright file="IHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Text.Json;
using System.Threading.Tasks;
using App.Metrics.Formatters.Datadog.Internal;

namespace App.Metrics.Formatters.Datadog
{
    public interface IDatadogMetricJsonWriter
    {
        Task WriteAsync(Utf8JsonWriter jsonWriter, DatadogPoint point, bool writeTimestamp = true);
    }
}