// <copyright file="OAuthTrackingMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class OAuthTrackingMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<OAuthTrackingMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly IList<int> _ignoredHttpStatusCodes;

        public OAuthTrackingMiddleware(
            RequestDelegate next,
            IOptions<MetricsWebTrackingOptions> trackingMiddlwareOptionsAccessor,
            ILogger<OAuthTrackingMiddleware> logger,
            IMetrics metrics)
        {
            _next = next;
            _logger = logger;
            _metrics = metrics;
            _ignoredHttpStatusCodes = trackingMiddlwareOptionsAccessor.Value.IgnoredHttpStatusCodes;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            await _next(context);

            var clientid = context.GetOAuthClientIdIfRequired();

            _logger.MiddlewareExecuting<OAuthTrackingMiddleware>();

            var routeTemplate = context.GetMetricsCurrentRouteName();

            _metrics.RecordClientRequestRate(routeTemplate, clientid);

            if (!context.Response.IsSuccessfulResponse() && _ignoredHttpStatusCodes.All(i => i != context.Response.StatusCode))
            {
                _metrics.RecordClientHttpRequestError(routeTemplate, context.Response.StatusCode, clientid);
            }

            var httpMethod = context.Request.Method.ToUpperInvariant();

            if (httpMethod == "POST")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    _metrics.UpdateClientPostRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                }
            }
            else if (httpMethod == "PUT")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    _metrics.UpdateClientPutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                }
            }

            _logger.MiddlewareExecuted<OAuthTrackingMiddleware>();
        }
    }
}