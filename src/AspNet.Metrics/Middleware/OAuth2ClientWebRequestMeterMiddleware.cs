// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    /// <summary>
    ///     Measures the overall request rate of each OAuth2 Client as well as the rate per endpoint
    /// </summary>
    public class OAuth2ClientWebRequestMeterMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public OAuth2ClientWebRequestMeterMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
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

                    MarkWebRequest(routeTemplate, clientId);
                }

                Logger.MiddlewareExecuted(GetType());
            }
        }

        private void MarkWebRequest(string routeTemplate, string clientId)
        {
            MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                .Meter(AspNetMetricsRegistry.Groups.OAuth2.Meters.EndpointWebRequests(routeTemplate))
                .Mark(clientId);

            MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                .Meter(AspNetMetricsRegistry.Groups.OAuth2.Meters.WebRequests)
                .Mark(clientId);
        }
    }
}