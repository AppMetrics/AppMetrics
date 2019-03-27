// <copyright file="RequestTimerMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.Timer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class RequestTimerMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private const string TimerItemsKey = "__App.Metrics.RequestTimer__";
        private readonly ILogger<RequestTimerMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly ITimer _requestTimer;

        public RequestTimerMiddleware(
            RequestDelegate next,
            ILogger<RequestTimerMiddleware> logger,
            IMetrics metrics)
        {
            _logger = logger;
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _requestTimer = metrics.Provider
                                   .Timer
                                   .Instance(HttpRequestMetricsRegistry.Timers.RequestTransactionDuration);
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<RequestTimerMiddleware>();

            context.Items[TimerItemsKey] = _requestTimer.NewContext();

            await _next(context);

            var timer = context.Items[TimerItemsKey];

            using (timer as IDisposable)
            {
            }

            context.Items.Remove(TimerItemsKey);

            _logger.MiddlewareExecuted<RequestTimerMiddleware>();
        }
    }
}