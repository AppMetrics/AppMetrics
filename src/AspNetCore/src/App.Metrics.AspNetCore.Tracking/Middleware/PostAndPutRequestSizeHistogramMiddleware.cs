// <copyright file="PostAndPutRequestSizeHistogramMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
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
    public class PostAndPutRequestSizeHistogramMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PostAndPutRequestSizeHistogramMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly bool _useBucketHistograms;
        private readonly BucketHistogramOptions _postSizeBucketHistogramOptions;
        private readonly BucketHistogramOptions _putSizeBucketHistogramOptions;

        public PostAndPutRequestSizeHistogramMiddleware(
            RequestDelegate next,
            IOptions<MetricsWebTrackingOptions> trackingMiddlwareOptionsAccessor,
            ILogger<PostAndPutRequestSizeHistogramMiddleware> logger,
            IMetrics metrics)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _metrics = metrics;
            if (trackingMiddlwareOptionsAccessor.Value.UseBucketHistograms)
            {
                _useBucketHistograms = true;
                _postSizeBucketHistogramOptions = HttpRequestMetricsRegistry.BucketHistograms.PostRequestSizeHistogram(trackingMiddlwareOptionsAccessor.Value.RequestSizeHistogramBuckets);
                _putSizeBucketHistogramOptions = HttpRequestMetricsRegistry.BucketHistograms.PutRequestSizeHistogram(trackingMiddlwareOptionsAccessor.Value.RequestSizeHistogramBuckets);
            }
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
                    if (_useBucketHistograms)
                    {
                        _metrics.UpdatePostRequestSize(_postSizeBucketHistogramOptions, long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                    else
                    {
                        _metrics.UpdatePostRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                }
            }
            else if (httpMethod == "PUT")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    if (_useBucketHistograms)
                    {
                        _metrics.UpdatePutRequestSize(_putSizeBucketHistogramOptions, long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                    else
                    {
                        _metrics.UpdatePutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                }
            }

            _logger.MiddlewareExecuted<PostAndPutRequestSizeHistogramMiddleware>();

            return _next(context);
        }
    }
}