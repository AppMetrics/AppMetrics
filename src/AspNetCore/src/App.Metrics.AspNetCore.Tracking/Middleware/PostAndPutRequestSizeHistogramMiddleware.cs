// <copyright file="PostAndPutRequestSizeHistogramMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class PostAndPutRequestSizeHistogramMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PostAndPutRequestSizeHistogramMiddleware> _logger;
        private readonly IMetrics _metrics;

        public PostAndPutRequestSizeHistogramMiddleware(
            RequestDelegate next,
            ILogger<PostAndPutRequestSizeHistogramMiddleware> logger,
            IMetrics metrics)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _metrics = metrics;
        }

        // ReSharper disable UnusedMember.Global
        public Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<PostAndPutRequestSizeHistogramMiddleware>();

            var httpMethod = context.Request.Method.ToUpperInvariant();

            if (httpMethod == "POST")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    _metrics.UpdatePostRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                }
            }
            else if (httpMethod == "PUT")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    _metrics.UpdatePutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                }
            }

            _logger.MiddlewareExecuted<PostAndPutRequestSizeHistogramMiddleware>();

            return _next(context);
        }
    }
}