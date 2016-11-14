// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using AspNet.Metrics.Internal;

// ReSharper disable CheckNamespace
namespace AppMetrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        public static IMetrics DecrementActiveRequests(this IMetrics metrics)
        {
            metrics.Decrement(AspNetMetricsRegistry.Contexts.WebRequests.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetrics IncrementActiveRequests(this IMetrics metrics)
        {
            metrics.Increment(AspNetMetricsRegistry.Contexts.WebRequests.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetrics MarkBadRequest(this IMetrics metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.EndpointBadRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.TotalBadRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.EndpointBadRequests(routeTemplate), clientId);

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.TotalBadRequests, clientId);

            return metrics;
        }

        public static IMetrics MarkEndpointRequest(this IMetrics metrics, string clientId, string routeTemplate)
        {
            metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.EndpointWebRequests(routeTemplate), clientId);

            return metrics;
        }

        public static IMetrics MarkInternalServerErrorRequest(this IMetrics metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.EndpointInternalErrorRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.TotalInternalErrorRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.EndpointInternalServerErrorRequests(routeTemplate));

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.TotalInternalServerErrorRequests);

            return metrics;
        }

        public static IMetrics MarkOverallWebRequestError(this IMetrics metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.EndpointTotalErrorRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.TotalErrorRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.TotalErrorRequests);

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.EndpointTotalErrorRequests(routeTemplate));

            return metrics;
        }

        public static IMetrics MarkRequest(this IMetrics metrics, string clientId)
        {
            metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.WebRequests, clientId);

            return metrics;
        }

        public static IMetrics MarkUnAuthorizedRequest(this IMetrics metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.EndpointUnAuthorizedRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Contexts.OAuth2.Meters.TotalUnAuthorizedRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.EndpointUnAuthorizedRequests(routeTemplate));

            metrics.Mark(AspNetMetricsRegistry.Contexts.WebRequests.Meters.TotalUnAuthorizedRequests);

            return metrics;
        }

        public static IMetrics RecordEndpointRequestTime(this IMetrics metrics, string clientId, string routeTemplate, long elapsed)
        {
            metrics.Advanced
                .Timer(AspNetMetricsRegistry.Contexts.WebRequests.Timers.EndpointPerRequestTimer(routeTemplate))
                .Record(elapsed, TimeUnit.Nanoseconds, clientId.IsPresent() ? clientId : null);

            return metrics;
        }

        public static IMetrics UpdatePostAndPutRequestSize(this IMetrics metrics, long value)
        {
            metrics.Update(AspNetMetricsRegistry.Contexts.WebRequests.Histograms.PostAndPutRequestSize, value);
            return metrics;
        }
    }
}