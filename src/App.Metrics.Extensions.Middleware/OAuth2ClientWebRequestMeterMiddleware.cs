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
    ///     Measures the overall request rate of each OAuth2 Client as well as the rate per endpoint
    /// </summary>
    public class OAuth2ClientWebRequestMeterMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public OAuth2ClientWebRequestMeterMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics) { }

        public async Task Invoke(HttpContext context)
        {
            await Next(context);

            if (PerformMetric(context) && Options.OAuth2TrackingEnabled)
            {
                Logger.MiddlewareExecuting(GetType());

                var clientId = context.OAuthClientId();

                if (clientId.IsPresent())
                {
                    var routeTemplate = context.GetMetricsCurrentRouteName();

                    Metrics.MarkHttpEndpointForOAuthClient(routeTemplate, clientId, context.Response.StatusCode);
                    Metrics.MarkHttpRequestForOAuthClient(clientId, context.Response.StatusCode);
                }

                Logger.MiddlewareExecuted(GetType());
            }
        }
    }
}