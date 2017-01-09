// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace App.Metrics.Extensions.Owin.Middleware
{
    /// <summary>
    ///     Measures the overall request rate of each OAuth2 Client as well as the rate per endpoint
    /// </summary>
    public class OAuth2ClientWebRequestMeterMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        public OAuth2ClientWebRequestMeterMiddleware(
            OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(owinOptions, loggerFactory, metrics)
        {
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await Next(environment);

            if (PerformMetric(environment) && Options.OAuth2TrackingEnabled)
            {
                Logger.MiddlewareExecuting(GetType());

                var clientId = environment.OAuthClientId();

                if (clientId.IsPresent())
                {
                    var httpResponseStatusCode = int.Parse(environment["owin.ResponseStatusCode"].ToString());

                    var routeTemplate = environment.GetMetricsCurrentRouteName();

                    Metrics.MarkHttpEndpointForOAuthClient(routeTemplate, clientId, httpResponseStatusCode);
                    Metrics.MarkHttpRequestForOAuthClient(clientId, httpResponseStatusCode);
                }

                Logger.MiddlewareExecuted(GetType());
            }
        }
    }
}