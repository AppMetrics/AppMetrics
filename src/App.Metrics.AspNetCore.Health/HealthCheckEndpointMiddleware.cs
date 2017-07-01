// <copyright file="HealthCheckEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class HealthCheckEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly AppMetricsMiddlewareHealthChecksOptions _appMiddlewareOptions;
        private readonly IProvideHealth _health;
        private readonly IHealthResponseWriter _healthResponseWriter;
        private readonly ILogger<HealthCheckEndpointMiddleware> _logger;

        public HealthCheckEndpointMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareHealthChecksOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IProvideHealth health,
            IHealthResponseWriter healthResponseWriter)
        {
            _next = next;
            _appMiddlewareOptions = appMiddlewareOptions;
            _health = health;
            _logger = loggerFactory.CreateLogger<HealthCheckEndpointMiddleware>();
            _healthResponseWriter = healthResponseWriter ?? throw new ArgumentNullException(nameof(healthResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (_appMiddlewareOptions.HealthEndpointEnabled &&
                _appMiddlewareOptions.HealthEndpoint.IsPresent() &&
                _appMiddlewareOptions.HealthEndpoint == context.Request.Path)
            {
                _logger.MiddlewareExecuting(GetType());

                var healthStatus = await _health.ReadStatusAsync(context.RequestAborted);
                string warning = null;

                var responseStatusCode = HttpStatusCode.OK;

                if (healthStatus.Status.IsUnhealthy())
                {
                    responseStatusCode = HttpStatusCode.ServiceUnavailable;
                }

                if (healthStatus.Status.IsDegraded())
                {
                    responseStatusCode = HttpStatusCode.OK;
                    warning = "Degraded";
                }

                context.Response.Headers["Content-Type"] = new[] { _healthResponseWriter.ContentType };
                context.SetNoCacheHeaders();
                context.Response.StatusCode = (int)responseStatusCode;

                if (warning.IsPresent())
                {
                    context.Response.Headers["Warning"] = new[] { $"Warning: 100 '{warning}'" };
                }

                await _healthResponseWriter.WriteAsync(context, healthStatus, context.RequestAborted).ConfigureAwait(false);

                _logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}