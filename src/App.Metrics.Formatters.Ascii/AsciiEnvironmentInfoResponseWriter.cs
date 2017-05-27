// <copyright file="AsciiEnvironmentInfoResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Formatters.Ascii
{
    public class AsciiEnvironmentInfoResponseWriter : IEnvironmentInfoResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/plain; app.metrics=vnd.app.metrics.v1.environment.info;";

        public Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default(CancellationToken))
        {
            return context.Response.WriteAsync(environmentInfo.Hummanize(), token);
        }
    }
}