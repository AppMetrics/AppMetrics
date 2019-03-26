// <copyright file="PerRequestTimerMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class PerRequestTimerMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private const string TimerItemsKey = "__App.Metrics.PerRequestStartTime__";
        private readonly RequestDelegate _next;
        private readonly ILogger<PerRequestTimerMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly IList<int> _ignoredHttpStatusCodes;

        public PerRequestTimerMiddleware(
            RequestDelegate next,
            IOptions<MetricsWebTrackingOptions> trackingMiddlwareOptionsAccessor,
            ILogger<PerRequestTimerMiddleware> logger,
            IMetrics metrics)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _metrics = metrics;
            _ignoredHttpStatusCodes = trackingMiddlwareOptionsAccessor.Value.IgnoredHttpStatusCodes;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<PerRequestTimerMiddleware>();

            context.Items[TimerItemsKey] = _metrics.Clock.Nanoseconds;

            await _next(context);

            if (context.HasMetricsCurrentRouteName() && _ignoredHttpStatusCodes.All(i => i != context.Response.StatusCode))
            {
                var startTime = (long)context.Items[TimerItemsKey];
                var elapsed = _metrics.Clock.Nanoseconds - startTime;

                _metrics.RecordEndpointsRequestTime(
                    context.GetOAuthClientIdIfRequired(),
                    context.GetMetricsCurrentRouteName(),
                    elapsed);
            }

            _logger.MiddlewareExecuted<PerRequestTimerMiddleware>();
        }
    }
}