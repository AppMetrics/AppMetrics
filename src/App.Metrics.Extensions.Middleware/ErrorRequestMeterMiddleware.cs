// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    /// <summary>
    ///     Measures the overall error request rate as well as the rate per endpoint.
    ///     Also measures these error rates per OAuth2 Client as a separate metric
    /// </summary>
    public class ErrorRequestMeterMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public ErrorRequestMeterMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (aspNetOptions == null)
            {
                throw new ArgumentNullException(nameof(aspNetOptions));
            }

            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }
        }

        public async Task Invoke(HttpContext context)
        {
            await Next(context);

            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                var routeTemplate = context.GetMetricsCurrentRouteName();

                if (!context.Response.IsSuccessfulResponse())
                {
                    Metrics.MarkHttpRequestEndpointError(routeTemplate, context.Response.StatusCode);
                    Metrics.MarkHttpRequestError(context.Response.StatusCode);
                    Metrics.ErrorRequestPercentage();
                }
            }

            Logger.MiddlewareExecuted(GetType());
        }
    }
}