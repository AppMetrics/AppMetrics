using System;
using System.Threading.Tasks;
using Metrics.Visualization;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointVisualizationEndpointMiddleware : MetricsEndpointMiddlewareBase
    {
        private readonly RequestDelegate _next;
        private readonly MetricsOptions _options;

        public MetricsEndpointVisualizationEndpointMiddleware(RequestDelegate next, MetricsOptions options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext context)
        {
            if (_options.MetricsEndpoint != "/json" && _options.MetricsVisualisationEnabled)
            {
                throw new NotImplementedException(
                    "metrics visualisation currently requires the metrics endpoint to be configured as /json. Disable metrics visualisation or change the metrics endpoint path.");
            }

            if (_options.HealthEndpoint != "/health" && _options.MetricsVisualisationEnabled)
            {
                throw new NotImplementedException(
                    "metrics visualisation currently requires the health endpoint to be configured as /health. Disable metrics visualisation or change the health check endpoint path.");
            }

            if (_options.MetricsVisualisationEnabled && _options.MetricsVisualizationEndpoint.HasValue &&
                _options.MetricsVisualizationEndpoint == context.Request.Path)
            {
                var content = FlotWebApp.GetFlotApp();
                return WriteResponse(context, content, "text/html");
            }

            return _next(context);
        }
    }
}