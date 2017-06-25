// <copyright file="MetricsEndpointTextEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsEndpointTextEndpointMiddleware : AppMetricsMiddleware<AppMetricsMiddlewareOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly IMetricsTextResponseWriter _metricsTextResponseWriter;

        public MetricsEndpointTextEndpointMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IMetricsTextResponseWriter metricsTextResponseWriter)
            : base(next, appMiddlewareOptions, loggerFactory, metrics)
        {
            _metricsTextResponseWriter = metricsTextResponseWriter ?? throw new ArgumentNullException(nameof(metricsTextResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (Options.MetricsTextEndpointEnabled && Options.MetricsTextEndpoint.IsPresent() && Options.MetricsTextEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                context.Response.Headers["Content-Type"] = new[] { _metricsTextResponseWriter.ContentType };
                context.SetNoCacheHeaders();
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                await _metricsTextResponseWriter.WriteAsync(context, Metrics.Snapshot.Get(), context.RequestAborted).ConfigureAwait(false);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}