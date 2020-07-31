// <copyright file="RequestTimerMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.Timer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class RequestTimerMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly ILogger<RequestTimerMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly ITimer _requestTimer;

        public RequestTimerMiddleware(
            RequestDelegate next,
            IOptions<MetricsWebTrackingOptions> trackingMiddlwareOptionsAccessor,
            ILogger<RequestTimerMiddleware> logger,
            IMetrics metrics)
        {
            _logger = logger;
            _next = next ?? throw new ArgumentNullException(nameof(next));
            if (trackingMiddlwareOptionsAccessor.Value.UseBucketHistograms)
            {
                _requestTimer = metrics.Provider.BucketTimer.Instance(HttpRequestMetricsRegistry.BucketTimers.RequestTransactionDuration(trackingMiddlwareOptionsAccessor.Value.RequestTimeHistogramBuckets));
            }
            else
            {
                _requestTimer = metrics.Provider.Timer.Instance(HttpRequestMetricsRegistry.Timers.RequestTransactionDuration);
            }
        }

        // ReSharper disable UnusedMember.Global
        public Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                return _next(context);
            }

            return TimeTransaction(context);
        }

        private async Task TimeTransaction(HttpContext context)
        {
            _logger.MiddlewareExecuting<RequestTimerMiddleware>();

            try
            {
                using (_requestTimer.NewContext())
                {
                    await _next(context);
                }
            }
            finally
            {
                _logger.MiddlewareExecuted<RequestTimerMiddleware>();
            }
        }
    }
}