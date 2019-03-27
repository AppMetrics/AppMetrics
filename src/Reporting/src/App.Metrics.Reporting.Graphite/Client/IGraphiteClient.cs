// <copyright file="IGraphiteClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.Graphite.Client
{
    public interface IGraphiteClient
    {
        Task<GraphiteWriteResult> WriteAsync(string payload, CancellationToken cancellationToken = default);
    }
}