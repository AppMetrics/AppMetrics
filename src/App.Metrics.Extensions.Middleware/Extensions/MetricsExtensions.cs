// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Gauge;
using App.Metrics.Tagging;

// ReSharper disable CheckNamespace
namespace App.Metrics
{
    // ReSharper restore CheckNamespace
    internal static class MetricsExtensions
    {
        public static IMetrics DecrementActiveRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Decrement(HttpRequestMetricsRegistry.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetrics ErrorRequestPercentage(this IMetrics metrics)
        {
            var errors = metrics.Provider.Meter.Instance(HttpRequestMetricsRegistry.Meters.HttpErrorRequests);
            var requests = metrics.Provider.Timer.Instance(HttpRequestMetricsRegistry.Timers.WebRequestTimer);

            metrics.Measure.Gauge.SetValue(
                HttpRequestMetricsRegistry.Gauges.PercentageErrorRequests,
                () => new HitPercentageGauge(errors, requests, m => m.OneMinuteRate));

            return metrics;
        }

        public static IMetrics IncrementActiveRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Increment(HttpRequestMetricsRegistry.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetrics MarkHttpEndpointForOAuthClient(this IMetrics metrics, string routeTemplate, string clientId, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                OAuth2MetricsRegistry.Meters.EndpointHttpRequests(routeTemplate),
                new MetricSetItem(new[] { "client_id", "http_status_code" }, new[] { clientId, httpStatusCode.ToString() }));

            return metrics;
        }

        public static IMetrics MarkHttpRequestEndpointError(this IMetrics metrics, string routeTemplate, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                HttpRequestMetricsRegistry.Meters.EndpointHttpErrorRequests(routeTemplate),
                new MetricSetItem("http_status_code", httpStatusCode.ToString()));

            return metrics;
        }

        public static IMetrics MarkHttpRequestError(this IMetrics metrics, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                HttpRequestMetricsRegistry.Meters.HttpErrorRequests,
                new MetricSetItem("http_status_code", httpStatusCode.ToString()));

            return metrics;
        }

        public static IMetrics MarkHttpRequestForOAuthClient(this IMetrics metrics, string clientId, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                OAuth2MetricsRegistry.Meters.HttpRequests,
                new MetricSetItem(new[] { "client_id", "http_status_code" }, new[] { clientId, httpStatusCode.ToString() }));

            return metrics;
        }

        public static IMetrics RecordEndpointRequestTime(this IMetrics metrics, string clientId, string routeTemplate, long elapsed)
        {
            metrics.Provider
                   .Timer
                   .Instance(HttpRequestMetricsRegistry.Timers.EndpointPerRequestTimer(routeTemplate))
                   .Record(elapsed, TimeUnit.Nanoseconds, clientId.IsPresent() ? clientId : null);

            return metrics;
        }

        public static IMetrics UpdatePostAndPutRequestSize(this IMetrics metrics, long value)
        {
            metrics.Measure.Histogram.Update(HttpRequestMetricsRegistry.Histograms.PostAndPutRequestSize, value);

            return metrics;
        }
    }
}