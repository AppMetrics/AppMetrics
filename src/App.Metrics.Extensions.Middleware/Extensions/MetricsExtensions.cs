// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Gauge;
using App.Metrics.Tagging;
using App.Metrics.Timer.Abstractions;

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

        public static ITimer EndpointRequestTimer(this IMetrics metrics, string routeTemplate, string clientId = null)
        {
            var tags = clientId.IsMissing()
                ? new MetricTags("route", routeTemplate)
                : new MetricTags(new[] { "route", "client_id" }, new[] { routeTemplate, clientId });

            return metrics.Provider.Timer.Instance(HttpRequestMetricsRegistry.Timers.HttpRequestTransactions, tags);
        }

        public static IMetrics ErrorRequestPercentage(this IMetrics metrics, string routeTemplate, string clientId = null)
        {
            var tags = clientId.IsMissing()
                ? new MetricTags("route", routeTemplate)
                : new MetricTags(new[] { "route", "clientid" }, new[] { routeTemplate, clientId });

            var errors = metrics.Provider.Meter.Instance(HttpRequestMetricsRegistry.Meters.OverallHttpErrorRequests, tags);
            var requests = metrics.EndpointRequestTimer(routeTemplate, clientId);

            metrics.Measure.Gauge.SetValue(
                HttpRequestMetricsRegistry.Gauges.PercentageErrorRequests,
                tags,
                () => new HitPercentageGauge(errors, requests, m => m.OneMinuteRate));

            return metrics;
        }

        public static IMetrics IncrementActiveRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Increment(HttpRequestMetricsRegistry.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetrics MarkHttpRequestError(
            this IMetrics metrics,
            string routeTemplate,
            int httpStatusCode,
            string clientId = null)
        {
            var tags = clientId.IsMissing()
                ? new MetricTags(
                    new[] { "route", "http_status_code" },
                    new[] { routeTemplate, httpStatusCode.ToString() })
                : new MetricTags(
                    new[] { "route", "http_status_code", "client_id" },
                    new[] { routeTemplate, httpStatusCode.ToString(), clientId });

            metrics.Measure.Meter.Mark(
                HttpRequestMetricsRegistry.Meters.HttpErrorRequests,
                tags);

            var overallTags = clientId.IsMissing()
                ? new MetricTags("route", routeTemplate)
                : new MetricTags(
                    new[] { "route", "client_id" },
                    new[] { routeTemplate, clientId });

            metrics.Measure.Meter.Mark(
                HttpRequestMetricsRegistry.Meters.OverallHttpErrorRequests,
                overallTags);

            return metrics;
        }

        public static IMetrics RecordEndpointRequestTime(this IMetrics metrics, string clientId, string routeTemplate, long elapsed)
        {
            metrics.EndpointRequestTimer(routeTemplate, clientId).
                    Record(
                        elapsed,
                        TimeUnit.Nanoseconds,
                        clientId.IsPresent() ? clientId : null);

            return metrics;
        }

        public static IMetrics UpdatePostAndPutRequestSize(this IMetrics metrics, long value)
        {
            metrics.Measure.Histogram.Update(HttpRequestMetricsRegistry.Histograms.PostAndPutRequestSize, value);

            return metrics;
        }
    }
}