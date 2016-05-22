using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Logging
{
    internal static class MetricsRouteHandlerLoggerExtensions
    {
        public static void AttributeTemplateFound(this ILogger logger, string template)
        {
            logger.LogTrace(1, $"Found AttributeRouteTemplate {template}");
        }

        public static void TemplateRouteFound(this ILogger logger, string template)
        {
            logger.LogTrace(1, $"Found TemplateRoute {template}");
        }
    }
}