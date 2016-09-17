using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AspNet.Metrics.Middleware
{
    public class OAuth2ClientRequestRateMiddleware : MetricsMiddlewareBase
    {
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;

        public OAuth2ClientRequestRateMiddleware(RequestDelegate next, MetricsOptions options,
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
                var clientId = GetClientId(context);

                if (!string.IsNullOrWhiteSpace(clientId))
                {
                    var statusCode = context.Response.StatusCode;
                    var routeTemplate = context.GetMetricsCurrentRouteName();

                    if ((statusCode >= (int)HttpStatusCode.OK) && (statusCode <= 299))
                    {
                        _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                            .Meter(routeTemplate, Unit.Requests).Mark(clientId);

                        _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                            .Meter("Web Requests", Unit.Requests)
                            .Mark(clientId);
                    }
                    else
                    {
                        switch (context.Response.StatusCode)
                        {
                            case (int)HttpStatusCode.InternalServerError:

                                _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                                    .Meter($"{routeTemplate} Error Requests", Unit.Custom("500 Errors"))
                                    .Mark(clientId);

                                _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                                    .Meter("Web Error Requests", Unit.Custom("500 Errors"))
                                    .Mark(clientId);

                                break;
                            case (int)HttpStatusCode.BadRequest:
                                _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                                    .Meter($"{routeTemplate} Bad Requests", Unit.Custom("400 Errors"))
                                    .Mark(clientId);

                                _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                                    .Meter("Web Bad Requests", Unit.Custom("400 Errors"))
                                    .Mark(clientId);
                                break;
                            case (int)HttpStatusCode.Unauthorized:
                                _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                                    .Meter($"{routeTemplate} Unauthorized Requests", Unit.Custom("401 Errors"))
                                    .Mark(clientId);

                                _metricsContext.Context.Context(ApplicationOauth2RequestsContextName)
                                    .Meter("Web Unauthorized Requests", Unit.Custom("401 Errors"))
                                    .Mark(clientId);
                                break;
                        }
                    }
                }
            }
        }

        private static string GetClientId(HttpContext context)
        {
            var claimsPrincipal = context.User;

            var clientId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == "client_id");

            return clientId?.Value;
        }
    }
}