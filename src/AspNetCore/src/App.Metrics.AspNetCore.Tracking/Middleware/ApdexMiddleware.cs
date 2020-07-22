// <copyright file="ApdexMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.AspNetCore.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class ApdexMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApdexMiddleware> _logger;
        private readonly IApdex _apdexTracking;

        public ApdexMiddleware(
            RequestDelegate next,
            IOptions<MetricsWebTrackingOptions> trackingMiddlwareOptionsAccessor,
            ILogger<ApdexMiddleware> logger,
            IMetrics metrics)
        {
            _next = next;
            _logger = logger;
            _apdexTracking = metrics.Provider
                                    .Apdex
                                    .Instance(HttpRequestMetricsRegistry.ApdexScores.Apdex(trackingMiddlwareOptionsAccessor.Value.ApdexTSeconds));
        }

        // ReSharper disable UnusedMember.Global
        public Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            return context.WebSockets.IsWebSocketRequest 
                ? _next(context)
                : MeasureTransaction(context);
        }

        private async Task MeasureTransaction(HttpContext context)
        {
            _logger.MiddlewareExecuting<ApdexMiddleware>();

            try
            {
                using (_apdexTracking.NewContext())
                {
                    await _next(context);
                }
            }
            finally
            {
                _logger.MiddlewareExecuted<ApdexMiddleware>();
            }
        }
    }
}