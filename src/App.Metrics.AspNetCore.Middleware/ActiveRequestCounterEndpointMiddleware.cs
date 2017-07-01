// <copyright file="ActiveRequestCounterEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Middleware.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class ActiveRequestCounterEndpointMiddleware : AppMetricsMiddleware<AppMetricsMiddlewareOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public ActiveRequestCounterEndpointMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, appMiddlewareOptions, loggerFactory, metrics)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (appMiddlewareOptions == null)
            {
                throw new ArgumentNullException(nameof(appMiddlewareOptions));
            }
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                Metrics.IncrementActiveRequests();

                try
                {
                    await Next(context);
                    Metrics.DecrementActiveRequests();
                }
                catch (Exception)
                {
                    Metrics.DecrementActiveRequests();
                    throw;
                }
                finally
                {
                    Logger.MiddlewareExecuted(GetType());
                }
            }
            else
            {
                await Next(context);
            }
        }
    }
}