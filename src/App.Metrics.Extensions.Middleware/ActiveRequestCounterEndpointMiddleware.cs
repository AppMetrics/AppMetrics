// <copyright file="ActiveRequestCounterEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class ActiveRequestCounterEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public ActiveRequestCounterEndpointMiddleware(
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