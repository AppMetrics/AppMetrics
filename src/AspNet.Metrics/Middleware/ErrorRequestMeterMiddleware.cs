using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    /// <summary>
    ///     Measures the overall error request rate as well as the rate per endpoint.
    ///     Also measures these error rates per OAuth2 Client as a separate metric
    /// </summary>
    /// <seealso cref="AspNet.Metrics.Middleware.MetricsMiddlewareBase" />
    public class ErrorRequestMeterMiddleware : MetricsMiddlewareBase
    {
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;

        public ErrorRequestMeterMiddleware(RequestDelegate next, AspNetMetricsOptions options,
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

                var routeTemplate = context.GetMetricsCurrentRouteName();

                if (!context.Response.IsSuccessfulResponse())
                {
                    MarkOverallWebRequestError(clientId, routeTemplate);

                    switch (context.Response.StatusCode)
                    {
                        case (int)HttpStatusCode.InternalServerError:
                            MarkInternalServerErrorRequest(routeTemplate, clientId);
                            break;
                        case (int)HttpStatusCode.BadRequest:
                            MarkBadRequest(routeTemplate, clientId);
                            break;
                        case (int)HttpStatusCode.Unauthorized:
                            MarkUnAuthorizedRequest(routeTemplate, clientId);
                            break;
                    }
                }
            }
        }

        private void MarkBadRequest(string routeTemplate, string clientId)
        {
            if (clientId.IsPresent())
            {
                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter($"{routeTemplate} Bad Requests", Unit.Custom("400 Errors"))
                    .Mark(clientId);

                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter("Total Bad Requests", Unit.Custom("400 Errors"))
                    .Mark(clientId);
            }

            _metricsContext.Context.GetWebApplicationContext()
                .Meter($"{routeTemplate} Bad Requests", Unit.Custom("400 Errors"))
                .Mark();

            _metricsContext.Context.GetWebApplicationContext()
                .Meter("Total Bad Requests", Unit.Custom("400 Errors"))
                .Mark();
        }

        private void MarkInternalServerErrorRequest(string routeTemplate, string clientId)
        {
            if (clientId.IsPresent())
            {
                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter($"{routeTemplate} Internal Server Error Requests", Unit.Custom("500 Errors"))
                    .Mark(clientId);

                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter("Total Internal Server Error Requests", Unit.Custom("500 Errors"))
                    .Mark(clientId);
            }

            _metricsContext.Context.GetWebApplicationContext()
                .Meter($"{routeTemplate} Internal Server Error Requests", Unit.Custom("500 Errors"))
                .Mark();

            _metricsContext.Context.GetWebApplicationContext()
                .Meter("Total Internal Server Error Requests", Unit.Custom("500 Errors"))
                .Mark();
        }

        private void MarkOverallWebRequestError(string clientId, string routeTemplate)
        {
            if (clientId.IsPresent())
            {
                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter("Total Error Requests", Unit.Errors)
                    .Mark(clientId);

                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter($"{routeTemplate} Total Error Requests", Unit.Errors)
                    .Mark(clientId);
            }

            _metricsContext.Context.GetWebApplicationContext()
                .Meter("Total Error Requests", Unit.Errors).Mark();

            _metricsContext.Context.GetWebApplicationContext()
                .Meter($"{routeTemplate} Total Error Requests", Unit.Errors).Mark();
        }

        private void MarkUnAuthorizedRequest(string routeTemplate, string clientId)
        {
            if (clientId.IsPresent())
            {
                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter($"{routeTemplate} Unauthorized Requests", Unit.Custom("401 Errors"))
                    .Mark(clientId);

                _metricsContext.Context.GetOAuth2ClientWebRequestsContext()
                    .Meter("Total Unauthorized Requests", Unit.Custom("401 Errors"))
                    .Mark(clientId);
            }

            _metricsContext.Context.GetWebApplicationContext()
                .Meter($"{routeTemplate} Unauthorized Requests", Unit.Custom("401 Errors"))
                .Mark();

            _metricsContext.Context.GetWebApplicationContext()
                .Meter("Total Unauthorized Requests", Unit.Custom("401 Errors"))
                .Mark();
        }
    }
}