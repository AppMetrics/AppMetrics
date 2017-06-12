// <copyright file="IEnvironmentInfoResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Extensions.Middleware.Abstractions
{
    public interface IEnvironmentInfoResponseWriter
    {
        string ContentType { get; }

        Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default(CancellationToken));
    }
}