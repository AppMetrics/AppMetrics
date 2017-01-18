// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Extensions.Middleware.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware.Middleware
{
    public class RequestTimerMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private const string TimerItemsKey = "__App.Mertics.RequestTimer__";
        private readonly ITimer _requestTimer;

        public RequestTimerMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
            _requestTimer = Metrics.Advanced
                                   .Timer
                                   .Instance(AspNetMetricsRegistry.Contexts.HttpRequests.Timers.WebRequestTimer);
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                context.Items[TimerItemsKey] = _requestTimer.NewContext();

                await Next(context);

                var timer = context.Items[TimerItemsKey];

                using (timer as IDisposable)
                {
                }

                context.Items.Remove(TimerItemsKey);

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(context);
            }
        }
    }
}