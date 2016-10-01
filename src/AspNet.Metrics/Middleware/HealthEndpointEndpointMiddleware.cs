using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.Json;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class HealthEndpointEndpointMiddleware : MetricsEndpointMiddlewareBase
    {
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;
        private readonly MetricsOptions _options;

        public HealthEndpointEndpointMiddleware(RequestDelegate next, MetricsOptions options, AspNetMetricsContext metricsContext)
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
            _options = options;
            _metricsContext = metricsContext;
        }

        public Task Invoke(HttpContext context)
        {
            if (_options.HealthEnabled && _options.HealthEndpoint.HasValue && _options.HealthEndpoint == context.Request.Path)
            {
                var healthStatus = _metricsContext.HealthStatus();
                var responseStatusCode = healthStatus.IsHealthy ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                return WriteResponse(context, JsonHealthChecks.BuildJson(healthStatus), "application/json", responseStatusCode);
            }

            return _next(context);
        }
    }
}