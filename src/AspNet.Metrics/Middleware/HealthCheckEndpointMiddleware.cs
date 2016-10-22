// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class HealthCheckEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public HealthCheckEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.HealthEnabled && Options.HealthEndpoint.HasValue && Options.HealthEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var healthStatus = await MetricsContext.Advanced.HealthCheckManager.GetStatusAsync();
                var responseStatusCode = healthStatus.IsHealthy ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                await
                    Task.FromResult(WriteResponseAsync(context, JsonHealthChecks.BuildJson(healthStatus, MetricsContext.Advanced.Clock, true),
                        "application/json", responseStatusCode));

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}