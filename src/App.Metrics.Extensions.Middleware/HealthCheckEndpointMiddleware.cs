// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Serialization;
using App.Metrics.Core.Internal;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    public class HealthCheckEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly IHealthStatusSerializer _serializer;

        public HealthCheckEndpointMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IHealthStatusSerializer serializer)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            _serializer = serializer;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.HealthEndpointEnabled &&
                Options.HealthEndpoint.IsPresent() &&
                Options.HealthEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var healthStatus = await Metrics.Health.ReadStatusAsync(context.RequestAborted);
                string warning = null;

                var responseStatusCode = HttpStatusCode.OK;

                if (healthStatus.Status.IsUnhealthy())
                {
                    responseStatusCode = HttpStatusCode.InternalServerError;
                }

                if (healthStatus.Status.IsDegraded())
                {
                    responseStatusCode = HttpStatusCode.OK;
                    warning = Constants.Health.DegradedStatusDisplay;
                }

                var json = _serializer.Serialize(healthStatus);

                await WriteResponseAsync(context, json, "application/json", responseStatusCode, warning);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}