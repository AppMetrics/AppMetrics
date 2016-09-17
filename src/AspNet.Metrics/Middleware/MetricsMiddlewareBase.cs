using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public abstract class MetricsMiddlewareBase
    {
        private readonly MetricsOptions _options;
        protected const string ApplicationRequestsContextName = "Application.WebRequests";
        protected const string ApplicationOauth2RequestsContextName = "Application.Client.WebRequests";

        protected MetricsMiddlewareBase(MetricsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options;
        }

        protected bool PerformMetric(HttpContext context)
        {
            if (_options.IgnoredRequestPatterns == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(context.Request.Path))
            {
                return false;
            }

            return !_options.IgnoredRequestPatterns.Any(ignorePattern => ignorePattern.IsMatch(context.Request.Path.ToString().TrimStart('/')));
        }
    }
}