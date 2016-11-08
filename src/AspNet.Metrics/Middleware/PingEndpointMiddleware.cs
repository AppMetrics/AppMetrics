// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Middleware
{
    public class PingEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public PingEndpointMiddleware(RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, aspNetOptions, loggerFactory, metricsContext)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.PingEndpointEnabled && Options.PingEndpoint.HasValue && Options.PingEndpoint == context.Request.Path)
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