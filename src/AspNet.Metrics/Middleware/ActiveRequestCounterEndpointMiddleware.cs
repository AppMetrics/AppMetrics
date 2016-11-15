// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics;
using AppMetrics;
using AspNet.Metrics.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Middleware
{
    public class ActiveRequestCounterEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public ActiveRequestCounterEndpointMiddleware(RequestDelegate next,
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
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                Metrics.IncrementActiveRequests();

                await Next(context);

                Metrics.DecrementActiveRequests();

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(context);
            }
        }
    }
}