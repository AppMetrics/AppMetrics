// <copyright file="PingEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    public class PingEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public PingEndpointMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics) { }

        public async Task Invoke(HttpContext context)
        {
            if (Options.PingEndpointEnabled && Options.PingEndpoint.IsPresent() && Options.PingEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                await WriteResponseAsync(context, "pong", "text/plain");

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}