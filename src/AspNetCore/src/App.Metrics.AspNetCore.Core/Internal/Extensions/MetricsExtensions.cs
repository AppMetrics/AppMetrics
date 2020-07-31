// <copyright file="MetricsExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Gauge;
using App.Metrics.Timer;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Provides extension methods on <see cref="IMetrics" /> to simplify measuring web application metrics.
    /// </summary>
    internal static class MetricsExtensions
    {
        /// <summary>
        ///     Decrements the number of active web requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        public static void DecrementActiveRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Decrement(HttpRequestMetricsRegistry.Counters.ActiveRequestCount);
        }

        /// <summary>
        ///     Increments the number of active active requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        public static void IncrementActiveRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Increment(HttpRequestMetricsRegistry.Counters.ActiveRequestCount);
        }

        /// <summary>
        ///     Increments the number of active active web socket requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        public static void IncrementActiveWSRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Increment(HttpRequestMetricsRegistry.Counters.ActiveWebSocketRequestCount);
        }

        /// <summary>
        ///     Decrements the number of active web socket requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        public static void DecrementActiveWSRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Decrement(HttpRequestMetricsRegistry.Counters.ActiveWebSocketRequestCount);
        }

        /// <summary>
        ///     Records metrics about a Clients HTTP request error, counts the total number of errors for each status code,
        ///     measures the
        ///     rate and percentage of HTTP error requests tagging by client id (if it exists) the endpoints route template and
        ///     HTTP status code.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="clientId">The OAuth2 client identifier.</param>
        public static void RecordClientHttpRequestError(
            this IMetrics metrics,
            string routeTemplate,
            int httpStatusCode,
            string clientId)
        {
            var tags = new MetricTags(
                new[] { MiddlewareConstants.DefaultTagKeys.ClientId, MiddlewareConstants.DefaultTagKeys.Route, MiddlewareConstants.DefaultTagKeys.HttpStatusCode },
                new[] { clientId, routeTemplate, httpStatusCode.ToString() });

            metrics.Measure.Meter.Mark(OAuthRequestMetricsRegistry.Meters.ErrorRate, tags);
        }

        public static void RecordClientRequestRate(this IMetrics metrics, string routeTemplate, string clientId)
        {
            var tags = new MetricTags(new[] { MiddlewareConstants.DefaultTagKeys.ClientId, MiddlewareConstants.DefaultTagKeys.Route }, new[] { clientId, routeTemplate });

            metrics.Measure.Meter.Mark(OAuthRequestMetricsRegistry.Meters.RequestRate, tags);
        }

        /// <summary>
        ///     Records the time taken to execute an API's endpoint in nanoseconds. Tags metrics by OAuth2 client id (if it exists)
        ///     and the endpoints route template.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="clientId">The OAuth2 client identifier, with track min/max durations by clientid.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        /// <param name="elapsed">The time elapsed in executing the endpoints request.</param>
        public static void RecordEndpointsRequestTime(this IMetrics metrics, string clientId, string routeTemplate, long elapsed)
        {
            metrics.EndpointRequestTimer(routeTemplate, null).
                    Record(
                        elapsed,
                        TimeUnit.Nanoseconds,
                        clientId.IsPresent() ? clientId : null);
        }

        /// <summary>
        ///     Records the time taken to execute an API's endpoint in nanoseconds. Tags metrics by OAuth2 client id (if it exists)
        ///     and the endpoints route template.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="bucketTimerOptions">The bucket timer options.</param>
        /// <param name="clientId">The OAuth2 client identifier, with track min/max durations by clientid.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        /// <param name="elapsed">The time elapsed in executing the endpoints request.</param>
        public static void RecordEndpointsRequestTime(this IMetrics metrics, BucketTimerOptions bucketTimerOptions, string clientId, string routeTemplate, long elapsed)
        {
            metrics.EndpointRequestTimer(routeTemplate, bucketTimerOptions).
                    Record(
                        elapsed,
                        TimeUnit.Nanoseconds,
                        clientId.IsPresent() ? clientId : null);
        }

        /// <summary>
        ///     Records metrics around unhanded exceptions, counts the total number of errors for each exception type.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        /// <param name="exception">The type of exception.</param>
        public static void RecordException(
            this IMetrics metrics,
            string routeTemplate,
            string exception)
        {
            var tags = new MetricTags(
                new[] { MiddlewareConstants.DefaultTagKeys.Route, MiddlewareConstants.DefaultTagKeys.Exception },
                new[] { routeTemplate, exception });
            metrics.Measure.Counter.Increment(HttpRequestMetricsRegistry.Counters.UnhandledExceptionCount, tags);
        }

        /// <summary>
        ///     Records metrics about an HTTP request error, counts the total number of errors for each status code, measures the
        ///     rate and percentage of HTTP error requests tagging by client id (if it exists) the endpoints route template and
        ///     HTTP status code.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="bucketTimerOptions">The Bucket Timer options.</param>
        public static void RecordHttpRequestError(
            this IMetrics metrics,
            string routeTemplate,
            int httpStatusCode, 
            BucketTimerOptions bucketTimerOptions)
        {
            CountOverallErrorRequestsByHttpStatusCode(metrics, httpStatusCode);

            metrics.Measure.Meter.Mark(HttpRequestMetricsRegistry.Meters.ErrorRequestRate);

            RecordEndpointsHttpRequestErrors(metrics, routeTemplate, httpStatusCode);
            RecordOverallPercentageOfErrorRequests(metrics, bucketTimerOptions);
            RecordEndpointsPercentageOfErrorRequests(metrics, routeTemplate, bucketTimerOptions);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP POST requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="value">The value.</param>
        /// <param name="clientId">The OAuth2 client identifier.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        public static void UpdateClientPostRequestSize(this IMetrics metrics, long value, string clientId, string routeTemplate)
        {
            var tags = new MetricTags(new[] { MiddlewareConstants.DefaultTagKeys.ClientId, MiddlewareConstants.DefaultTagKeys.Route }, new[] { clientId, routeTemplate });
            metrics.Measure.Histogram.Update(OAuthRequestMetricsRegistry.Histograms.PostRequestSizeHistogram, tags, value);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP PUT requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="value">The value.</param>
        /// <param name="clientId">The OAuth2 client identifier to tag the histogram values.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        public static void UpdateClientPutRequestSize(this IMetrics metrics, long value, string clientId, string routeTemplate)
        {
            var tags = new MetricTags(new[] { MiddlewareConstants.DefaultTagKeys.ClientId, MiddlewareConstants.DefaultTagKeys.Route }, new[] { clientId, routeTemplate });
            metrics.Measure.Histogram.Update(OAuthRequestMetricsRegistry.Histograms.PutRequestSizeHistogram, tags, value);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP POST requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="value">The value.</param>
        public static void UpdatePostRequestSize(this IMetrics metrics, long value)
        {
            metrics.Measure.Histogram.Update(HttpRequestMetricsRegistry.Histograms.PostRequestSizeHistogram, value);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP PUT requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="value">The value.</param>
        public static void UpdatePutRequestSize(this IMetrics metrics, long value)
        {
            metrics.Measure.Histogram.Update(HttpRequestMetricsRegistry.Histograms.PutRequestSizeHistogram, value);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP POST requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="bucketHistogramOptions">The bucket histogram options</param>
        /// <param name="value">The value.</param>
        /// <param name="clientId">The OAuth2 client identifier.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        public static void UpdateClientPostRequestSize(this IMetrics metrics, BucketHistogramOptions bucketHistogramOptions, long value, string clientId, string routeTemplate)
        {
            var tags = new MetricTags(new[] { MiddlewareConstants.DefaultTagKeys.ClientId, MiddlewareConstants.DefaultTagKeys.Route }, new[] { clientId, routeTemplate });
            metrics.Measure.BucketHistogram.Update(bucketHistogramOptions, tags, value);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP PUT requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="bucketHistogramOptions">The bucket histogram options</param>
        /// <param name="value">The value.</param>
        /// <param name="clientId">The OAuth2 client identifier to tag the histogram values.</param>
        /// <param name="routeTemplate">The route template of the endpoint.</param>
        public static void UpdateClientPutRequestSize(this IMetrics metrics, BucketHistogramOptions bucketHistogramOptions, long value, string clientId, string routeTemplate)
        {
            var tags = new MetricTags(new[] { MiddlewareConstants.DefaultTagKeys.ClientId, MiddlewareConstants.DefaultTagKeys.Route }, new[] { clientId, routeTemplate });
            metrics.Measure.BucketHistogram.Update(bucketHistogramOptions, tags, value);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP POST requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="bucketHistogramOptions">The bucket histogram options</param>
        /// <param name="value">The value.</param>
        public static void UpdatePostRequestSize(this IMetrics metrics, BucketHistogramOptions bucketHistogramOptions, long value)
        {
            metrics.Measure.BucketHistogram.Update(bucketHistogramOptions, value);
        }

        /// <summary>
        ///     Records a metric for the size of a HTTP PUT requests.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <param name="bucketHistogramOptions">The bucket histogram options</param>
        /// <param name="value">The value.</param>
        public static void UpdatePutRequestSize(this IMetrics metrics, BucketHistogramOptions bucketHistogramOptions, long value)
        {
            metrics.Measure.BucketHistogram.Update(bucketHistogramOptions, value);
        }

        private static void CountOverallErrorRequestsByHttpStatusCode(IMetrics metrics, int httpStatusCode)
        {
            var errorCounterTags = new MetricTags(MiddlewareConstants.DefaultTagKeys.HttpStatusCode, httpStatusCode.ToString());
            metrics.Measure.Counter.Increment(HttpRequestMetricsRegistry.Counters.TotalErrorRequestCount, errorCounterTags);
        }

        private static ITimer EndpointRequestTimer(this IMetrics metrics, string routeTemplate, BucketTimerOptions bucketTimerOptions)
        {
            var tags = new MetricTags(MiddlewareConstants.DefaultTagKeys.Route, routeTemplate);
            if (bucketTimerOptions != null)
            {
                return metrics.Provider.BucketTimer.Instance(bucketTimerOptions, tags);
            }
            return metrics.Provider.Timer.Instance(HttpRequestMetricsRegistry.Timers.EndpointRequestTransactionDuration, tags);
        }

        private static ITimer RequestTimer(this IMetrics metrics, BucketTimerOptions bucketTimerOptions)
        {
            if (bucketTimerOptions != null)
            {
                return metrics.Provider.BucketTimer.Instance(bucketTimerOptions);
            }
            return metrics.Provider.Timer.Instance(HttpRequestMetricsRegistry.Timers.RequestTransactionDuration);
        }

        private static void RecordEndpointsHttpRequestErrors(IMetrics metrics, string routeTemplate, int httpStatusCode)
        {
            var endpointErrorRequestTags = new MetricTags(MiddlewareConstants.DefaultTagKeys.Route, routeTemplate);
            metrics.Measure.Meter.Mark(HttpRequestMetricsRegistry.Meters.EndpointErrorRequestRate, endpointErrorRequestTags);

            var endpointErrorRequestPerStatusCodeTags = new MetricTags(
                new[] { MiddlewareConstants.DefaultTagKeys.Route, MiddlewareConstants.DefaultTagKeys.HttpStatusCode },
                new[] { routeTemplate, httpStatusCode.ToString() });

            metrics.Measure.Meter.Mark(
                HttpRequestMetricsRegistry.Meters.EndpointErrorRequestPerStatusCodeRate,
                endpointErrorRequestPerStatusCodeTags);
        }

        private static void RecordEndpointsPercentageOfErrorRequests(IMetrics metrics, string routeTemplate, BucketTimerOptions bucketTimerOptions)
        {
            var tags = new MetricTags(MiddlewareConstants.DefaultTagKeys.Route, routeTemplate);

            var endpointsErrorRate = metrics.Provider.Meter.Instance(HttpRequestMetricsRegistry.Meters.EndpointErrorRequestRate, tags);
            var endpointsRequestTransactionTime = metrics.EndpointRequestTimer(routeTemplate, bucketTimerOptions);

            metrics.Measure.Gauge.SetValue(
                HttpRequestMetricsRegistry.Gauges.EndpointOneMinuteErrorPercentageRate,
                tags,
                () => new HitPercentageGauge(endpointsErrorRate, endpointsRequestTransactionTime, m => m.OneMinuteRate));
        }

        private static void RecordOverallPercentageOfErrorRequests(IMetrics metrics, BucketTimerOptions bucketTimerOptions)
        {
            var totalErrorRate = metrics.Provider.Meter.Instance(HttpRequestMetricsRegistry.Meters.ErrorRequestRate);
            var overallRequestTransactionTime = metrics.RequestTimer(bucketTimerOptions);

            metrics.Measure.Gauge.SetValue(
                HttpRequestMetricsRegistry.Gauges.OneMinErrorPercentageRate,
                () => new HitPercentageGauge(totalErrorRate, overallRequestTransactionTime, m => m.OneMinuteRate));
        }
    }
}