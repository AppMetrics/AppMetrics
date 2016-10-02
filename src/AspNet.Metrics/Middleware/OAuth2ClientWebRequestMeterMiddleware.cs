using System;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    /// <summary>
    ///     Measures the overall request rate of each OAuth2 Client as well as the rate per endpoint
    /// </summary>
    /// <seealso cref="AspNet.Metrics.Middleware.MetricsMiddlewareBase" />
    public class OAuth2ClientWebRequestMeterMiddleware : MetricsMiddlewareBase
    {
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;

        public OAuth2ClientWebRequestMeterMiddleware(RequestDelegate next, AspNetMetricsOptions options,
            AspNetMetricsContext metricsContext)
            : base(options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            _next = next;
            _metricsContext = metricsContext;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (PerformMetric(context))
            {
                var clientId = context.OAuthClientId();

                if (clientId.IsPresent())
                {
                    var routeTemplate = context.GetMetricsCurrentRouteName();

                    MarkWebRequest(routeTemplate, clientId);
                }
            }
        }

        private void MarkWebRequest(string routeTemplate, string clientId)
        {
            _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                .Meter(routeTemplate, Unit.Requests)
                .Mark(clientId);

            _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                .Meter("Total Web Requests", Unit.Requests)
                .Mark(clientId);
        }
    }
}