// <copyright file="IDatadogClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.Datadog.Client
{
    public interface IDatadogClient
    {
        Task<DatadogWriteResult> WriteAsync(string payload, CancellationToken cancellationToken = default);
    }
}