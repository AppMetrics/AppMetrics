using System;
using App.Metrics;
using AspNet.Metrics.Internal;

// ReSharper disable CheckNamespace

namespace AppMetrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        public static IMetricsContext DecrementActiveRequests(this IMetricsContext metrics)
        {
            metrics.Decrement(AspNetMetricsRegistry.Groups.WebRequests.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetricsContext IncrementActiveRequests(this IMetricsContext metrics)
        {
            metrics.Increment(AspNetMetricsRegistry.Groups.WebRequests.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetricsContext MarkBadRequest(this IMetricsContext metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.EndpointBadRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.TotalBadRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.EndpointBadRequests(routeTemplate), clientId);

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.TotalBadRequests, clientId);

            return metrics;
        }

        public static IMetricsContext MarkEndpointRequest(this IMetricsContext metrics, string clientId, string routeTemplate)
        {
            metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.EndpointWebRequests(routeTemplate), clientId);

            return metrics;
        }

        public static IMetricsContext MarkInternalServerErrorRequest(this IMetricsContext metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.EndpointInternalErrorRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.TotalInternalErrorRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.EndpointInternalServerErrorRequests(routeTemplate));

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.TotalInternalServerErrorRequests);

            return metrics;
        }

        public static IMetricsContext MarkOverallWebRequestError(this IMetricsContext metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.EndpointTotalErrorRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.TotalErrorRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.TotalErrorRequests);

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.EndpointTotalErrorRequests(routeTemplate));

            return metrics;
        }

        public static IMetricsContext MarkRequest(this IMetricsContext metrics, string clientId)
        {
            metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.WebRequests, clientId);

            return metrics;
        }

        public static IMetricsContext MarkUnAuthorizedRequest(this IMetricsContext metrics, string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.EndpointUnAuthorizedRequests(routeTemplate), clientId);

                metrics.Mark(AspNetMetricsRegistry.Groups.OAuth2.Meters.TotalUnAuthorizedRequests, clientId);
            }

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.EndpointUnAuthorizedRequests(routeTemplate));

            metrics.Mark(AspNetMetricsRegistry.Groups.WebRequests.Meters.TotalUnAuthorizedRequests);

            return metrics;
        }

        public static IMetricsContext RecordEndpointRequestTime(this IMetricsContext metrics, string clientId, string routeTemplate, long elapsed)
        {
            metrics.Advanced
                .Timer(AspNetMetricsRegistry.Groups.WebRequests.Timers.EndpointPerRequestTimer(routeTemplate))
                .Record(elapsed, TimeUnit.Nanoseconds, clientId.IsPresent() ? clientId : null);

            return metrics;
        }

        public static IMetricsContext UpdatePostAndPutRequestSize(this IMetricsContext metrics, long value)
        {
            metrics.Update(AspNetMetricsRegistry.Groups.WebRequests.Histograms.PostAndPutRequestSize, value);
            return metrics;
        }
    }
}