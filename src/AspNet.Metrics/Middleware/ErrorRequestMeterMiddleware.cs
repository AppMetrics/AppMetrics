// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    /// <summary>
    ///     Measures the overall error request rate as well as the rate per endpoint.
    ///     Also measures these error rates per OAuth2 Client as a separate metric
    /// </summary>
    public class ErrorRequestMeterMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public ErrorRequestMeterMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)

        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }
        }

        public async Task Invoke(HttpContext context)
        {
            //TODO: AH - does oauth2 client tracking belong here?
            await Next(context);

            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                var clientId = context.OAuthClientId();

                var routeTemplate = context.GetMetricsCurrentRouteName();

                if (!context.Response.IsSuccessfulResponse())
                {
                    MarkOverallWebRequestError(clientId, routeTemplate);

                    switch (context.Response.StatusCode)
                    {
                        case (int)HttpStatusCode.InternalServerError:
                            MarkInternalServerErrorRequest(routeTemplate, clientId);
                            break;
                        case (int)HttpStatusCode.BadRequest:
                            MarkBadRequest(routeTemplate, clientId);
                            break;
                        case (int)HttpStatusCode.Unauthorized:
                            MarkUnAuthorizedRequest(routeTemplate, clientId);
                            break;
                    }
                }
            }

            Logger.MiddlewareExecuted(GetType());
        }

        private void MarkBadRequest(string routeTemplate, string clientId)
        {
            if (clientId.IsPresent())
            {
                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter($"{routeTemplate} Bad Requests", Unit.Custom("400 Errors"))
                    .Mark(clientId);

                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter("Total Bad Requests", Unit.Custom("400 Errors"))
                    .Mark(clientId);
            }

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter($"{routeTemplate} Bad Requests", Unit.Custom("400 Errors"))
                .Mark();

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter("Total Bad Requests", Unit.Custom("400 Errors"))
                .Mark();
        }

        private void MarkInternalServerErrorRequest(string routeTemplate, string clientId)
        {
            if (clientId.IsPresent())
            {
                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter($"{routeTemplate} Internal Server Error Requests", Unit.Custom("500 Errors"))
                    .Mark(clientId);

                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter("Total Internal Server Error Requests", Unit.Custom("500 Errors"))
                    .Mark(clientId);
            }

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter($"{routeTemplate} Internal Server Error Requests", Unit.Custom("500 Errors"))
                .Mark();

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter("Total Internal Server Error Requests", Unit.Custom("500 Errors"))
                .Mark();
        }

        private void MarkOverallWebRequestError(string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter("Total Error Requests", Unit.Errors)
                    .Mark(clientId);

                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter($"{routeTemplate} Total Error Requests", Unit.Errors)
                    .Mark(clientId);
            }

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter("Total Error Requests", Unit.Errors).Mark();

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter($"{routeTemplate} Total Error Requests", Unit.Errors).Mark();
        }

        private void MarkUnAuthorizedRequest(string routeTemplate, string clientId)
        {
            if (clientId.IsPresent())
            {
                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter($"{routeTemplate} Unauthorized Requests", Unit.Custom("401 Errors"))
                    .Mark(clientId);

                MetricsContext.GetOAuth2ClientWebRequestsContext().Advanced
                    .Meter("Total Unauthorized Requests", Unit.Custom("401 Errors"))
                    .Mark(clientId);
            }

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter($"{routeTemplate} Unauthorized Requests", Unit.Custom("401 Errors"))
                .Mark();

            MetricsContext.GetWebApplicationContext().Advanced
                .Meter("Total Unauthorized Requests", Unit.Custom("401 Errors"))
                .Mark();
        }
    }
}