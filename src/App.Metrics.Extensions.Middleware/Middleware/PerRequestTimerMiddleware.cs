// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Metrics.Extensions.Middleware.Middleware
{
    public class PerRequestTimerMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private const string TimerItemsKey = "__Mertics.PerRequestStartTime__";

        public PerRequestTimerMiddleware(RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                context.Items[TimerItemsKey] = Metrics.Advanced.Clock.Nanoseconds;

                await Next(context);

                if (context.HasMetricsCurrentRouteName() && context.Response.StatusCode != (int)HttpStatusCode.NotFound)
                {
                    var clientId = context.OAuthClientId();

                    var startTime = (long)context.Items[TimerItemsKey];
                    var elapsed = Metrics.Advanced.Clock.Nanoseconds - startTime;

                    Metrics.RecordEndpointRequestTime(clientId, context.GetMetricsCurrentRouteName(), elapsed);
                }

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(context);
            }
        }
    }
}