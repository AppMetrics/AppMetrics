// <copyright file="EnvironmentInfoMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Core.Infrastructure;
using App.Metrics.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class EnvironmentInfoMiddleware : AppMetricsMiddleware<AppMetricsMiddlewareOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly EnvironmentInfoProvider _environmentInfoProvider;
        private readonly IEnvironmentInfoResponseWriter _environmentInfoResponseWriter;
        private readonly RequestDelegate _next;

        public EnvironmentInfoMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IEnvironmentInfoResponseWriter environmentInfoResponseWriter,
            EnvironmentInfoProvider environmentInfoProvider)
            : base(next, appMiddlewareOptions, loggerFactory, metrics)
        {
            _environmentInfoProvider = environmentInfoProvider;
            _environmentInfoResponseWriter = environmentInfoResponseWriter ?? throw new ArgumentNullException(nameof(environmentInfoResponseWriter));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (Options.EnvironmentInfoEndpointEnabled && Options.EnvironmentInfoEndpoint.IsPresent() && Options.EnvironmentInfoEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                context.Response.Headers["Content-Type"] = new[] { _environmentInfoResponseWriter.ContentType };
                context.SetNoCacheHeaders();
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                await _environmentInfoResponseWriter.WriteAsync(context, _environmentInfoProvider.Build(), context.RequestAborted).ConfigureAwait(false);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}
