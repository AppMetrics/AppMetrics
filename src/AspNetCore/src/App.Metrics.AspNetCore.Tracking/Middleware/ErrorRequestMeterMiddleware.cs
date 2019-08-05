// <copyright file="ErrorRequestMeterMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.BucketTimer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    /// <summary>
    ///     Measures the overall error request rate as well as the rate per endpoint.
    ///     Also measures these error rates per OAuth2 Client as a separate metric
    /// </summary>
    // ReSharper disable ClassNeverInstantiated.Global
    public class ErrorRequestMeterMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorRequestMeterMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly IList<int> _ignoredHttpStatusCodes;
        private readonly BucketTimerOptions _bucketTimerOptions;

        public ErrorRequestMeterMiddleware(
            RequestDelegate next,
            IOptions<MetricsWebTrackingOptions> trackingMiddlwareOptionsAccessor,
            ILogger<ErrorRequestMeterMiddleware> logger,
            IMetrics metrics)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _metrics = metrics;
            _ignoredHttpStatusCodes = trackingMiddlwareOptionsAccessor.Value.IgnoredHttpStatusCodes;
            if (trackingMiddlwareOptionsAccessor.Value.UseBucketHistograms)
            {
                _bucketTimerOptions =
                    HttpRequestMetricsRegistry.BucketTimers.EndpointRequestTransactionDuration(
                        trackingMiddlwareOptionsAccessor.Value.RequestTimeHistogramBuckets);
            }
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            try
            {
                _logger.MiddlewareExecuting<ErrorRequestMeterMiddleware>();

                await _next(context);

                var routeTemplate = context.GetMetricsCurrentRouteName();

                if (!context.Response.IsSuccessfulResponse() && _ignoredHttpStatusCodes.All(i => i != context.Response.StatusCode))
                {
                    _metrics.RecordHttpRequestError(routeTemplate, context.Response.StatusCode, _bucketTimerOptions);
                }
            }
            catch (Exception exception)
            {
                var routeTemplate = context.GetMetricsCurrentRouteName();

                if (_ignoredHttpStatusCodes.All(i => i != StatusCodes.Status500InternalServerError))
                {
                    _metrics.RecordHttpRequestError(routeTemplate, StatusCodes.Status500InternalServerError, _bucketTimerOptions);
                }

                _metrics.RecordException(routeTemplate, exception.GetType().FullName);

                throw;
            }
            finally
            {
                _logger.MiddlewareExecuted<ErrorRequestMeterMiddleware>();
            }
        }
    }
}