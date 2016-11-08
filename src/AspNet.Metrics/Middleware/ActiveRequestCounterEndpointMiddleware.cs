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
    public class ActiveRequestCounterEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public ActiveRequestCounterEndpointMiddleware(RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, aspNetOptions, loggerFactory, metricsContext)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (aspNetOptions == null)
            {
                throw new ArgumentNullException(nameof(aspNetOptions));
            }
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                MetricsContext.IncrementActiveRequests();

                await Next(context);

                MetricsContext.DecrementActiveRequests();

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(context);
            }
        }
    }
}