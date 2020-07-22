// <copyright file="OAuthTrackingMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.BucketHistogram;
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
        private readonly bool _useBucketHistograms;
        private readonly BucketHistogramOptions _postSizeBucketHistogramOptions;
        private readonly BucketHistogramOptions _putSizeBucketHistogramOptions;

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
            if (trackingMiddlwareOptionsAccessor.Value.UseBucketHistograms)
            {
                _useBucketHistograms = true;
                _postSizeBucketHistogramOptions = OAuthRequestMetricsRegistry.BucketHistograms.PostRequestSizeHistogram(trackingMiddlwareOptionsAccessor.Value.RequestSizeHistogramBuckets);
                _putSizeBucketHistogramOptions = OAuthRequestMetricsRegistry.BucketHistograms.PutRequestSizeHistogram(trackingMiddlwareOptionsAccessor.Value.RequestSizeHistogramBuckets);
            }
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
        // ReSharper restore UnusedMember.Global
        {
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.MiddlewareExecuting<OAuthTrackingMiddleware>();

                var clientid = context.GetOAuthClientIdIfRequired();

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
                        if (_useBucketHistograms)
                        {
                            _metrics.UpdateClientPostRequestSize(_postSizeBucketHistogramOptions, long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                        }
                        else
                        {
                            _metrics.UpdateClientPostRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                        }
                    }
                }
                else if (httpMethod == "PUT")
                {

                    if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                    {
                        if (_useBucketHistograms)
                        {
                            _metrics.UpdateClientPutRequestSize(_putSizeBucketHistogramOptions, long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                        }
                        else
                        {
                            _metrics.UpdateClientPutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                        }
                    }
                }

                _logger.MiddlewareExecuted<OAuthTrackingMiddleware>();
            }
        }
    }
}