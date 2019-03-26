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
        private const string ApdexItemsKey = "__App.Metrics.Apdex__";
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
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<ApdexMiddleware>();

            context.Items[ApdexItemsKey] = _apdexTracking.NewContext();

            await _next(context);

            var apdex = context.Items[ApdexItemsKey];

            using (apdex as IDisposable)
            {
            }

            context.Items.Remove(ApdexItemsKey);

            _logger.MiddlewareExecuted<ApdexMiddleware>();
        }
    }
}