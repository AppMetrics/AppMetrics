using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public abstract class AppMetricsMiddleware<TOptions> where TOptions : AspNetMetricsOptions, new()
    {
        protected AppMetricsMiddleware(RequestDelegate next,
            IOptions<TOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            Options = options.Value;
            Logger = loggerFactory.CreateLogger(this.GetType().FullName);
            MetricsContext = metricsContext;

            Next = next;
        }

        public ILogger Logger { get; set; }

        public IMetricsContext MetricsContext { get; set; }

        public RequestDelegate Next { get; set; }

        public TOptions Options { get; set; }

        protected bool PerformMetric(HttpContext context)
        {
            if (Options.IgnoredRequestPatterns == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(context.Request.Path))
            {
                return false;
            }

            return !Options.IgnoredRequestPatterns.Any(ignorePattern => ignorePattern.IsMatch(context.Request.Path.ToString().TrimStart('/')));
        }

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