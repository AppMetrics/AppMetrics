using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public abstract class MetricsEndpointMiddlewareBase
    {
        protected Task WriteResponse(HttpContext context, string content, string contentType, HttpStatusCode code = HttpStatusCode.OK)
        {
            context.Response.Headers["Content-Type"] = new[] { contentType };
            context.Response.Headers["Cache-Control"] = new[] { "no-cache, no-store, must-revalidate" };
            context.Response.Headers["Pragma"] = new[] { "no-cache" };
            context.Response.Headers["Expires"] = new[] { "0" };
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(content);
        }
    }
}