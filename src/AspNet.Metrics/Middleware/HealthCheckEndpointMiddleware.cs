// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.DataProviders;
using App.Metrics.Json;
using App.Metrics.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class HealthCheckEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly IHealthCheckManager _healthCheckManager;
        private readonly IClock _systemClock;

        public HealthCheckEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            IClock systemClock,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IHealthCheckManager healthCheckManager)
            : base(next, options, loggerFactory, metricsContext)
        {
            _systemClock = systemClock;
            _healthCheckManager = healthCheckManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.HealthEnabled && Options.HealthEndpoint.HasValue && Options.HealthEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var healthStatus = await _healthCheckManager.GetStatusAsync();
                var responseStatusCode = healthStatus.IsHealthy ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                await
                    Task.FromResult(WriteResponseAsync(context, JsonHealthChecks.BuildJson(healthStatus, _systemClock, true), "application/json",
                        responseStatusCode));

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}