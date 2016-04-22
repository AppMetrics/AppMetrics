using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace AspNet.Metrics.Middleware
{
    public class PingEndpointEndpointMiddleware : MetricsEndpointMiddlewareBase
    {
        private readonly RequestDelegate _next;
        private readonly MetricsOptions _options;

        public PingEndpointEndpointMiddleware(RequestDelegate next, MetricsOptions options)
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
            if (_options.PingEnabled && _options.PingEndpoint.HasValue && _options.PingEndpoint == context.Request.Path)
            {
                return WriteResponse(context, "pong", "text/plain");
            }

            return _next(context);
        }
    }
}