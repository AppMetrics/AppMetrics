// <copyright file="RequestTimerMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Timer.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global

    public class RequestTimerMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private const string TimerItemsKey = "__App.Metrics.RequestTimer__";
        private readonly ITimer _requestTimer;

        public RequestTimerMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
            _requestTimer = Metrics.Provider
                                   .Timer
                                   .Instance(HttpRequestMetricsRegistry.Timers.RequestTransactionDuration);
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
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