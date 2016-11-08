// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics;
using AppMetrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Middleware
{
    /// <summary>
    ///     Measures the overall request rate of each OAuth2 Client as well as the rate per endpoint
    /// </summary>
    public class OAuth2ClientWebRequestMeterMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public OAuth2ClientWebRequestMeterMiddleware(RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, aspNetOptions, loggerFactory, metricsContext)
        {
        }

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

                    MetricsContext.MarkEndpointRequest(routeTemplate, clientId);
                    MetricsContext.MarkRequest(clientId);
                }

                Logger.MiddlewareExecuted(GetType());
            }
        }
    }
}