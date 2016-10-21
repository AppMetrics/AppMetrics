// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using AppMetrics;
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
                    MetricsContext.MarkOverallWebRequestError(clientId, routeTemplate);

                    switch (context.Response.StatusCode)
                    {
                        case (int)HttpStatusCode.InternalServerError:
                            MetricsContext.MarkInternalServerErrorRequest(clientId, routeTemplate);
                            break;
                        case (int)HttpStatusCode.BadRequest:
                            MetricsContext.MarkBadRequest(clientId, routeTemplate);
                            break;
                        case (int)HttpStatusCode.Unauthorized:
                            MetricsContext.MarkUnAuthorizedRequest(clientId, routeTemplate);
                            break;
                    }
                }
            }

            Logger.MiddlewareExecuted(GetType());
        }
    }
}