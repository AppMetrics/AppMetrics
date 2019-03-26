// <copyright file="IEnvResponseWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore
{
    public interface IEnvResponseWriter
    {
        Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default);
    }
}