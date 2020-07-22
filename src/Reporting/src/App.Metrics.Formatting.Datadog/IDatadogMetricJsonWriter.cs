// <copyright file="IHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Text.Json;
using System.Threading.Tasks;
using App.Metrics.Formatting.Datadog.Internal;

namespace App.Metrics.Formatting.Datadog
{
    public interface IDatadogMetricJsonWriter
    {
        Task Write(Utf8JsonWriter jsonWriter, DatadogPoint point, bool writeTimestamp = true);
    }
}