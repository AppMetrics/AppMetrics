// <copyright file="IHostedMetricsClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.GrafanaCloudHostedMetrics.Client
{
    public interface IHostedMetricsClient
    {
        Task<HostedMetricsWriteResult> WriteAsync(string payload, CancellationToken cancellationToken = default);
    }
}