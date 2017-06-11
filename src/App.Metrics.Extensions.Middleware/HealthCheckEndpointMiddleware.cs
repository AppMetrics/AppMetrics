// <copyright file="HealthCheckEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.Core.Internal;
using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global

    public class HealthCheckEndpointMiddleware : AppMetricsMiddleware<AppMetricsMiddlewareOptions>
    {
        private readonly IHealthResponseWriter _healthResponseWriter;

        public HealthCheckEndpointMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IHealthResponseWriter healthResponseWriter)
            : base(next, appMiddlewareOptions, loggerFactory, metrics)
        {
            _healthResponseWriter = healthResponseWriter ?? throw new ArgumentNullException(nameof(healthResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
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

                context.Response.Headers["Content-Type"] = new[] { _healthResponseWriter.ContentType };
                context.SetNoCacheHeaders();
                context.Response.StatusCode = (int)responseStatusCode;

                if (warning.IsPresent())
                {
                    context.Response.Headers["Warning"] = new[] { $"Warning: 100 '{warning}'" };
                }

                await _healthResponseWriter.WriteAsync(context, healthStatus, context.RequestAborted).ConfigureAwait(false);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }

    // ReSharper restore UnusedMember.Global
}